using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Wpf.Clippy;
using Wpf.Clippy.Types;

namespace Clippy.Wpf.Demo.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private ClippyCharacter m_character;

        public ObservableCollection<string> Animations { get; }
            = new ObservableCollection<string>();

        public string SelectedAnimation
        {
            get => m_character.ActiveAnimation;
            set
            {
                if (value != m_character.ActiveAnimation)
                {
                    m_character.PlayAnimation(value, AnimationMode.Loop);
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Character> Characters { get; } 
            = new ObservableCollection<Character>();

        public Character SelectedCharacter
        {
            get => m_character.CharacterType;
            set
            {
                if (value != m_character?.CharacterType)
                {
                    RecreateCharacter(value);
                    OnPropertyChanged();
                }
            }
        }

        private void OnCharacterDoubleClicked(ClippyCharacter character)
        {
            if (!character.PlayAnimation("Wave", AnimationMode.Once))
            {
                character.PlayAnimation("Pleased", AnimationMode.Once);
            }

            character.Say("Hello! This is a longer message to see if the wrapping works as expected.", TimeSpan.FromSeconds(4));
        }

        private void RecreateCharacter(Character character)
        {
            if (m_character != null)
            {
                m_character.OnDoubleClick -= OnCharacterDoubleClicked;
                m_character.Close();
            }

            m_character = new ClippyCharacter(character);
            m_character.OnDoubleClick += OnCharacterDoubleClicked;
            m_character.Show();
            
            Animations.Clear();
            foreach (var animationName in m_character.AnimationNames.OrderBy(x => x))
            {
                Animations.Add(animationName);
            }
            SelectedAnimation = Animations.FirstOrDefault(x => x.ToLower().StartsWith("idle"))
                                ?? Animations.FirstOrDefault();
        }

        public MainWindowViewModel()
        {
            foreach (Character character in Enum.GetValues(typeof(Character)))
            {
                Characters.Add(character);
            }

            SelectedCharacter = Character.Clippy;
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
