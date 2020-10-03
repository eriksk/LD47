using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LD47
{
    public class ResourceContext
    {
        public readonly GraphicsDevice GraphicsDevice;
        public readonly SpriteBatch SpriteBatch;
        public readonly ContentManager Content;
        public readonly Texture2D Pixel;

        public ResourceContext(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = spriteBatch;
            Content = content;

            Pixel = new Texture2D(graphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }
    }
}