using PortalGame.ComponentPattern;
using PortalGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalGame.BuilderPattern;
using System.Numerics;
using Portal.ComponentPattern;

namespace Portal.BuilderPattern
{
    public class PlatformBuilder : IBuilder
    {
        private GameObject gameObject;
        private Vector2 position;
        private int Id;

        public PlatformBuilder(int x, int y, int tileSize, int id)
        {
            position = new Vector2(x * tileSize, y * tileSize);
            Id = id;
        }

        /// <summary>
        /// creates a new gameobject
        /// </summary>
        public void BuildGameObject()
        {
            gameObject = new GameObject();

            BuildComponents();
        }

        /// <summary>
        /// adds components to the gameobject
        /// </summary>
        private void BuildComponents()
        {
            // add components 
            gameObject.AddComponent(new SpriteRenderer());

            Platform platform = gameObject.AddComponent(new Platform(position, Id)) as Platform;
            Collider collider = gameObject.AddComponent(new Collider()) as Collider;

            gameObject.Tag = "Platform";
        }

        /// <summary>
        /// returns the object that was built
        /// </summary>
        /// <returns>gameobject which was just built out of components</returns>
        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
