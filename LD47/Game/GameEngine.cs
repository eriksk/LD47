using System;
using System.Collections.Generic;
using LD47.Game.Characters;
using LD47.Game.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LD47.Game
{
    public class GameEngine
    {
        private readonly List<Character> _characters;
        private readonly GameTimer _timer;
        private CharacterInput _inputState;
        private Character _playerCharacter;
        private Dictionary<int, CharacterFrameRecorder> _recorders;
        public readonly int IterationTimeInframes = 60 * 10; // 5 sec

        public List<Character> Characters => _characters;
        public GameTimer Timer => _timer;
        public Character Player => _playerCharacter;

        private Random _random;

        public GameEngine()
        {
            _recorders = new Dictionary<int, CharacterFrameRecorder>();
            _characters = new List<Character>();
            _timer = new GameTimer();
            _random = new Random();
        }

        public void Start()
        {
            NextIteration();
        }

        private void NextIteration()
        {
            _timer.Reset();

            _playerCharacter = new Character(new Vector2(_random.Next(16, 512 - 16), 512));
            _characters.Add(_playerCharacter);

            foreach (var character in _characters)
            {
                character.Reset();
            }
        }

        public void Update(float dt)
        {
            var progression = _timer.Move(dt);
            ProcessInput();
            for (var i = 0; i < progression.Count; i++)
            {
                var currentFrame = (_timer.TotalElapsedFrames - progression.Count) + i;
                if (_timer.TotalElapsedFrames > IterationTimeInframes)
                {
                    NextIteration();
                    return;
                }
                Step(currentFrame);
            }
        }

        private void ProcessInput()
        {
            var keys = Keyboard.GetState();

            _inputState = new CharacterInput()
            {
                Left = keys.IsKeyDown(Keys.Left),
                Right = keys.IsKeyDown(Keys.Right),
                Jump = keys.IsKeyDown(Keys.Up),
                Shoot = keys.IsKeyDown(Keys.LeftControl)
            };
        }

        private void Step(int frame)
        {
            if (_playerCharacter.Dead)
            {
                // TODO: Game over

            }
            _playerCharacter.Update(_timer.DeltaTime, _inputState);
            RecordFrame(_playerCharacter, _inputState, frame);

            foreach (var character in _characters)
            {
                if (character == _playerCharacter) continue;
                if (character.Dead) continue;

                var recorder = GetOrCreateRecorder(character);
                var input = recorder.GetFrameInput(frame);
                character.Update(_timer.DeltaTime, CharacterInput.FromInt(input));
            }
        }

        private void RecordFrame(Character character, CharacterInput inputState, int frame)
        {
            GetOrCreateRecorder(character)
                .PushFrame(frame, inputState);
        }

        private CharacterFrameRecorder GetOrCreateRecorder(Character character)
        {
            if (!_recorders.TryGetValue(character.Id, out var recorder))
            {
                recorder = new CharacterFrameRecorder();
                _recorders.Add(character.Id, recorder);
            }

            return recorder;
        }
    }
}