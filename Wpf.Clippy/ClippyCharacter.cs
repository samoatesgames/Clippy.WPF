using System;
using System.Collections.Generic;
using System.Windows;
using Wpf.Clippy.Types;
using Wpf.Clippy.ViewModels;
using Wpf.Clippy.Views;

namespace Wpf.Clippy
{
    public enum Character
    {
        Bonzi,
        Clippy,
        F1,
        Genie,
        Genius,
        Links,
        Merlin,
        Peedy,
        Rocky,
        Rover
    }

    public class ClippyCharacter
    {
        private readonly ClippyControl m_control;
        private readonly ClippyViewModel m_viewModel;

        public delegate void ClippyCharacterEventHandler(ClippyCharacter sender);
        public event ClippyCharacterEventHandler OnDoubleClick;

        public delegate void ClippyCharacterLocationEventHandler(ClippyCharacter sender, Point location);
        public event ClippyCharacterLocationEventHandler OnLocationChanged;

        public delegate void ClippyCharacterAnimationCompletedEventHandler(ClippyCharacter sender, string animationName, AnimationMode mode);
        public event ClippyCharacterAnimationCompletedEventHandler OnAnimationCompleted;

        private Point m_location;

        public Character CharacterType { get; }
        public string GetActiveAnimation(AnimationMode mode) => m_viewModel.GetActiveAnimation(mode);
        public IReadOnlyCollection<string> AnimationNames => m_viewModel.AnimationNames;

        public Point Location
        {
            get => m_location;
            set
            {
                if (m_location != value)
                {
                    m_location = value;
                    m_control.Dispatcher.InvokeAsync(() =>
                    {
                        var location = m_location;
                        m_control.Left = location.X;
                        m_control.Top = location.Y;
                    });
                }
            }
        }

        public ClippyCharacter(Character character)
        {
            CharacterType = character;
            m_viewModel = new ClippyViewModel(character);
            m_viewModel.OnAnimationCompleted += HandleAnimationCompleted;
            m_control = new ClippyControl(m_viewModel);
            m_control.OnDoubleClick += HandleDoubleClick;
            m_control.LocationChanged += HandleLocationChanged;
        }

        private void HandleAnimationCompleted(ClippyViewModel sender, string animationName, AnimationMode mode)
        {
            OnAnimationCompleted?.Invoke(this, animationName, mode);
        }

        private void HandleLocationChanged(object sender, EventArgs e)
        {
            m_location = new Point(m_control.Left, m_control.Top);
            OnLocationChanged?.Invoke(this, m_location);
        }

        private void HandleDoubleClick(ClippyControl control, ClippyViewModel viewModel)
        {
            OnDoubleClick?.Invoke(this);
        }

        public void Show()
        {
            m_control.Show();
            m_location = new Point(m_control.Left, m_control.Top);
            m_viewModel.Show();
        }

        public void Hide()
        {
            m_viewModel.Hide(() =>
            {
                Application.Current.Dispatcher?.InvokeAsync(() =>
                {
                    m_control.Hide();
                });
            });
        }

        public void Close()
        {
            m_control.SpeechPopup.IsOpen = false;
            m_control.Close();
        }

        public bool PlayAnimation(string animationName, AnimationMode mode)
        {
            return m_viewModel.PlayAnimation(animationName, mode);
        }

        public void Say(string message, TimeSpan dismissAfter)
        {
            m_viewModel.Say(message, dismissAfter);
        }

        public void Say(FrameworkElement content, TimeSpan? dismissAfter = null)
        {
            m_viewModel.Say(content, dismissAfter);
        }
    }
}
