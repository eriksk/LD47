using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD47.Scenes
{
    public class GameOverScene : Scene
    {
        private readonly GameResult _result;
        private Texture2D _gameOverTexture;

        public GameOverScene(GameResult result, ISceneManager sceneManager, ResourceContext resources)
            : base(sceneManager, resources)
        {
            _result = result;
        }

        public override void Load()
        {
            _gameOverTexture = Resources.Content.Load<Texture2D>("Gfx/game_over");
        }

        public override void Update(GameTime gameTime)
        {
            var keys = Keyboard.GetState();

            if(keys.IsKeyDown(Keys.Enter))
            {
                SceneManager.LoadScene(new GameScene(SceneManager, Resources));
            }
            else if(keys.IsKeyDown(Keys.Escape))
            {
                SceneManager.LoadScene(new ExitScene(SceneManager, Resources));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var text =
            "Alas the knight failed\n" +
            "in his quest and\n" +
            "only managed to stay\n" +
            $"alive for {_result.Iteration} time loops.";

            var sb = Resources.SpriteBatch;

            var center = new Vector2(512, 512) * 0.5f;

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            sb.Draw(_gameOverTexture, new Rectangle(0, 0, 512, 512), Color.White);
            DrawStringCenteredWithShadow("GAME OVER", center + new Vector2(0, -128f), Color.White, 0f, 1f);
            DrawStringCenteredWithShadow(text, center, Color.White, 0f, 0.5f);
            DrawStringCenteredWithShadow("Enter: Play Again", center + new Vector2(-80f, 128), Color.White, 0f, 0.4f);
            DrawStringCenteredWithShadow("Esc: Exit", center + new Vector2(80, 128), Color.White, 0f, 0.4f);
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

    public class GameResult
    {
        public int Iteration;
    }
}