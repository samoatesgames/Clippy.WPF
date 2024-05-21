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

        private Point m_location;

        public Character CharacterType { get; }
        public string ActiveAnimation => m_viewModel.ActiveAnimation;
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
            m_control = new ClippyControl(m_viewModel);
            m_control.OnDoubleClick += HandleDoubleClick;
            m_control.LocationChanged += HandleLocationChanged;
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
        }

        public void Close()
        {
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
