namespace LD47.Game.Characters
{
    public struct CharacterInput
    {
        public bool Left;
        public bool Right;
        public bool Jump;
        public bool Shoot;

        public int ToInt()
        {
            var value = 0;

            value |= (Left ? 1 : 0);
            value |= (Right ? 2 : 0);
            value |= (Jump ? 4 : 0);
            value |= (Shoot ? 8 : 0);

            return value;
        }

        public static CharacterInput FromInt(int value)
        {
            return new CharacterInput()
            {
                Left = ((value & 1) == 1),
                Right = ((value & 2) == 2),
                Jump = ((value & 4) == 4),
                Shoot = ((value & 8) == 8),
            };
        }
    }
}