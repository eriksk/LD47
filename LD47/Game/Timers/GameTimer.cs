using System;

namespace LD47.Game.Timers
{
    public class GameTimer
    {
        public const float TimeStep = 1f / 60f;
        private float _timeAccumulator;
        private int _elapsedFrames;

        public TimeSpan Elapsed => TimeSpan.FromSeconds(_elapsedFrames * TimeStep);
        public int TotalElapsedFrames => _elapsedFrames;

        public float DeltaTime => TimeStep;

        public void Reset()
        {
            _timeAccumulator = 0;
            _elapsedFrames = 0;
        }

        public FrameProgression Move(float dt)
        {
            _timeAccumulator += dt;

            var frames = 0;
            while (_timeAccumulator > TimeStep)
            {
                _timeAccumulator -= TimeStep;
                frames++;
                _elapsedFrames++;
            }

            return FrameProgression.Create(frames);
        }
    }
}