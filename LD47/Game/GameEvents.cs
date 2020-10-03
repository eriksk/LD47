
using System;
using LD47.Game.Characters;

namespace LD47.Game
{
    public class GameEvents
    {
        public event Action<GameState> OnStateChanged;
        public event Action<int> OnIterationStarted;
        public event Action<Character> OnCharacterJumped;
        public event Action<Character> OnCharacterLanded;
        public event Action<Character> OnCharacterDied;
        public event Action<Character> OnCharacterAttacked;

        public void InvokeStateChanged(GameState state) => OnStateChanged?.Invoke(state);
        public void InvokeIterationStarted(int iteration) => OnIterationStarted?.Invoke(iteration);
        public void InvokeCharacterJumped(Character character) => OnCharacterJumped?.Invoke(character);
        public void InvokeCharacterLanded(Character character) => OnCharacterLanded?.Invoke(character);
        public void InvokeCharacterDied(Character character) => OnCharacterDied?.Invoke(character);
        public void InvokeCharacterAttacked(Character character) => OnCharacterAttacked?.Invoke(character);
    }
}