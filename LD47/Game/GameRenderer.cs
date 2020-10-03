using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD47.Game
{
    public class GameRenderer
    {
        private readonly ResourceContext _resources;
        private readonly GameEngine _engine;

        public GameRenderer(ResourceContext resources, GameEngine engine)
        {
            _resources = resources;
            _engine = engine;
        }

        public void Draw()
        {
            var sb = _resources.SpriteBatch;

            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            foreach (var character in _engine.Characters)
            {
                if (character.Dead) continue; // TODO: or draw them as dead or something
                character.Draw(sb, _resources.Pixel);
            }
            sb.End();

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