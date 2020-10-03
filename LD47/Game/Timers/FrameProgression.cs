namespace LD47.Game.Timers
{
    public struct FrameProgression
    {
        public int Count;

        public static FrameProgression Create(int frames) => new FrameProgression() { Count = frames };
    }
}