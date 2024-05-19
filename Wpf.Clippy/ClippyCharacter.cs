using System;
using System.Collections.Generic;
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

        public Character CharacterType { get; }
        public string ActiveAnimation => m_viewModel.ActiveAnimation;
        public IReadOnlyCollection<string> AnimationNames => m_viewModel.AnimationNames;

        public ClippyCharacter(Character character)
        {
            CharacterType = character;
            m_viewModel = new ClippyViewModel(character);
            m_control = new ClippyControl(m_viewModel);
            m_control.OnDoubleClick += HandleDoubleClick;
        }

        private void HandleDoubleClick(ClippyControl control, ClippyViewModel viewModel)
        {
            OnDoubleClick?.Invoke(this);
        }

        public void Show()
        {
            m_control.Show();
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
    }
}
