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
                new Rectangle(0, 512-32, 512, 32)
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