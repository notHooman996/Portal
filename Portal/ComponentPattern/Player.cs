﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Portal.CommandPattern;
using Portal.ObserverPattern;
using Portal.ObserverPattern.TileCollisionEvents;
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

        private float jumpCooldown = 0;
        private bool hasJumped = false; 

        private Vector2 gravity;

        private Dictionary<Keys, ButtonState> movementKeys = new Dictionary<Keys, ButtonState>();
        #endregion

        #region methods
        /// <summary>
        /// method used for running logic as the first thing on the component
        /// </summary>
        public override void Start()
        {
            speed = 250;
            gravity = new Vector2(0, 0.9f) * speed;

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
                GameObject.Transform.Translate(gravity * GameWorld.DeltaTime);
            }

            // update jump cooldown 
            jumpCooldown += GameWorld.DeltaTime;

            if (hasJumped)
            {
                if(jumpCooldown < 0.1f)
                {
                    Vector2 velocity = new Vector2(0, -5f) * speed;
                    GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);
                }
                else if (jumpCooldown < 0.3f)
                {
                    Vector2 velocity = new Vector2(0, -1.5f) * speed;
                    GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);
                }
                else
                {
                    hasJumped = false;
                }
            }

            // make player turn forward 
            //if (movementKeys[Keys.A] == ButtonState.UP && movementKeys[Keys.D] == ButtonState.UP)
            //{
            //    animator.PlayAnimation("Forward");
            //}
        }

        public void Jump()
        {
            if (!isFalling && jumpCooldown > 0.5f)
            {
                //Debug.WriteLine("jump");

                hasJumped = true; 

                jumpCooldown = 0; 
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
                    //Debug.WriteLine("tile");
                    isFalling = true;
                }
                if (other.Tag == "CollisionTile")
                {
                    //Debug.WriteLine("collision tile");
                    isFalling = true;
                }
            }


            if(gameEvent is TopCollisionEvent)
            {
                GameObject other = (gameEvent as TopCollisionEvent).Other;

                if (other.Tag == "CollisionTile")
                {
                    //Debug.WriteLine("top");
                    isFalling = false;

                    GameObject.Transform.Position = new Vector2(GameObject.Transform.Position.X, 
                                                                other.Transform.Position.Y - (spriteRenderer.Sprite.Height + spriteRenderer.Origin.Y)); 
                }
            }
            if (gameEvent is BottomCollisionEvent)
            {
                GameObject other = (gameEvent as BottomCollisionEvent).Other;

                if (other.Tag == "CollisionTile")
                {
                    //Debug.WriteLine("bottom");

                    jumpCooldown = 1; 

                    SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    GameObject.Transform.Position = new Vector2(GameObject.Transform.Position.X,
                                                                other.Transform.Position.Y + (otherSpriteRenderer.Sprite.Height + spriteRenderer.Sprite.Height / 2));
                }
            }
            if (gameEvent is RightCollisionEvent)
            {
                GameObject other = (gameEvent as RightCollisionEvent).Other;

                if (other.Tag == "CollisionTile")
                {
                    //Debug.WriteLine("right");

                    SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    GameObject.Transform.Position = new Vector2(other.Transform.Position.X + otherSpriteRenderer.Sprite.Width + spriteRenderer.Origin.X,
                                                                GameObject.Transform.Position.Y);
                }
            }
            if (gameEvent is LeftCollisionEvent)
            {
                GameObject other = (gameEvent as LeftCollisionEvent).Other;

                if (other.Tag == "CollisionTile")
                {
                    //Debug.WriteLine("left");

                    GameObject.Transform.Position = new Vector2(other.Transform.Position.X - (spriteRenderer.Sprite.Width + spriteRenderer.Origin.X),
                                                                GameObject.Transform.Position.Y);
                }
            }


            if (gameEvent is ButtonEvent)
            {
                ButtonEvent buttonEvent = (gameEvent as ButtonEvent);

                movementKeys[buttonEvent.Key] = buttonEvent.State;
            }

        }
        #endregion
    }
}
