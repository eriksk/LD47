using LD47.Game;
using LD47.Game.Stages;
using Microsoft.Xna.Framework;

namespace LD47.Scenes
{
    public class GameScene : Scene
    {
        private GameEngine _engine;
        private GameRenderer _renderer;

        public GameScene(ISceneManager sceneManager, ResourceContext resources)
            : base(sceneManager, resources)
        {
        }

        public override void Load()
        {
            _engine = new GameEngine(new Stage(new[]
            {
                // Always use 64px margin on both ways for floor
                // FLOOR
                new Rectangle(-64, 512-32, 512+128, 32),
                // Left Lower Platform
                // new Rectangle(0, 512 - 128, 128, 16),
                new Rectangle(512 - 128, 512 -128, 128, 128),
                new Rectangle(0, 512 - 128, 128, 128)
            }));
            _renderer = new GameRenderer(Resources, _engine);

            _engine.OnStateChanged += (state) =>
            {

            };

            // TODO: delay start and stuff, states etc
            _engine.Start();
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _engine.Update(dt);
            _renderer.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _renderer.Draw();
        }
    }
}