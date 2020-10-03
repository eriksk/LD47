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

        public GameRenderer(ResourceContext resources, GameEngine engine)
        {
            _resources = resources;
            _engine = engine;
            _characterTexture = resources.Content.Load<Texture2D>("Gfx/character");
            _iconsTexture = resources.Content.Load<Texture2D>("Gfx/icons");
        }

        public void Draw()
        {
            var sb = _resources.SpriteBatch;

            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
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
            }

            var arrowSourceRectangle = new Rectangle(0, 0, 32, 32);
            sb.Draw(
                _iconsTexture,
                _engine.Player.Position + new Vector2(0f, -80f),
                arrowSourceRectangle,
                Color.White,
                0f,
                new Vector2(16, 16),
                Vector2.One * 4f,
                SpriteEffects.None, 0f);

            sb.End();

            DrawUI();
        }

        private void DrawUI()
        {
            var sb = _resources.SpriteBatch;
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

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
                Color.Red);

            var timePercentage = (int)(progress * 100f);
            var timeString = $"{timePercentage}%";

            DrawStringCentered(
                timeString,
                new Vector2(512f * 0.5f, 38f),
                Color.Gray,
                0f,
                0.5f);

            sb.End();
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
    }
}