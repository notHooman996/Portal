using Microsoft.Xna.Framework;
using Portal.ComponentPattern;
using PortalGame;
using PortalGame.BuilderPattern;
using PortalGame.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.BuilderPattern
{
    public class EndBuilder : IBuilder
    {
        private GameObject gameObject;
        private Vector2 position;

        public EndBuilder(int x, int y, int tileSize)
        {
            position = new Vector2(x * tileSize, y * tileSize - 10);
        }

        public void BuildGameObject()
        {
            gameObject = new GameObject(); 

            BuildComponents();
        }

        private void BuildComponents()
        {
            gameObject.AddComponent(new SpriteRenderer());
            End end = gameObject.AddComponent(new End(position)) as End;
            Collider collider = gameObject.AddComponent(new Collider()) as Collider;
            collider.CollisionEvent.Attach(end); 
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
