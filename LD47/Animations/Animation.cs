
namespace LD47.Animations
{
    public class Animation
    {
        public readonly int[] Frames;
        public readonly float Interval;

        public int ActiveFrameIndex => Frames[_currentFrame];
        public float Duration => Interval * Frames.Length;

        private int _currentFrame;
        private float _current;

        public Animation(float interval, params int[] frames)
        {
            Frames = frames;
            Interval = interval;
        }

        public void Reset()
        {
            _current = 0;
            _currentFrame = 0;
        }

        public void Update(float dt)
        {
            _current += dt;

            if(_current >= Interval)
            {
                _currentFrame++;
                _current = 0f;
                if(_currentFrame > Frames.Length - 1)
                {
                    _currentFrame = 0;
                }
            }
        }
    }
}