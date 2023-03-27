using Microsoft.Xna.Framework;
using Portal.CommandPattern;
using Portal.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern
{
    public class Player : Component, IGameListener
    {
        #region fields
        private SpriteRenderer spriteRenderer;
        private float speed;

        private Vector2 goalPosition;
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
            // set initial position to middle of the map 
            GameObject.Transform.Position = new Vector2(GameWorld.Instance.Map.Width / 2, GameWorld.Instance.Map.Height / 2);
            goalPosition = GameObject.Transform.Position;
        }

        /// <summary>
        /// method for running logic each frame
        /// </summary>
        /// <param name="gameTime">used to access the frames per second through the gametime</param>
        public override void Update(GameTime gameTime)
        {
            //handles input
            InputHandler.Instance.Execute(this);

            // keep moving until goal is reached 
            int offset = 5;

            if (GameObject.Transform.Position.X <= goalPosition.X - offset || GameObject.Transform.Position.Y <= goalPosition.Y - offset ||
               GameObject.Transform.Position.X >= goalPosition.X + offset || GameObject.Transform.Position.Y >= goalPosition.Y + offset)
            {
                Move(goalPosition);
            }
        }

        public void Move(Vector2 endPoint)
        {
            goalPosition = endPoint;

            Vector2 velocity = new Vector2(endPoint.X - GameObject.Transform.Position.X, endPoint.Y - GameObject.Transform.Position.Y);

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }

            velocity *= speed;
            GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);
        }

        public void Notify(GameEvent gameEvent)
        {

        }
        #endregion
    }
}
