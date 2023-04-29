using PortalGame.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame.BuilderPattern
{
    public class PlayerBuilder : IBuilder
    {
        private GameObject gameObject;

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
            Player player = (Player)gameObject.AddComponent(new Player());
            gameObject.AddComponent(new SpriteRenderer());
            Collider collider = (Collider)gameObject.AddComponent(new Collider());
            collider.CollisionEvent.Attach(player);
            collider.TopCollisionEvent.Attach(player); 
            collider.BottomCollisionEvent.Attach(player); 
            collider.RightCollisionEvent.Attach(player); 
            collider.LeftCollisionEvent.Attach(player); 
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
