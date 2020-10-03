using System.Collections.Generic;
using LD47.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD47.Game.Characters
{
    public class Character
    {
        private static int _idCounter;
        public readonly int Id = _idCounter++;

        public Vector2 Position;
        public readonly Vector2 InitialPosition;
        public Vector2 Velocity;
        public Color Color = Color.Black;
        public bool Alive;
        public bool Flipped;

        public bool Dead => !Alive;

        private const int Width = 4;
        private const int Height = 12;
        private float _attackCooldown;

        private Dictionary<string, Animation> _animations;
        private string _currentAnimation;

        public int CurrentAnimationSourceIndex => _animations[_currentAnimation].ActiveFrameIndex;

        public Rectangle BoundingBox =>
             new Rectangle(
                    (int)(Position.X - (Width / 2)),
                    (int)(Position.Y - Height),
                    Width,
                    Height
                );

        public Character(Vector2 initialPosition)
        {
            InitialPosition = initialPosition;
            _animations = new Dictionary<string, Animation>()
            {
                { "idle", new Animation(0.1f, 0, 1, 2, 3) },
                { "walk", new Animation(0.1f, 4, 5, 6, 7) },
                { "jump_up", new Animation(0.1f, 4) },
                { "jump_down", new Animation(0.1f, 6) },
                { "attack", new Animation(0.05f, 8, 9, 10, 11) },
            };
            _currentAnimation = "idle";
        }

        public void Reset()
        {
            Position = InitialPosition;
            Velocity = Vector2.Zero;
            Alive = true;
            Flipped = false;
            _attackCooldown = 0f;
            SetAnimation("idle");
        }

        private float GetAnimationDuration(string name)
        {
            var anim = _animations[name];
            return anim.Duration;
        }

        private void SetAnimation(string name)
        {
            if (_currentAnimation == name) return;
            _currentAnimation = name;
            _animations[_currentAnimation].Reset();
        }

        public void Die()
        {
            Alive = false;
        }

        public void Update(float dt, CharacterInput input)
        {
            const float acceleration = 2600f;
            const float movementDamping = 0.2f;
            const float jumpForce = 500f;
            const float gravity = 1200f;
            const float ground = 512f;

            var airborne = Position.Y < ground - 1;

            var movement = new Vector2();

            if(_attackCooldown > 0f)
            {
                _attackCooldown -= dt;
            }
            else if (!airborne && input.Shoot)
            {
                _attackCooldown = GetAnimationDuration("attack");
                SetAnimation("attack");
            }
            else
            {
                if (input.Left && !input.Right)
                {
                    movement.X = -1f;
                    Flipped = true;
                    SetAnimation("walk");
                }
                if (input.Right && !input.Left)
                {
                    movement.X = 1f;
                    Flipped = false;
                    SetAnimation("walk");
                }

                if (input.Left && input.Right)
                {
                    movement.X = 0f;
                }

                if (!input.Left && !input.Right)
                {
                    SetAnimation("idle");
                }

                if (airborne)
                {
                    if (Velocity.Y < 0f)
                    {
                        SetAnimation("jump_up");
                    }
                    else
                    {
                        SetAnimation("jump_down");
                    }
                }

                if (input.Jump && Position.Y >= ground)
                {
                    Velocity.Y = -jumpForce;
                }
            }

            Velocity.X += movement.X * acceleration * dt;
            Velocity.X += -Velocity.X * movementDamping;
            Velocity.Y += gravity * dt;

            Position += Velocity * dt;

            if (Position.Y >= ground)
            {
                Position.Y = ground;
            }

            const float borderMargin = 32f;

            if(Position.X < -borderMargin)
            {
                Position.X = 512f + borderMargin;
            }
            if(Position.X > 512f + borderMargin)
            {
                Position.X = -borderMargin;
            }

            _animations[_currentAnimation].Update(dt);
        }

        private Color Darken(Color color)
        {
            color.R = (byte)(color.R / 2);
            color.G = (byte)(color.G / 2);
            color.B = (byte)(color.B / 2);
            return color;
        }
    }
}