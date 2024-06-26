﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Portal.ComponentPattern;
using Portal.CreationalPattern;
using Portal.MenuStates;
using PortalGame.CommandPattern;
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
using KeyButtonState = PortalGame.ObserverPattern.KeyButtonState;

namespace PortalGame.ComponentPattern
{
    public class Player : Component, IGameListener
    {
        #region fields
        private SpriteRenderer spriteRenderer;
        private Collider collider; 
        private float speed;
        private Vector2 startPosition;

        private float jumpTime;
        private bool isJumping;
        private bool canJump; 

        private Vector2 gravity;

        private static Dictionary<Keys, KeyButtonState> movementKeys = new Dictionary<Keys, KeyButtonState>();
        private Animator animator;

        // portal timer 
        private float portalTimer = 0;
        private float portalTime = 1; 

        private GameObject Wand { get; set; }
        #endregion

        public Player(Vector2 position)
        {
            startPosition = position; 
        }

        #region methods
        /// <summary>
        /// method used for running logic as the first thing on the component
        /// </summary>
        public override void Start()
        {
            speed = 250;
            gravity = new Vector2(0, 0.9f) * speed;

            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            spriteRenderer.SetSprite("Player\\Idle\\idle");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;

            animator = GameObject.GetComponent<Animator>() as Animator; 

            // set initial position to middle of the map 
            GameObject.Transform.Position = startPosition;
            GameObject.Tag = "Player";

            collider = GameObject.GetComponent<Collider>() as Collider;

            // set collisionbox 
            SetCollisionBox();

            // set movement keys 
            movementKeys = new Dictionary<Keys, KeyButtonState>();
            movementKeys.Add(Keys.A, KeyButtonState.UP);
            movementKeys.Add(Keys.D, KeyButtonState.UP);
            movementKeys.Add(Keys.W, KeyButtonState.UP);

            // create wand 
            CreateWand();
        }

        private void CreateWand()
        {
            Wand = WandFactory.Instance.Create(PortalType.Blue);

            GameState.Instantiate(Wand);
        }

        public void RemoveWand()
        {
            GameState.Destroy(Wand); 
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
            GameObject.Transform.Translate(gravity * GameState.DeltaTime);

            if (isJumping)
            {
                // update jump time 
                jumpTime += GameState.DeltaTime;
                if (jumpTime < 0.1f)
                {
                    Vector2 velocity = new Vector2(0, -5f) * speed;
                    GameObject.Transform.Translate(velocity * GameState.DeltaTime);
                }
                else if (jumpTime < 0.3f)
                {
                    Vector2 velocity = new Vector2(0, -1.5f) * speed;
                    GameObject.Transform.Translate(velocity * GameState.DeltaTime);
                }
                else
                {
                    isJumping = false;
                }
            }

            // make player turn forward 
            if (movementKeys[Keys.A] == KeyButtonState.UP && movementKeys[Keys.D] == KeyButtonState.UP)
            {
                animator.PlayAnimation("Idle");
            }

            // update collisionbox 
            if(collider.CollisionBox.X != GameObject.Transform.Position.X + spriteRenderer.Sprite.Width / 2 ||
               collider.CollisionBox.Y != GameObject.Transform.Position.Y + spriteRenderer.Sprite.Height / 2)
            {
                SetCollisionBox();
            }

            // update portal timer 
            portalTimer += GameState.DeltaTime; 
        }

        private void SetCollisionBox()
        {
            collider.CollisionBox = new Rectangle(
                                                  (int)(GameObject.Transform.Position.X - spriteRenderer.Sprite.Width / 2),
                                                  (int)(GameObject.Transform.Position.Y - spriteRenderer.Sprite.Height / 2),
                                                  (int)(spriteRenderer.Sprite.Width),
                                                  (int)(spriteRenderer.Sprite.Height)
                                                  );
        }

        public void Jump()
        {
            if (canJump)
            {
                isJumping = true; 
                jumpTime = 0;
                canJump = false; 
            }
        }

        public void Aim(Vector2 direction)
        {
            Wand wand = Wand.GetComponent<Wand>() as Wand;
            wand.Aim(direction, GameObject.Transform.Position); 
        }

