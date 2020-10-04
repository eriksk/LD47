using LD47.Game.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD47.Scenes
{
    public class PreGameScene : Scene
    {
        private Texture2D _background;
        private float _current;
        private readonly float _inputTreshold = 2f;

        public PreGameScene(ISceneManager sceneManager, ResourceContext resources) : base(sceneManager, resources)
        {
        }

        public override void Load()
        {
            _background = Resources.Content.Load<Texture2D>("Gfx/title_screen");
        }

        public override void Update(GameTime gameTime)
        {
            _current += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_current > _inputTreshold)
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
            var center = new Vector2(512, 512) * 0.5f;

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            sb.Draw(_background, fullScreen, Color.White);

            DrawStringCenteredWithShadow(
                "THE KNIGHT IS STUCK IN A TIME LOOP.\n" +
                "HE MUST DEFEAT HIS PAST SELF IN\n" +
                "ORDER TO STAY ALIVE.", center + new Vector2(0f, -200), Color.Gray, 0f, 0.5f);
                
            DrawStringCenteredWithShadow(
                "Arrow Keys: MOVE/JUMP/ATTACK", center + new Vector2(0f, 0), Color.Orange, 0f, 0.5f);

            if (_current > _inputTreshold)
            {
                DrawStringCenteredWithShadow("PRESS ENTER TO START", center + new Vector2(0f, 200), Color.White, 0f, 0.5f);
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