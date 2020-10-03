using LD47.Game;
using LD47.Game.Stages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD47.Scenes
{
    public class GameScene : Scene
    {
        private GameEngine _engine;
        private GameRenderer _renderer;
        private float _transitionCurrent;
        private readonly float _transitionDuration = 0.3f;

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
                new Rectangle(-64, 512-40, 216+64, 20),
                new Rectangle(312, 512-40, 264, 20),
                // FLOOR TOP
                new Rectangle(-64, 108, 216+64, 20),
                new Rectangle(312, 108, 264, 20),
                // Left Lower Platform
                new Rectangle(16, 512 - 128, 120, 20),
                // Right Lower Platform
                new Rectangle(512 - (120 + 16), 512 - 128, 120, 20),
                // Middle left Platform
                new Rectangle(-64, 280, 140+76, 20),
                // Middle right Platform
                new Rectangle(364, 280, 140+76, 20),
                // Middle middle Platform
                new Rectangle(204, 344, 120, 20),
                // Middle top Platform
                new Rectangle(204, 196, 120, 20),
            }));
            _renderer = new GameRenderer(Resources, _engine);

            _engine.OnStateChanged += (state) =>
            {

            };
            _engine.OnIterationStarted += (iteration) =>
            {
                _transitionCurrent = 0f;
            };

            // TODO: delay start and stuff, states etc
            _engine.Start();
        }

        public override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_transitionCurrent <= _transitionDuration)
            {
                _transitionCurrent += dt;
                if (_transitionCurrent > _transitionDuration)
                {
                    _transitionCurrent = _transitionDuration;
                }
            }

            _engine.Update(dt);
            _renderer.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _renderer.Draw();

            var sb = Resources.SpriteBatch;

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);

            var color = Color.Black;
            color.A = (byte)(255 * (1f - (_transitionCurrent / _transitionDuration)));
            sb.Draw(
                Resources.Pixel,
                new Rectangle(0, 0, 512, 512),
                color);
            sb.End();
        }
    }
}