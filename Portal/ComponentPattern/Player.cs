using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Portal.CommandPattern;
using Portal.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ButtonState = Portal.ObserverPattern.ButtonState;

namespace Portal.ComponentPattern
{
    public class Player : Component, IGameListener
    {
        #region fields
        private SpriteRenderer spriteRenderer;
        private float speed;
        private bool isFalling = true;

        private Dictionary<Keys, ButtonState> movementKeys = new Dictionary<Keys, ButtonState>();
        #endregion

        #region methods
        /// <summary>
        /// method used for running logic as the first thing on the component
        /// </summary>
        public override void Start()
        {
            speed = 250;

            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            spriteRenderer.SetSprite("Player\\player");
            spriteRenderer.LayerDepth = 0.5f;
            spriteRenderer.Scale = 1f;
            spriteRenderer.Color = new Color(255, 255, 255);

            // set initial position to middle of the map 
            GameObject.Transform.Position = new Vector2(GameWorld.Instance.Map.Width / 2, GameWorld.Instance.Map.Height / 2);
            GameObject.Tag = "Player";

            movementKeys.Add(Keys.A, ButtonState.UP);
            movementKeys.Add(Keys.D, ButtonState.UP);
            movementKeys.Add(Keys.W, ButtonState.UP); 
        }

        /// <summary>
        /// method for running logic each frame
        /// </summary>
        /// <param name="gameTime">used to access the frames per second through the gametime</param>
        public override void Update(GameTime gameTime)
        {
            //handles input
            InputHandler.Instance.Execute(this);

            if (isFalling)
            {
                // make player fall 
                Vector2 fall = new Vector2(0, 0.7f) * speed;
                GameObject.Transform.Translate(fall * GameWorld.DeltaTime);
            }

            // make player turn forward 
            //if (movementKeys[Keys.A] == ButtonState.UP && movementKeys[Keys.D] == ButtonState.UP)
            //{
            //    animator.PlayAnimation("Forward");
            //}
        }

        public void Jump()
        {
            if (!isFalling)
            {
                Debug.WriteLine("jump");
                Vector2 jump = new Vector2(0, -15) * speed;
                GameObject.Transform.Translate(jump * GameWorld.DeltaTime);
            }
        }

        public void Move(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }

            velocity *= speed;
            GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);
        }

        public void Notify(GameEvent gameEvent)
        {
            if(gameEvent is CollisionEvent)
            {
                GameObject other = (gameEvent as CollisionEvent).Other;

                if (other.Tag == "Tile")
                {
                    isFalling = true;
                }
                if (other.Tag == "CollisionTile")
                {
                    //Debug.WriteLine("test");
                    isFalling = false;
                }
                
            }
            else if (gameEvent is ButtonEvent)
            {
                ButtonEvent buttonEvent = (gameEvent as ButtonEvent);

                movementKeys[buttonEvent.Key] = buttonEvent.State;
            }

        }
        #endregion
    }
}
