using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD47.Scenes
{
    public class ExitScene : Scene
    {
        private float _time;

        public ExitScene(ISceneManager sceneManager, ResourceContext resources) : base(sceneManager, resources)
        {
            _time = 2f;
        }

        public override void Load()
        {
        }

        public override void Update(GameTime gameTime)
        {
            _time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_time <= 0f)
            {
                Game1.RequestExit();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var sb = Resources.SpriteBatch;

            var center = new Vector2(512, 512) * 0.5f;

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            sb.Draw(Resources.Pixel, new Rectangle(0, 0, 512, 512), new Color(150, 177, 214, 255));
            DrawStringCenteredWithShadow("KTHXBYE", center, Color.White, 0f, 1f);
            DrawStringCenteredWithShadow("Thanks for playing", center + new Vector2(0f, 90), Color.White, 0f, 0.5f);
            sb.End();
        }

        private void DrawStringCenteredWithShadow(string text, Vector2 position, Color color, float rotation, float scale)
        {
            var origin = Resources.Font.MeasureString(text) * 0.5f;

            Resources.SpriteBatch.DrawString(Resources.Font,
                text,
                position + new Vector2(2, 2) * scale,
                Color.Black,
                rotation,
                origin,
                Vector2.One * scale,
                SpriteEffects.None, 0f);

            Resources.SpriteBatch.DrawString(Resources.Font,
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