using PortalGame.ComponentPattern.Portals;
using PortalGame.ComponentPattern;
using PortalGame;
using PortalGame.CreationalPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Portal.CreationalPattern
{
    public enum Side
    {
        Top,
        Bottom, 
        Left, 
        Right
    }

    public class RedPortalFactory : Factory
    {
        private static RedPortalFactory instance;

        public static RedPortalFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RedPortalFactory();
                }
                return instance;
            }
        }

        private GameObject redPrototype; 
        private GameObject topPrototype;
        private GameObject bottomPrototype;
        private GameObject leftPrototype;
        private GameObject rightPrototype;

        private RedPortalFactory()
        {
            CreateRedPrototype(); 
            CreateTopPrototype();
            CreateBottomPrototype();
            CreateLeftPrototype();
            CreateRightPrototype();
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

        private void CreateTopPrototype()
        {
            topPrototype = (GameObject)redPrototype.Clone(); 
        }

        private void CreateBottomPrototype()
        {
            bottomPrototype = (GameObject)redPrototype.Clone();
        }

        private void CreateLeftPrototype()
        {
            leftPrototype = (GameObject)redPrototype.Clone();
        }

        private void CreateRightPrototype()
        {
            rightPrototype = (GameObject)redPrototype.Clone();
        }

        public override GameObject Create(Enum type)
        {
            // remove old portal 
            // find the red portal object 
            GameObject redPortalObject = GameWorld.Instance.GetObjectOfType<RedPortal>();
            // destroy red portal 
            if (redPortalObject != null)
            {
                GameWorld.Instance.Destroy(redPortalObject);
            }

            GameObject gameObject = new GameObject();
            Collider collider;
            RedPortal redPortal; 

            switch (type)
            {
                case Side.Top:
                    gameObject = (GameObject)topPrototype.Clone();
                    gameObject.Tag = BeamType.Red.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal);

                    redPortal.PlayerDisplacement = new Vector2(0, -1);
                    break;
                case Side.Bottom:
                    gameObject = (GameObject)bottomPrototype.Clone();
                    gameObject.Tag = BeamType.Red.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal);

                    redPortal.PlayerDisplacement = new Vector2(0, 1);
                    break;
                case Side.Left:
                    gameObject = (GameObject)leftPrototype.Clone();
                    gameObject.Tag = BeamType.Red.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal);

                    redPortal.PlayerDisplacement = new Vector2(-1, 0);
                    break;
                case Side.Right:
                    gameObject = (GameObject)rightPrototype.Clone();
                    gameObject.Tag = BeamType.Red.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal);

                    redPortal.PlayerDisplacement = new Vector2(1, 0);
                    break;
            }

            return gameObject;
        }
    }
}
