using LD47.Game;
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
            _engine = new GameEngine();
            _renderer = new GameRenderer(Resources, _engine);

            // TODO: delay start and stuff, states etc
            _engine.Start();
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _engine.Update(dt);
        }

        public override void Draw(GameTime gameTime)
        {
            _renderer.Draw();
        }
    }
}