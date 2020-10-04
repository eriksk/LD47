using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD47.Game
{
    public class GameRenderer
    {
        private readonly ResourceContext _resources;
        private readonly GameEngine _engine;
        private readonly Texture2D _characterTexture;
        private readonly Texture2D _iconsTexture;
        private readonly Texture2D _stageTexture;
        public bool DrawDebug = false;
        private float _totalTime;

        private readonly Vector2 _center = new Vector2(512, 512) * 0.5f;
        private readonly Rectangle _fullScreen = new Rectangle(0, 0, 512, 512);

        public GameRenderer(ResourceContext resources, GameEngine engine)
        {
            _resources = resources;
            _engine = engine;
            _characterTexture = resources.Content.Load<Texture2D>("Gfx/character");
            _iconsTexture = resources.Content.Load<Texture2D>("Gfx/icons");
            _stageTexture = resources.Content.Load<Texture2D>("Gfx/stage");
        }

        public void Update(GameTime time)
        {
            _totalTime += (float)time.ElapsedGameTime.TotalSeconds;
        }

        public void Draw()
        {
            var sb = _resources.SpriteBatch;

            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

            sb.Draw(_stageTexture,
                new Rectangle(0, 0, 512, 512),
                Color.White);

            if (DrawDebug)
            {
                foreach (var platform in _engine.Stage.Platforms)
                {
                    DrawRectOutline(platform, 1, Color.Orange);
                }
            }

            foreach (var character in _engine.Characters)
            {
                if (character.Dead) continue; // TODO: or draw them as dead or something

                var isPlayer = character == _engine.Player;
                var index = character.CurrentAnimationSourceIndex;

                var col = index % 4;
                var row = index / 4;
                const int cellSize = 16;

                var sourceRectangle = new Rectangle(col * cellSize, row * cellSize, cellSize, cellSize);

                sb.Draw(
                    _characterTexture,
                    character.Position,
                    sourceRectangle,
                    isPlayer ? Color.White : Color.Gray,
                    0f,
                    new Vector2(cellSize * 0.5f, cellSize),
                    Vector2.One * 4f,
                    character.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                if (DrawDebug)
                {
                    DrawRectOutline(character.BoundingBox, 1f, Color.Green);
                    if (character.Attacking)
                    {
                        DrawRectOutline(character.Hitbox, 1f, Color.Red);
                    }
                    if (character.JumpAttacking)
                    {
                        DrawRectOutline(character.JumpHitbox, 1f, Color.Red);
                    }

                    DrawRectOutline(character.GroundCheckbox, 1f, character.Grounded ? Color.Red : Color.Blue);
                }
            }

            _engine.Particles.Draw(_resources);

            var arrowSourceRectangle = new Rectangle(0, 0, 32, 32);
            sb.Draw(
                _iconsTexture,
                _engine.Player.Position + new Vector2(0f, -80f + (MathF.Sin(_totalTime * 10f) * 4f)),
                arrowSourceRectangle,
                Color.White,
                0f,
                new Vector2(16, 16),
                Vector2.One * 4f,
                SpriteEffects.None, 0f);

            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            if (_engine.State == GameState.Playing)
            {
                DrawGameStateUI(sb);
            }
            else if (_engine.State == GameState.StartingIteration)
            {
                DrawIterationStartingUI(sb);
            }
            sb.End();
        }

        private void DrawIterationStartingUI(SpriteBatch sb)
        {
            sb.Draw(_resources.Pixel, _fullScreen, new Color(0, 0, 0, 100));
            var textPosition = new Vector2(_fullScreen.Center.X, _fullScreen.Top + 50);
            DrawStringCenteredWithShadow("TIME LOOP STARTING", textPosition, Color.White, 0f, 1f);
            DrawStringCenteredWithShadow("Find a good starting position!", textPosition + new Vector2(0f, 32), Color.White, 0f, 0.5f);
            DrawStringCenteredWithShadow((((int)_engine.StartTimeCountDown) + 1).ToString(), _center, Color.White, 0f, 2f);
        }

        private void DrawGameStateUI(SpriteBatch sb)
        {
            var progress = _engine.Timer.TotalElapsedFrames / (float)_engine.IterationTimeInframes;

            var progressBarRectangle = new Rectangle(
                0, 0,
                512,
                4
            );

            sb.Draw(
                _resources.Pixel,
                progressBarRectangle,
                new Color(0, 0, 0, 50));

            progressBarRectangle.Width = (int)(512f * progress);

            sb.Draw(
                _resources.Pixel,
                progressBarRectangle,
                Color.Lerp(Color.Blue, Color.Red, progress));

            DrawStringCenteredWithShadow(
                "iteration",
                new Vector2(512f * 0.5f, 64f),
                Color.White,
                0f,
                0.5f);

            DrawStringCenteredWithShadow(
                _engine.Iteration.ToString(),
                new Vector2(512f * 0.5f, 128f),
                Color.White,
                0f,
                1f);

            if (progress > 0.75f)
            {
                var color = Color.Lerp(Color.Red, Color.Orange, 0.5f + MathF.Sin(_totalTime * 10f) * 0.5f);
                color.A = 255;
                color *= 0.3f;
                sb.Draw(
                    _resources.Pixel,
                    _fullScreen,
                    color);
            }
        }

        private void DrawStringCenteredWithShadow(string text, Vector2 position, Color color, float rotation, float scale)
        {
            var origin = _resources.Font.MeasureString(text) * 0.5f;

            _resources.SpriteBatch.DrawString(_resources.Font,
                text,
                position + new Vector2(2, 2) * scale,
                Color.Black,
                rotation,
                origin,
                Vector2.One * scale,
                SpriteEffects.None, 0f);

            _resources.SpriteBatch.DrawString(_resources.Font,
                text,
                position,
                color,
                rotation,
                origin,
                Vector2.One * scale,
                SpriteEffects.None, 0f);
        }

        private void DrawStringCentered(string text, Vector2 position, Color color, float rotation, float scale)
        {
            var origin = _resources.Font.MeasureString(text) * 0.5f;

            _resources.SpriteBatch.DrawString(_resources.Font,
                text,
                position,
                color,
                rotation,
                origin,
                Vector2.One * scale,
                SpriteEffects.None, 0f);
        }


        private void DrawLine(Vector2 a, Vector2 b, float thickness, Color color)
        {
            var angle = MathF.Atan2(b.Y - a.Y, b.X - a.X);
            var distance = Vector2.Distance(a, b);
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(distance, thickness);

            _resources.SpriteBatch.Draw(
                _resources.Pixel,
                a,
                null,
                color,
                angle,
                origin,
                scale,
                SpriteEffects.None, 0f);
        }

        private void DrawRectOutline(
            Rectangle rect,
            float thickness,
            Color color)
        {
            // TODO: edges have gaps
            DrawLine(new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top), thickness, color);
            DrawLine(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom), thickness, color);
            DrawLine(new Vector2(rect.Left, rect.Top), new Vector2(rect.Left, rect.Bottom), thickness, color);
            DrawLine(new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom), thickness, color);
        }

        private void DrawCircleOutline(
            Vector2 position,
            float radius,
            float thickness,
            int segments,
            Color color)
        {
            // TODO: edges have gaps
            for (var i = 0; i <= segments; i++)
            {
                var a = (i / (float)(segments + 1)) * MathF.PI * 2f;
                var b = ((i + 1) / (float)(segments + 1)) * MathF.PI * 2f;

                var aPos = new Vector2(
                    MathF.Cos(a),
                    MathF.Sin(a)
                ) * radius;

                var bPos = new Vector2(
                    MathF.Cos(b),
                    MathF.Sin(b)
                ) * radius;

                DrawLine(position + aPos, position + bPos, thickness, color);
            }
        }
    }
}