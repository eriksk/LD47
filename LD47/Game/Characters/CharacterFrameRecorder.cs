using System;
using System.Collections.Generic;

namespace LD47.Game.Characters
{
    public class CharacterFrameRecorder
    {
        private List<CharacterInputFrame> _frames;

        public CharacterFrameRecorder()
        {
            _frames = new List<CharacterInputFrame>();
        }

        public void Clear()
        {
            _frames.Clear();
        }

        public void PushFrame(int frame, CharacterInput inputState)
        {
            _frames.Add(new CharacterInputFrame()
            {
                Frame = frame,
                Input = inputState.ToInt()
            });
        }

        public int GetFrameInput(int frame)
        {
            if(frame > _frames.Count - 1) return 0; // No frames!

            var inputFrame = _frames[frame];
            if (inputFrame.Frame == frame) return inputFrame.Input;

            return 0; // Wrong frame? Throw exception
        }

        struct CharacterInputFrame
        {
            public int Frame;
            public int Input;
        }
    }
}