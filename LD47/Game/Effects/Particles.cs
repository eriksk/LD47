using System;
using LD47.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD47.Game.Effects
{
    public class Particles
    {
        private Pool<Particle> _particles;
        private Random _random;

        public Particles()
        {
            _particles = new Pool<Particle>(1024);
            _random = new Random();
        }

        public void Clear()
        {
            _particles.Clear();
        }

        public void SpawnDeath(Vector2 position)
        {
            for (var i = 0; i < 64; i++)
            {

                var p = _particles.Pop();
                if (p == null) return;

                p.Reset();

                p.Position = position;
                p.Color = i % 2 == 0 ? Color.Red : Color.DarkRed;
                
                var offset = new Vector2(
                    _random.Next(-16, 16),
                    _random.Next(-16, 16)
                );
                p.Position = position + offset;
                p.Velocity = new Vector2(_random.Next(-32, 32), _random.Next(-200, -100));
                p.Gravity = new Vector2(0f, 400f);
                p.Duration = _random.NextFloat(0.5f, 1.5f);
                p.StartScale = 1f;
                p.Scale = p.StartScale;
                p.EndScale = 0f;
            }
        }

        public void SpawnJump(Vector2 position)
        {
            for (var i = 0; i < 12; i++)
            {
                var p = _particles.Pop();
                if (p == null) return;

                p.Reset();

                var offset = new Vector2(
                    _random.Next(-8, 8),
                    _random.Next(-8, 8)
                );
                p.Position = position + offset;
                p.Velocity = new Vector2(_random.Next(-16, 16), _random.Next(-200, -100));
                p.Gravity = new Vector2(0f, 600f);
                p.Duration = _random.NextFloat(0.1f, 0.5f);
                p.StartScale = 1f;
                p.Scale = p.StartScale;
                p.EndScale = 0f;
            }
        }

        public void SpawnLand(Vector2 position)
        {
            for (var i = 0; i < 12; i++)
            {
                var p = _particles.Pop();
                if (p == null) return;

                p.Reset();

                var offset = new Vector2(
                    _random.Next(-8, 8),
                    _random.Next(-8, 8)
                );
                var offsetXSign = MathF.Sign(offset.X);

                p.Position = position + offset;
                p.Velocity = new Vector2(_random.Next(50, 100) * offsetXSign, _random.Next(-200, -100));
                p.Gravity = new Vector2(0f, 600f);
                p.Duration = _random.NextFloat(0.1f, 0.5f);
                p.StartScale = 1f;
                p.Scale = p.StartScale;
                p.EndScale = 0f;
            }
        }

        public void Update(float dt)
        {
            for (var i = 0; i < _particles.Count; i++)
            {
                var p = _particles[i];

                p.Current += dt;
                if (p.Current >= p.Duration)
                {
                    _particles.Push(i--);
                    continue;
                }

                var progress = MathHelper.Clamp(p.Current / p.Duration, 0f, 1f);

                p.Scale = MathHelper.Lerp(p.StartScale, p.EndScale, progress);

                p.Velocity += p.Gravity * dt;
                p.Position += p.Velocity * dt;
            }
        }

        public void Draw(ResourceContext context)
        {
            var sb = context.SpriteBatch;
            var pixel = context.Pixel;

            var origin = new Vector2(0.5f, 0.5f);
            var scale = 4f;

            for (var i = 0; i < _particles.Count; i++)
            {
                var p = _particles[i];

                sb.Draw(
                    pixel,
                    p.Position,
                    null,
                    p.Color,
                    0f,
                    origin,
                    scale * p.Scale,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        class Particle
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public Vector2 Gravity;
            public Color Color;
            public float Current;
            public float Duration;

            public float StartScale;
            public float EndScale;
            public float Scale;

            public void Reset()
            {
                Position = Vector2.Zero;
                Velocity = Vector2.Zero;
                Gravity = Vector2.Zero;
                Color = Color.White;
                Current = 0f;
                Duration = 1f;
                StartScale = 1f;
                EndScale = 1f;
                Scale = 1f;
            }
        }
    }
}