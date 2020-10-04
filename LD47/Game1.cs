using LD47.Game.Audio;
using LD47.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD47
{
    public class Game1 : Microsoft.Xna.Framework.Game, ISceneManager
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private IScene _scene;
        private ResourceContext _resources;
        private static bool _exitRequested;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public static void RequestExit() => _exitRequested = true;

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 512;
            _graphics.PreferredBackBufferHeight = 512;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _resources = new ResourceContext(GraphicsDevice, _spriteBatch, Content, Content.Load<SpriteFont>("Fonts/font"));

            SoundManager.I.Load(Content);

            // LoadScene(new GameScene(this, _resources));
            LoadScene(new StartScene(this, _resources));
            // LoadScene(new GameOverScene(new GameResult(){
            //     Iteration = 12
            // }, this, _resources));
        }

        public void LoadScene(IScene scene)
        {
            // TODO: transitions
            scene.Load();
            _scene = scene;
        }

        protected override void Update(GameTime gameTime)
        {
            if (_exitRequested)
            {
                Exit();
                return;
            }

            _scene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _scene.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
