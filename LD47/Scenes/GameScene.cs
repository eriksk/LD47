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
                new Rectangle(-64, 512-40, 512+128, 40),
                // Left Lower Platform
                new Rectangle(16, 512 - 128, 120, 20),
                // Right Lower Platform
                new Rectangle(512 - (120 + 16), 512 - 128, 120, 20),
                // Middle left Platform
                new Rectangle(-64, 280, 140+74, 20),
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