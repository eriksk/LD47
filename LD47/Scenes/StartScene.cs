using LD47.Game.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD47.Scenes
{
    public class StartScene : Scene
    {
        private Texture2D _background;
        private Texture2D _logo;
        private Vector2 _logoPosition;
        private float _current;
        private float _duration = 1f;
        private Vector2 _logoStart = new Vector2(256, -512);
        private Vector2 _logoTarget = new Vector2(512, 512) * 0.5f;
        private bool _done;

        public StartScene(ISceneManager sceneManager, ResourceContext resources) : base(sceneManager, resources)
        {
        }

        public override void Load()
        {
            _background = Resources.Content.Load<Texture2D>("Gfx/title_screen");
            _logo = Resources.Content.Load<Texture2D>("Gfx/logo");
            SoundManager.I.PlaySfx("time_travel");
        }

        private float Progress => MathHelper.Clamp(_current / _duration, 0f, 1f);

        public override void Update(GameTime gameTime)
        {
            _current += (float)gameTime.ElapsedGameTime.TotalSeconds;

            _logoPosition = Vector2.Lerp(
                _logoStart,
                _logoTarget,
                MathHelper.SmoothStep(0f, 1f, Progress)
            );

            if (Progress >= 1f && !_done)
            {
                _done = true;
                SoundManager.I.PlaySfx("attack");
            }

            if (Progress >= 1)
            {
                var keys = Keyboard.GetState();

                if (keys.IsKeyDown(Keys.Enter))
                {
                    SoundManager.I.PlaySfx("switch");
                    SceneManager.LoadScene(new GameScene(SceneManager, Resources));
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var sb = Resources.SpriteBatch;
            var fullScreen = new Rectangle(0, 0, 512, 512);

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            sb.Draw(_background, fullScreen, Color.White);

            sb.Draw(
                _logo,
                _logoPosition,
                null,
                Color.LightBlue,
                0f,
                new Vector2(_logo.Width, _logo.Height) * 0.5f,
                4f,
                SpriteEffects.None, 0f);

            if (Progress >= 1f)
            {
                var center = new Vector2(512, 512) * 0.5f;
                DrawStringCenteredWithShadow("PRESS ENTER", center + new Vector2(0f, 128), Color.White, 0f, 0.5f);
            }

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