using Portal.ComponentPattern;
using PortalGame;
using PortalGame.ComponentPattern;
using PortalGame.CreationalPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.CreationalPattern
{
    public class WandFactory : Factory
    {
        private static WandFactory instance; 
        public static WandFactory Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new WandFactory();
                }
                return instance; 
            }
        }

        private GameObject wandPrototype; 

        private WandFactory()
        {
            CreatePrototype();
        }

        private void CreatePrototype()
        {
            wandPrototype = new GameObject();
            SpriteRenderer spriteRenderer = wandPrototype.AddComponent(new SpriteRenderer()) as SpriteRenderer;
            spriteRenderer.SetSprite("Wand\\wand");
            spriteRenderer.LayerDepth = 0.8f;
            spriteRenderer.Scale = 1f;

            wandPrototype.AddComponent(new Wand()); 
        }

        public override GameObject Create(Enum type)
        {
            GameObject gameObject = (GameObject)wandPrototype.Clone();

            return gameObject; 
        }
    }
}
