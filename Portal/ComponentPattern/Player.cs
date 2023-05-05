﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PortalGame.CommandPattern;
using PortalGame.ComponentPattern.Beams;
using PortalGame.ComponentPattern.Portals;
using PortalGame.CreationalPattern;
using PortalGame.ObserverPattern;
using PortalGame.ObserverPattern.TileCollisionEvents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ButtonState = PortalGame.ObserverPattern.ButtonState;

namespace PortalGame.ComponentPattern
{
    public class Player : Component, IGameListener
    {
        #region fields
        private SpriteRenderer spriteRenderer;
        private float speed;

        private float jumpCooldown = 0;
        private bool hasJumped = false;
        private bool canJump = false; 

        private Vector2 gravity;

        private Dictionary<Keys, ButtonState> movementKeys = new Dictionary<Keys, ButtonState>();

        private bool canShoot;

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

            // set initial position to middle of the map 
            GameObject.Transform.Position = new Vector2(100, 100);
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

            // make player fall 
            GameObject.Transform.Translate(gravity * GameWorld.DeltaTime);

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

            canJump = false; 

            // make player turn forward 
            //if (movementKeys[Keys.A] == ButtonState.UP && movementKeys[Keys.D] == ButtonState.UP)
            //{
            //    animator.PlayAnimation("Forward");
            //}
        }

        public void Jump()
        {
            if (canJump && jumpCooldown > 0.5f)
            {
                hasJumped = true; 

                jumpCooldown = 0; 
            }
        }

        public void Shoot(Vector2 direction, BeamType beamType)
        {
            GameObject beamObject = BeamFactory.Instance.Create(beamType);
            beamObject.Transform.Position = GameObject.Transform.Position;

            //Atan2 gives an angle measured in radians, between -Pi abd Pi
            beamObject.Transform.Rotation = MathF.Atan2(direction.Y - beamObject.Transform.Position.Y, direction.X - beamObject.Transform.Position.X);

            if (beamType == BeamType.Red)
            {
                RedBeam beam = (RedBeam)beamObject.GetComponent<RedBeam>();
                beam.Direction = new Vector2(direction.X - beamObject.Transform.Position.X, direction.Y - beamObject.Transform.Position.Y);
            } 
            else if(beamType == BeamType.Blue)
            {
                BlueBeam beam = (BlueBeam)beamObject.GetComponent<BlueBeam>();
                beam.Direction = new Vector2(direction.X - beamObject.Transform.Position.X, direction.Y - beamObject.Transform.Position.Y);
            }

            GameWorld.Instance.Instantiate(beamObject); 
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

                // make sure both portals exists 
                RedPortal redPortal = (RedPortal)GameWorld.Instance.FindObjectOfType<RedPortal>();
                BluePortal bluePortal = (BluePortal)GameWorld.Instance.FindObjectOfType<BluePortal>();
                if (redPortal != null && bluePortal != null)
                {
                    // when colliding with portal 
                    if (other.Tag == BeamType.Red.ToString())
                    {
                        // get blue portals position 
                        GameObject bluePortalObject = GameWorld.Instance.GetObjectOfType<BluePortal>();

                        // set players position to blue portal, plus offset 
                        GameObject.Transform.Position = bluePortalObject.Transform.Position + (bluePortal.PlayerDisplacement * spriteRenderer.Sprite.Width); 
                    }
                    if (other.Tag == BeamType.Blue.ToString())
                    {
                        // get red portals position 
                        GameObject redPortalObject = GameWorld.Instance.GetObjectOfType<RedPortal>();

                        // set player position to red portal, plus offset 
                        GameObject.Transform.Position = redPortalObject.Transform.Position + (redPortal.PlayerDisplacement * spriteRenderer.Sprite.Width);
                    }
                }
            }


            if(gameEvent is TopCollisionEvent)
            {
                GameObject other = (gameEvent as TopCollisionEvent).Other;

                if (other.Tag == "Platform")
                {
                    //Debug.WriteLine("top");
                    //isFalling = false;

                    canJump = true; 

                    SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    GameObject.Transform.Position = new Vector2(GameObject.Transform.Position.X,
                                                                other.Transform.Position.Y - (otherSpriteRenderer.Origin.Y + spriteRenderer.Origin.Y));
                }
            }
            if (gameEvent is BottomCollisionEvent)
            {
                GameObject other = (gameEvent as BottomCollisionEvent).Other;

                if (other.Tag == "Platform")
                {
                    //Debug.WriteLine("bottom");

                    jumpCooldown = 1; 

                    SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    GameObject.Transform.Position = new Vector2(GameObject.Transform.Position.X,
                                                                other.Transform.Position.Y + (otherSpriteRenderer.Origin.Y + spriteRenderer.Origin.Y));
                }
            }
            if (gameEvent is LeftCollisionEvent)
            {
                GameObject other = (gameEvent as LeftCollisionEvent).Other;

                if (other.Tag == "Platform")
                {
                    //Debug.WriteLine("left");

                    SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    GameObject.Transform.Position = new Vector2(other.Transform.Position.X - (otherSpriteRenderer.Origin.X + spriteRenderer.Origin.X),
                                                                GameObject.Transform.Position.Y);
                }
            }
            if (gameEvent is RightCollisionEvent)
            {
                GameObject other = (gameEvent as RightCollisionEvent).Other;

                if (other.Tag == "Platform")
                {
                    //Debug.WriteLine("right");

                    SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    GameObject.Transform.Position = new Vector2(other.Transform.Position.X + (otherSpriteRenderer.Origin.X + spriteRenderer.Origin.X),
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
