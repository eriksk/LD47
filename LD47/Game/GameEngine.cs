using System;
using System.Collections.Generic;
using System.Linq;
using LD47.Game.Audio;
using LD47.Game.Characters;
using LD47.Game.Effects;
using LD47.Game.Stages;
using LD47.Game.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LD47.Game
{
    public class GameEngine
    {
        private readonly List<Character> _characters;
        private readonly GameTimer _timer;
        private readonly Stage _stage;
        private CharacterInput _inputState;
        private Character _playerCharacter;
        private Dictionary<int, CharacterFrameRecorder> _recorders;
        public readonly int IterationTimeInframes = 60 * 10; // 5 sec

        public List<Character> Characters => _characters;
        public GameTimer Timer => _timer;
        public Character Player => _playerCharacter;
        public Stage Stage => _stage;

        public Particles Particles { get; private set; }

        public readonly GameEvents Events;

        private Random _random;

        public int Iteration = 0;
        private GameState _state;
        public GameState State => _state;
        private float _startWaitTime = 0f;
        public float StartTimeCountDown => _startWaitTime;
        private float _nextIterationDelay;

        public GameEngine(Stage stage)
        {
            _stage = stage;
            Events = new GameEvents();
            _recorders = new Dictionary<int, CharacterFrameRecorder>();
            _characters = new List<Character>();
            _timer = new GameTimer();
            _random = new Random();
            Particles = new Particles();

            SetupEvents();
        }

        private void SetupEvents()
        {
            Events.OnCharacterLanded += (character) =>
            {
                Particles.SpawnLand(character.Position);
                SoundManager.I.PlaySfx("land");
            };
            Events.OnCharacterJumped += (character) =>
            {
                Particles.SpawnJump(character.Position);
                SoundManager.I.PlaySfx("jump");
            };
            Events.OnCharacterDied += (character) =>
            {
                Particles.SpawnDeath(character.BoundingBox.Center.ToVector2());
                SoundManager.I.PlaySfx("die");
            };
            Events.OnCharacterAttacked += (character) =>
            {
                SoundManager.I.PlaySfx("attack", 0.5f);
            };
            Events.OnIterationStarted += (iteration) =>
            {
                SoundManager.I.PlaySfx("time_travel");
            };
            Events.OnStateChanged += (state) =>
            {
                if (state == GameState.GameOver)
                {
                    SoundManager.I.PlaySfx("game_over");
                }
            };
        }

        public void Start()
        {
            _state = GameState.StartingIteration;
            NextIteration();
        }

        private void SetState(GameState state)
        {
            if (state == GameState.Playing && _state == GameState.StartingIteration)
            {
                // Clear when starting game
                GetOrCreateRecorder(_playerCharacter).Clear();
                _playerCharacter.SetCurrentAsStartingPosition();
            }

            _state = state;

            if (_state == GameState.StartingIteration)
            {
                _startWaitTime = 3f;
            }
            if (_state == GameState.WaitForNextIteration)
            {
                _nextIterationDelay = 2f;
            }

            Events.InvokeStateChanged(_state);
        }

        private bool IsInWinState => _playerCharacter.Alive && _characters.Except(new[] { _playerCharacter }).All(x => x.Dead);

        private void NextIteration()
        {
            if (!_characters.Except(new[] { _playerCharacter }).All(x => x.Dead))
            {
                SetState(GameState.GameOver);
                return;
            }

            Iteration++;
            _timer.Reset();
            Particles.Clear();

            _playerCharacter = new Character(_stage.GetRandomSpawnPoint(_random));
            _characters.Add(_playerCharacter);

            foreach (var character in _characters)
            {
                character.Reset();
            }

            SetState(GameState.StartingIteration);
            Events.InvokeIterationStarted(Iteration);
        }

        public void Update(float dt)
        {
            if (_state == GameState.Playing)
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
            else if (_state == GameState.StartingIteration)
            {
                _startWaitTime -= dt;
                if (_startWaitTime <= 0f)
                {
                    _startWaitTime = 0f;
                    SetState(GameState.Playing);
                }
                else
                {
                    // Allow player to reposition to a good starting point
                    ProcessInput();
                    _playerCharacter.Update(dt, _inputState, this);
                    Particles.Update(dt);
                }
            }
            else if (_state == GameState.WaitForNextIteration)
            {
                _nextIterationDelay -= dt;
                Particles.Update(dt);
                if(_nextIterationDelay <= 0f)
                {
                    NextIteration();
                }
            }
            else if (_state == GameState.GameOver)
            {
                Particles.Update(dt);
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
                Shoot = keys.IsKeyDown(Keys.Down)
            };
        }

        private void Step(int frame)
        {
            Particles.Update(_timer.DeltaTime);

            if (_playerCharacter.Dead)
            {
                // TODO: Game over
                SetState(GameState.GameOver);
                return;
            }

            if (IsInWinState && Iteration > 1) // Let first iteration play out
            {
                //Early exit
                SoundManager.I.PlaySfx("win");
                SetState(GameState.WaitForNextIteration);
                return;
            }

            UpdateClockTickSound(frame);

            _playerCharacter.Update(_timer.DeltaTime, _inputState, this);
            RecordFrame(_playerCharacter, _inputState, frame);

            foreach (var character in _characters)
            {
                if (character == _playerCharacter) continue;
                if (character.Dead) continue;

                var recorder = GetOrCreateRecorder(character);
                var input = recorder.GetFrameInput(frame);
                character.Update(_timer.DeltaTime, CharacterInput.FromInt(input), this);
            }

            // Attacks
            for (var i = 0; i < _characters.Count; i++)
            {
                var character1 = _characters[i];
                if (character1.Dead) continue;

                if (!character1.Attacking && !character1.JumpAttacking)
                {
                    continue;
                }

                for (var j = 0; j < _characters.Count; j++)
                {
                    if (i == j) continue;

                    var character2 = _characters[j];
                    if (character2.Dead) continue;

                    if (character1.Attacking && character1.Hitbox.Intersects(character2.BoundingBox))
                    {
                        character2.Die();
                        Events.InvokeCharacterDied(character2);
                        continue;
                    }
                    if (character1.JumpAttacking && character1.JumpHitbox.Intersects(character2.BoundingBox))
                    {
                        character2.Die();
                        character1.Bounce();
                        Events.InvokeCharacterDied(character2);
                        continue;
                    }
                }
            }
        }

        private void UpdateClockTickSound(int frame)
        {
            var frameSegment = IterationTimeInframes / 4;
            var mod = 60;
            if (frame > frameSegment && frame <= frameSegment * 2)
            {
                mod = 30;
            }
            if (frame > frameSegment * 2 && frame <= frameSegment * 3)
            {
                mod = 30;
            }
            if (frame > frameSegment * 3)
            {
                mod = 15;
            }

            if (frame % mod == 0)
            {
                SoundManager.I.PlaySfx("tick");
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