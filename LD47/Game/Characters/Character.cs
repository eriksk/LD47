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
        public bool Alive;

        public bool Dead => !Alive;

        private const int Width = 4;
        private const int Height = 12;

        public Rectangle BoundingBox =>
             new Rectangle(
                    (int)(Position.X - (Width / 2)),
                    (int)(Position.Y - Height),
                    Width,
                    Height
                );

        public void Reset(Vector2 startPosition)
        {
            Position = startPosition;
            Velocity = Vector2.Zero;
            Alive = true;
        }

        public void Die()
        {
            Alive = false;
        }

        public void Update(float dt, CharacterInput input)
        {
            const float acceleration = 1600f;
            const float movementDamping = 0.2f;
            const float jumpForce = 256f;
            const float gravity = 800f;

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

            if (Position.Y >= 512f)
            {
                Position.Y = 512f;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            var halfWidth = Width / 2;

            // Full size
            spriteBatch.Draw(
                pixel,
                BoundingBox,
                new Color(31, 100, 136, 255)
            );

            // Shirt
            spriteBatch.Draw(
                pixel,
                new Rectangle(
                    (int)(Position.X - halfWidth),
                    (int)(Position.Y - Height),
                    Width,
                    8
                ),
                new Color(126, 177, 56, 255)
            );

            // Head
            spriteBatch.Draw(
                pixel,
                new Rectangle(
                    (int)(Position.X - halfWidth),
                    (int)(Position.Y - Height),
                    Width,
                    3
                ),
                new Color(210, 159, 113, 255)
            );
        }
    }
}