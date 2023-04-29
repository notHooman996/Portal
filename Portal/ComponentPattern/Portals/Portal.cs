using Microsoft.Xna.Framework;
using PortalGame.BuilderPattern;
using PortalGame.CreationalPattern;
using PortalGame.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame.ComponentPattern.Portals
{
    public class Portal : Component, IGameListener
    {
        public bool IsNewest { get; set; }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Notify(GameEvent gameEvent)
        {
            if (IsNewest)
            {
                if (gameEvent is CollisionEvent)
                {
                    GameObject other = (gameEvent as CollisionEvent).Other;

                    if (GameObject.Tag == BeamType.Red.ToString() && other.Tag == BeamType.Blue.ToString())
                    {
                        // remove blue portal 
                        GameWorld.Instance.Destroy(other);
                    }
                    if (GameObject.Tag == BeamType.Blue.ToString() && other.Tag == BeamType.Red.ToString())
                    {
                        // remove red portal 
                        GameWorld.Instance.Destroy(other);
                    }
                }
            }
        }
    }
}
