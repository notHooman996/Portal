using Microsoft.Xna.Framework;
using Portal.BuilderPattern;
using Portal.CreationalPattern;
using Portal.ObserverPattern;
using Portal.ObserverPattern.TileCollisionEvents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern.Beams
{
    public class Beam : Component, IGameListener
    {
        private int speed; 

        public Vector2 Direction { get; set; }

        public BeamType BeamType { get; set; }

        private bool createdPortal = false; 

        public override void Awake()
        {
            speed = 250; 

            base.Awake();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 d = Direction; 

            if (d != Vector2.Zero)
            {
                d.Normalize();
            }

            d *= speed; 

            GameObject.Transform.Translate(d * GameWorld.DeltaTime);

            //Debug.WriteLine($"x: {GameObject.Transform.Position.X}\ty: {GameObject.Transform.Position.Y}"); 

            base.Update(gameTime);
        }

        public void Notify(GameEvent gameEvent)
        {
            if(gameEvent is TopCollisionEvent)
            {
                Debug.WriteLine("top");

                GameObject other = (gameEvent as TopCollisionEvent).Other;

                if(other.Tag == "CollisionTile")
                {
                    GameObject portalObject = PortalFactory.Instance.Create(BeamType);
                    portalObject.Transform.Position = GameObject.Transform.Position;

                    portalObject.Transform.Rotation = 0; 

                    GameWorld.Instance.Instantiate(portalObject);

                    createdPortal = true; 
                }
            }
            if (gameEvent is BottomCollisionEvent)
            {
                Debug.WriteLine("bottom");

                GameObject other = (gameEvent as BottomCollisionEvent).Other;

                if (other.Tag == "CollisionTile")
                {
                    GameObject portalObject = PortalFactory.Instance.Create(BeamType);
                    portalObject.Transform.Position = GameObject.Transform.Position;

                    portalObject.Transform.Rotation = 0;

                    GameWorld.Instance.Instantiate(portalObject);

                    createdPortal = true;
                }
            }
            if (gameEvent is RightCollisionEvent)
            {
                Debug.WriteLine("right");

                GameObject other = (gameEvent as RightCollisionEvent).Other;

                if (other.Tag == "CollisionTile")
                {
                    GameObject portalObject = PortalFactory.Instance.Create(BeamType);
                    portalObject.Transform.Position = GameObject.Transform.Position;

                    portalObject.Transform.Rotation = -3.14f;

                    GameWorld.Instance.Instantiate(portalObject);

                    createdPortal = true;
                }
            }
            if (gameEvent is LeftCollisionEvent)
            {
                Debug.WriteLine("left");

                GameObject other = (gameEvent as LeftCollisionEvent).Other;

                if (other.Tag == "CollisionTile")
                {
                    GameObject portalObject = PortalFactory.Instance.Create(BeamType);
                    portalObject.Transform.Position = GameObject.Transform.Position;

                    portalObject.Transform.Rotation = 3.14f;

                    GameWorld.Instance.Instantiate(portalObject);

                    createdPortal = true;
                }
            }

            if (createdPortal)
            {
                if (gameEvent is CollisionEvent)
                {
                    GameObject other = (gameEvent as CollisionEvent).Other;

                    if (other.Tag == "CollisionTile")
                    {
                        GameWorld.Instance.Destroy(GameObject);
                    }
                }
            }
        }
    }
}
