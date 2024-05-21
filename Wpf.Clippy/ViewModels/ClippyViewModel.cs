using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Clippy.Types;

namespace Wpf.Clippy.ViewModels
{
    public class ClippyViewModel : INotifyPropertyChanged
    {
        private readonly CharacterData m_data;
        private readonly CancellationTokenSource m_cancellationTokenSource;

        private CharacterData.CharacterAnimation m_activeAnimation;
        private string m_loopingAnimation;
        private string m_playOnceAnimation;
        private int m_frameIndex;
        private Point m_frameCoords;
        private Rect m_frameRect;
        private Visibility m_canvasVisibility = Visibility.Hidden;

        private Popup m_speechPopup;
        private ClippyMessage m_activeMessage;

        public delegate void ClippyViewModelAnimationCompletedEventHandler(ClippyViewModel sender, string animationName, AnimationMode mode);
        public event ClippyViewModelAnimationCompletedEventHandler OnAnimationCompleted;

        public IReadOnlyCollection<string> AnimationNames => m_data.Animations.Keys;

        public string ActiveAnimation
        {
            get
            {
                if (m_playOnceAnimation != null)
                {
                    return m_playOnceAnimation;
                }
                return m_loopingAnimation;
            }
        }

        public ClippyMessage ActiveMessage
        {
            get => m_activeMessage;
            private set
            {
                if (SetField(ref m_activeMessage, value))
                {
                    Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        m_speechPopup.IsOpen = true;
                    });
                }
            }
        }

        public ImageSource ImageMap { get; }

        public Rect FrameRect
        {
            get => m_frameRect;
            set => SetField(ref m_frameRect, value);
        }

        public Point FrameCoords
        {
            get => m_frameCoords;
            set
            {
                if (SetField(ref m_frameCoords, value))
                {
                    FrameRect = new Rect(
                        -m_frameCoords.X, -m_frameCoords.Y,
                        FrameRect.Width, FrameRect.Height
                        );
                }
            }
        }

        public Visibility CanvasVisibility
        {
            get => m_canvasVisibility;
            set => SetField(ref m_canvasVisibility, value);
        }

        public ClippyViewModel(Character character)
        {
            m_data = LoadCharacterData(character);
            ImageMap = LoadCharacterImageMap(character);

            FrameRect = new Rect(
                0, 0,
                m_data.FrameSize[0], m_data.FrameSize[1]
            );

            m_cancellationTokenSource = new CancellationTokenSource();
            Task.Run(UpdateAsync);
        }

        internal void Close()
        {
            m_cancellationTokenSource.Cancel();
        }

        internal void SetSpeechPopup(Popup speechPopup)
        {
            m_speechPopup = speechPopup;
        }

        internal bool PlayAnimation(string animationName, AnimationMode mode)
        {
            if (animationName == null)
            {
                return false;
            }

            if (!m_data.Animations.ContainsKey(animationName))
            {
                return false;
            }

            if (mode == AnimationMode.Once)
            {
                m_playOnceAnimation = animationName;
                m_frameIndex = 0;
                m_data.Animations.TryGetValue(animationName, out m_activeAnimation);
                return true;
            }

            m_loopingAnimation = animationName;
            if (m_playOnceAnimation == null)
            {
                m_frameIndex = 0;
                m_data.Animations.TryGetValue(animationName, out m_activeAnimation);
            }
            return true;
        }

        internal void Say(string message, TimeSpan dismissAfter)
        {
            ActiveMessage = new ClippySpeechMessage(message, dismissAfter);
        }

        internal void Say(FrameworkElement content, TimeSpan? dismissAfter)
        {
            ActiveMessage = new ClippyCustomMessage(content, dismissAfter);
        }

        private async Task UpdateAsync()
        {
            var token = m_cancellationTokenSource.Token;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    if (m_activeMessage != null)
                    {
                        if (m_activeMessage.ShouldDismiss)
                        {
                            m_activeMessage = null;

                            await Application.Current.Dispatcher.InvokeAsync(() =>
                            {
                                m_speechPopup.IsOpen = false;
                            });
                        }
                    }

                    if (m_activeAnimation == null)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(20), token)
                            .ConfigureAwait(false);
                        continue;
                    }
                    
                    var frames = m_activeAnimation.Frames;
                    var frame = frames[m_frameIndex++];

                    if (frame.Images != null)
                    {
                        var images = frame.Images[0];
                        FrameCoords = new Point(-images[0], -images[1]);
                        CanvasVisibility = Visibility.Visible;
                    }
                    else
                    {
                        CanvasVisibility = Visibility.Hidden;
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(frame.Duration), token)
                        .ConfigureAwait(false);

                    if (m_frameIndex >= frames.Length)
                    {
                        OnAnimationCompleted?.Invoke(this, 
                            ActiveAnimation,
                            m_playOnceAnimation != null ? AnimationMode.Once : AnimationMode.Loop);

                        m_frameIndex = 0;
                        if (m_playOnceAnimation != null)
                        {
                            m_playOnceAnimation = null;
                            PlayAnimation(m_loopingAnimation, AnimationMode.Loop);
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                // Ignored
            }
        }

        private ImageSource LoadCharacterImageMap(Character character)
        {
            var uri = new Uri($"pack://application:,,,/Wpf.Clippy;Component/Resources/{character}/map.png", UriKind.Absolute);
            return BitmapFrame.Create(uri,
                BitmapCreateOptions.None,
                BitmapCacheOption.OnLoad);
        }

        private CharacterData LoadCharacterData(Character character)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream($"Wpf.Clippy.Resources.{character}.data.json"))
            {
                if (stream == null)
                {
                    throw new Exception($"Failed to find resource named 'Wpf.Clippy.Resources.{character}.data.json'");
                }

                var data = JsonSerializer.Deserialize<CharacterData>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data;
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}
