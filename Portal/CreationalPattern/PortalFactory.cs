using Portal.ComponentPattern.Beams;
using Portal.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Portal.ComponentPattern.Portals;

namespace Portal.CreationalPattern
{
    public class PortalFactory : Factory
    {
        private static PortalFactory instance;

        public static PortalFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PortalFactory();
                }
                return instance;
            }
        }

        private GameObject redPrototype;
        private GameObject bluePrototype;

        private PortalFactory()
        {
            CreateRedPrototype();
            CreateBluePrototype();
        }

        private void CreateRedPrototype()
        {
            redPrototype = new GameObject();
            SpriteRenderer spriteRenderer = (SpriteRenderer)redPrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Portal\\redportal_back");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;
            spriteRenderer.Color = new Color(255, 255, 255);
            redPrototype.AddComponent(new RedPortal());
            redPrototype.AddComponent(new Collider());
        }

        private void CreateBluePrototype()
        {
            bluePrototype = new GameObject();
            SpriteRenderer spriteRenderer = (SpriteRenderer)bluePrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Portal\\blueportal_back");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;
            spriteRenderer.Color = new Color(255, 255, 255);
            bluePrototype.AddComponent(new BluePortal());
            bluePrototype.AddComponent(new Collider());
        }

        public override GameObject Create(Enum type)
        {
            GameObject gameObject = new GameObject();
            Collider collider; 

            switch (type)
            {
                case BeamType.Red:
                    // remove old portal 
                    // find the red portal object 
                    GameObject redPortalObject = GameWorld.Instance.GetObjectOfType<RedPortal>();
                    // destroy red portal 
                    if (redPortalObject != null)
                    {
                        GameWorld.Instance.Destroy(redPortalObject);
                    }

                    // create new portal 
                    gameObject = (GameObject)redPrototype.Clone();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    RedPortal redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal); 
                    break;
                case BeamType.Blue:
                    // remove old portal 
                    // find the blue portal object 
                    GameObject bluePortalObject = GameWorld.Instance.GetObjectOfType<BluePortal>();
                    // destroy blue portal 
                    if (bluePortalObject != null)
                    {
                        GameWorld.Instance.Destroy(bluePortalObject);
                    }

                    // create new portal 
                    gameObject = (GameObject)bluePrototype.Clone();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    BluePortal bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);
                    break;
            }

            return gameObject; 
        }
    }
}
