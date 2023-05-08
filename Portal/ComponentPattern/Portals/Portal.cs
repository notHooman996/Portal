using Microsoft.Xna.Framework;
using Portal.ComponentPattern;
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
        private Animator animator;
        public string AnimationName { get; set; }

        public Vector2 PlayerDisplacement { get; set; }

        public override void Start()
        {
            animator = GameObject.GetComponent<Animator>() as Animator;
            animator.PlayAnimation(AnimationName);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Notify(GameEvent gameEvent)
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
