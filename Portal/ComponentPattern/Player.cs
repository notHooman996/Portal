using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Portal.ComponentPattern;
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
using KeyButtonState = PortalGame.ObserverPattern.KeyButtonState;

namespace PortalGame.ComponentPattern
{
    public class Player : Component, IGameListener
    {
        #region fields
        private SpriteRenderer spriteRenderer;
        private float speed;
        private Vector2 startPosition;

        private bool canShoot;

        private float jumpTime;
        private bool isJumping;
        private bool canJump; 

        private Vector2 gravity;

        private Dictionary<Keys, KeyButtonState> movementKeys = new Dictionary<Keys, KeyButtonState>();
        private Animator animator; 
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
            spriteRenderer.LayerDepth = 0.5f;
            spriteRenderer.Scale = 1f;

            animator = GameObject.GetComponent<Animator>() as Animator; 

            // set initial position to middle of the map 
            GameObject.Transform.Position = startPosition;
            GameObject.Tag = "Player";

            movementKeys.Add(Keys.A, KeyButtonState.UP);
            movementKeys.Add(Keys.D, KeyButtonState.UP);
            movementKeys.Add(Keys.W, KeyButtonState.UP); 
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

            

            if (isJumping)
            {
                // update jump time 
                jumpTime += GameWorld.DeltaTime;
                if (jumpTime < 0.1f)
                {
                    Vector2 velocity = new Vector2(0, -5f) * speed;
                    GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);
                }
                else if (jumpTime < 0.3f)
                {
                    Vector2 velocity = new Vector2(0, -1.5f) * speed;
                    GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);
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
