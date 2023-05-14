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
    public class BackgroundBuilder : IBuilder
    {
        private GameObject gameObject; 
        public void BuildGameObject()
        {
            gameObject = new GameObject();

            BuildComponents();
        }

        private void BuildComponents()
        {
            gameObject.AddComponent(new SpriteRenderer());
            gameObject.AddComponent(new Background()); 
        }

        public GameObject GetResult()
        {
            return gameObject; 
        }
    }
}
