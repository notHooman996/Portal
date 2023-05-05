using PortalGame.ComponentPattern.Portals;
using PortalGame.ComponentPattern;
using PortalGame.CreationalPattern;
using PortalGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Portal.CreationalPattern
{
    public class BluePortalFactory : Factory
    {
        private static BluePortalFactory instance;

        public static BluePortalFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BluePortalFactory();
                }
                return instance;
            }
        }

        private GameObject bluePrototype;
        private GameObject topPrototype;
        private GameObject bottomPrototype;
        private GameObject leftPrototype;
        private GameObject rightPrototype;

        private BluePortalFactory()
        {
            CreateBluePrototype();
            CreateTopPrototype();
            CreateBottomPrototype();
            CreateLeftPrototype();
            CreateRightPrototype();
        }

        private void CreateBluePrototype()
        {
            bluePrototype = new GameObject();
            SpriteRenderer spriteRenderer = (SpriteRenderer)bluePrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Portal\\blueportal_back");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;
            bluePrototype.AddComponent(new BluePortal());
            bluePrototype.AddComponent(new Collider());
        }

        private void CreateTopPrototype()
        {
            topPrototype = (GameObject)bluePrototype.Clone();
        }

        private void CreateBottomPrototype()
        {
            bottomPrototype = (GameObject)bluePrototype.Clone();
        }

        private void CreateLeftPrototype()
        {
            leftPrototype = (GameObject)bluePrototype.Clone();
        }

        private void CreateRightPrototype()
        {
            rightPrototype = (GameObject)bluePrototype.Clone();
        }

        public override GameObject Create(Enum type)
        {
            // remove old portal 
            // find the blue portal object 
            GameObject bluePortalObject = GameWorld.Instance.GetObjectOfType<BluePortal>();
            // destroy blue portal 
            if (bluePortalObject != null)
            {
                GameWorld.Instance.Destroy(bluePortalObject);
            }

            GameObject gameObject = new GameObject();
            Collider collider;
            BluePortal bluePortal;

            switch (type)
            {
                case Side.Top:
                    gameObject = (GameObject)topPrototype.Clone();
                    gameObject.Tag = BeamType.Blue.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);

                    bluePortal.PlayerDisplacement = new Vector2(0, -1); 
                    break;
                case Side.Bottom:
                    gameObject = (GameObject)bottomPrototype.Clone();
                    gameObject.Tag = BeamType.Blue.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);

                    bluePortal.PlayerDisplacement = new Vector2(0, 1);
                    break;
                case Side.Left:
                    gameObject = (GameObject)leftPrototype.Clone();
                    gameObject.Tag = BeamType.Blue.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);

                    bluePortal.PlayerDisplacement = new Vector2(-1, 0);
                    break;
                case Side.Right:
                    gameObject = (GameObject)rightPrototype.Clone();
                    gameObject.Tag = BeamType.Blue.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);

                    bluePortal.PlayerDisplacement = new Vector2(1, 0);
                    break;
            }

            return gameObject;
        }
    }
}
