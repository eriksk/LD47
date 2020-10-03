
using Microsoft.Xna.Framework;

namespace LD47.Scenes
{
    public interface ISceneManager
    {
        void LoadScene(IScene scene);
    }

    public interface IScene
    {
        void Load();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }

    public abstract class Scene : IScene
    {
        protected readonly ResourceContext Resources;
        protected readonly ISceneManager SceneManager;

        public Scene(ISceneManager sceneManager, ResourceContext resources)
        {
            SceneManager = sceneManager;
            Resources = resources;
        }

        public abstract void Load();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}