using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD47.Game.Characters
{
    public class Character
    {
        private static int _idCounter;
        public readonly int Id = _idCounter++;
        public Vector2 Position;
        public Vector2 Velocity;
        public Color Color = Color.Black;

        public void Reset(Vector2 startPosition)
        {
            Position = startPosition;
            Velocity = Vector2.Zero;
        }

        public void Update(float dt, CharacterInput input)
        {
            const float acceleration = 800f;
            const float movementDamping = 0.1f;
            const float jumpForce = 128f;
            const float gravity = 256f;

            var movement = new Vector2();

            if (input.Left)
            {
                movement.X = -1f;
            }
            if (input.Right)
            {
                movement.X = 1f;
            }
            if (input.Left && input.Right)
            {
                movement.X = 0f;
            }

            if (input.Jump && Position.Y >= 512f)
            {
                Velocity.Y = -jumpForce;
            }

            Velocity.X += movement.X * acceleration * dt;
            Velocity.X += -Velocity.X * movementDamping;
            Velocity.Y += gravity * dt;

            Position += Velocity * dt;

            if(Position.Y >= 512f)
            {
                Position.Y = 512f;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            int width = 4;
            int height = 8;

            var halfWidth = width / 2;
            var halfHeight = height / 2;

            spriteBatch.Draw(
                pixel,
                new Rectangle(
                    (int)(Position.X - halfWidth),
                    (int)(Position.Y - height),
                    width,
                    height
                ),
                Color
            );
        }
    }
}