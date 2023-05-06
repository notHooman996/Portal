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
            position = new Vector2(x * tileSize, y * tileSize);
        }

        public void BuildGameObject()
        {
            gameObject = new GameObject(); 

            BuildComponents();
        }

        private void BuildComponents()
        {
            gameObject.AddComponent(new End(position));
            gameObject.AddComponent(new SpriteRenderer());
            gameObject.AddComponent(new Collider()); 
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
