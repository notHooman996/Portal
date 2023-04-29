using PortalGame.ComponentPattern.Beams;
using PortalGame.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PortalGame.ComponentPattern.Portals;

namespace PortalGame.CreationalPattern
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
            // set all other portals to be old 
            //List<Portal> portals = GameWorld.Instance.FindAllObjectsOfType<Portal>();
            //foreach (Portal portal in portals)
            //{
            //    portal.IsNewest = false; 
            //}

            RedPortal oldRedPortal = (RedPortal)GameWorld.Instance.FindObjectOfType<RedPortal>();
            if(oldRedPortal != null)
            {
                oldRedPortal.IsNewest = false;
            }
            
            BluePortal oldBluePortal = (BluePortal)GameWorld.Instance.FindObjectOfType<BluePortal>();
            if(oldBluePortal != null)
            {
                oldBluePortal.IsNewest = false;
            }
            


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
                    gameObject.Tag = BeamType.Red.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    RedPortal redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal);

                    redPortal.IsNewest = true; 
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
                    gameObject.Tag = BeamType.Blue.ToString(); 
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    BluePortal bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);

                    bluePortal.IsNewest = true;
                    break;
            }

            return gameObject; 
        }
    }
}