        public void Shoot(PortalType portalType)
        {
            Wand wand = Wand.GetComponent<Wand>() as Wand;
            wand.Shoot(portalType); 
        }

        public void Move(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }

            velocity *= speed;
            GameObject.Transform.Translate(velocity * GameState.DeltaTime);

            // animation 
            if(velocity.X > 0)
            {
                animator.PlayAnimation("WalkRight");
            }
            else if(velocity.X < 0)
            {
                animator.PlayAnimation("WalkLeft");
            }
        }

        public void Notify(GameEvent gameEvent)
        {
            if(gameEvent is CollisionEvent)
            {
                GameObject other = (gameEvent as CollisionEvent).Other;

                // collision with portals 
                // make sure both portals exists 
                RedPortal redPortal = (RedPortal)GameState.FindObjectOfType<RedPortal>();
                BluePortal bluePortal = (BluePortal)GameState.FindObjectOfType<BluePortal>();
                if (redPortal != null && bluePortal != null)
                {
                    // check timer 
                    if (portalTimer > portalTime)
                    {
                        // when colliding with portal 
                        if (other.Tag == PortalType.Red.ToString())
                        {
                            // get blue portals position 
                            GameObject bluePortalObject = GameState.GetObjectOfType<BluePortal>();

                            // set players position to blue portal, plus offset 
                            GameObject.Transform.Position = bluePortalObject.Transform.Position + (bluePortal.PlayerDisplacement * spriteRenderer.Sprite.Width);

                            SetCollisionBox();

                            portalTimer = 0;
                        }
                        if (other.Tag == PortalType.Blue.ToString())
                        {
                            // get red portals position 
                            GameObject redPortalObject = GameState.GetObjectOfType<RedPortal>();

                            // set player position to red portal, plus offset 
                            GameObject.Transform.Position = redPortalObject.Transform.Position + (redPortal.PlayerDisplacement * spriteRenderer.Sprite.Width);

                            SetCollisionBox();

                            portalTimer = 0;
                        }
                    }
                }

                // collision with end 
                if(other.Tag == "End")
                {
                    Debug.WriteLine("end reached");
                }
            }


            if(gameEvent is TopCollisionEvent)
            {
                GameObject other = (gameEvent as TopCollisionEvent).Other;

                if (other.Tag == "Platform")
                {
                    // player can jump when touching top of platform 
                    canJump = true; 

                    SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    GameObject.Transform.Position = new Vector2(GameObject.Transform.Position.X,
                                                                other.Transform.Position.Y - (otherSpriteRenderer.Origin.Y + spriteRenderer.Origin.Y));
                    
                    SetCollisionBox();
                }
            }
            if (gameEvent is BottomCollisionEvent)
            {
                GameObject other = (gameEvent as BottomCollisionEvent).Other;

                if (other.Tag == "Platform")
                {
                    // make sure player can not jump through platform 
                    isJumping = false; 

                    SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    GameObject.Transform.Position = new Vector2(GameObject.Transform.Position.X,
                                                                other.Transform.Position.Y + (otherSpriteRenderer.Origin.Y + spriteRenderer.Origin.Y));
                    
                    SetCollisionBox();
                }
            }
            if (gameEvent is LeftCollisionEvent)
            {
                GameObject other = (gameEvent as LeftCollisionEvent).Other;

                if (other.Tag == "Platform")
                {
                    SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    GameObject.Transform.Position = new Vector2(other.Transform.Position.X - (otherSpriteRenderer.Origin.X + spriteRenderer.Origin.X),
                                                                GameObject.Transform.Position.Y);
                    
                    SetCollisionBox();
                }
            }
            if (gameEvent is RightCollisionEvent)
            {
                GameObject other = (gameEvent as RightCollisionEvent).Other;

                if (other.Tag == "Platform")
                {
                    SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    GameObject.Transform.Position = new Vector2(other.Transform.Position.X + (otherSpriteRenderer.Origin.X + spriteRenderer.Origin.X),
                                                                GameObject.Transform.Position.Y);
                    
                    SetCollisionBox();
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
