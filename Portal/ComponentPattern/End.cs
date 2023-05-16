using Microsoft.Xna.Framework;
using Portal.MenuStates;
using PortalGame;
using PortalGame.ComponentPattern;
using PortalGame.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern
{
    public class End : Component, IGameListener
    {
        private Vector2 position;
        private SpriteRenderer spriteRenderer;
        private Collider collider; 

        public End(Vector2 position)
        {
            this.position = position; 
        }

        public override void Awake()
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            spriteRenderer.SetSprite("Door\\DoorOpen");
            spriteRenderer.LayerDepth = 0.5f;
            spriteRenderer.Scale = 1f;

            GameObject.Transform.Position = position;
            GameObject.Tag = "End";

            // set collisionbox 
            collider = GameObject.GetComponent<Collider>() as Collider; 
            collider.CollisionBox = new Rectangle(
                                                  (int)(GameObject.Transform.Position.X - spriteRenderer.Sprite.Width / 2),
                                                  (int)(GameObject.Transform.Position.Y - spriteRenderer.Sprite.Height / 2),
                                                  (int)(spriteRenderer.Sprite.Width),
                                                  (int)(spriteRenderer.Sprite.Height)
                                                  );
        }

        public void Notify(GameEvent gameEvent)
        {
            if(gameEvent is CollisionEvent)
            {
                GameObject other = (gameEvent as CollisionEvent).Other;

                if(other.Tag == "Player")
                {
                    spriteRenderer.SetSprite("Door\\DoorClosed");

                    Player player = other.GetComponent<Player>() as Player;
                    player.RemoveWand(); 
                    GameState.Destroy(other);

                    GameState.EndReached = true;
                }
            }
        }
    }
}
