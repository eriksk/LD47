using Microsoft.Xna.Framework.Graphics;

namespace LD47.Game
{
    public class GameRenderer
    {
        private readonly ResourceContext _resources;
        private readonly GameEngine _engine;

        public GameRenderer(ResourceContext resources, GameEngine engine)
        {
            _resources = resources;
            _engine = engine;
        }

        public void Draw()
        {
            var sb = _resources.SpriteBatch;

            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            foreach (var character in _engine.Characters)
            {
                character.Draw(sb, _resources.Pixel);
            }
            sb.End();
        }
    }
}