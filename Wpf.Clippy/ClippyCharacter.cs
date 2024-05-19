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

        public Character CharacterType { get; }
        public string ActiveAnimation => m_viewModel.ActiveAnimation;
        public IReadOnlyCollection<string> AnimationNames => m_viewModel.AnimationNames;

        public ClippyCharacter(Character character)
        {
            CharacterType = character;
            m_viewModel = new ClippyViewModel(character);
            m_control = new ClippyControl(m_viewModel);
        }

        public void Show()
        {
            m_control.Show();
        }

        public void Close()
        {
            m_control.Close();
        }

        public void PlayAnimation(string animationName)
        {
            m_viewModel.PlayAnimation(animationName, AnimationMode.Loop);
        }
    }
}
