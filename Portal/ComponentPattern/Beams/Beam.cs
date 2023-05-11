using Microsoft.Xna.Framework;
using Portal.ComponentPattern;
using Portal.CreationalPattern;
using PortalGame.BuilderPattern;
using PortalGame.CreationalPattern;
using PortalGame.ObserverPattern;
using PortalGame.ObserverPattern.TileCollisionEvents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame.ComponentPattern.Beams
{
    public class Beam : Component, IGameListener
    {
        private SpriteRenderer spriteRenderer;
        private Collider collider;

        private int speed; 

        public Vector2 Direction { get; set; }

        public BeamType BeamType { get; set; }

        private bool createdPortal = false; 

        public override void Awake()
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;

            speed = 250;

            collider = GameObject.GetComponent<Collider>() as Collider;
            // set collisionbox 
            SetCollisionBox();

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

            SetCollisionBox();

            base.Update(gameTime);
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

        public void Notify(GameEvent gameEvent)
        {
            if(gameEvent is CollisionEvent)
            {
                GameObject other = (gameEvent as CollisionEvent).Other;

                if(other.Tag == "Platform")
                {
                    if (createdPortal)
                    {
                        // when a portal is created in the beams stead, remove the beam 
                        GameWorld.Instance.Destroy(GameObject);
                        return; 
                    }

                    string side = CheckSide(other);

                    if (side != "") {

                        SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;

                        GameObject portalObject;

                        Debug.WriteLine(side);

                        // place portals in middle of side, and rotate it 
                        if (side == "top")
                        {
                            if(BeamType is BeamType.Red)
                            {
                                portalObject = RedPortalFactory.Instance.Create(Side.Top); 
                            }
                            else
                            {
                                portalObject = BluePortalFactory.Instance.Create(Side.Top);
                            }

                            portalObject.Transform.Position = new Vector2(other.Transform.Position.X, 
                                                                          other.Transform.Position.Y - otherSpriteRenderer.Origin.Y);
                            
                            GameWorld.Instance.Instantiate(portalObject);
                        }
                        if (side == "bottom")
                        {
                            if (BeamType is BeamType.Red)
                            {
                                portalObject = RedPortalFactory.Instance.Create(Side.Bottom);
                            }
                            else
                            {
                                portalObject = BluePortalFactory.Instance.Create(Side.Bottom);
                            }

                            portalObject.Transform.Position = new Vector2(other.Transform.Position.X,
                                                                          other.Transform.Position.Y + otherSpriteRenderer.Origin.Y);
                            
                            GameWorld.Instance.Instantiate(portalObject);
                        }
                        if (side == "left")
                        {
                            if (BeamType is BeamType.Red)
                            {
                                portalObject = RedPortalFactory.Instance.Create(Side.Left);
                            }
                            else
                            {
                                portalObject = BluePortalFactory.Instance.Create(Side.Left);
                            }

                            portalObject.Transform.Position = new Vector2(other.Transform.Position.X - otherSpriteRenderer.Origin.X,
                                                                          other.Transform.Position.Y);
                            
                            GameWorld.Instance.Instantiate(portalObject);
                        }
                        if (side == "right")
                        {
                            if (BeamType is BeamType.Red)
                            {
                                portalObject = RedPortalFactory.Instance.Create(Side.Right);
                            }
                            else
                            {
                                portalObject = BluePortalFactory.Instance.Create(Side.Right);
                            }

                            portalObject.Transform.Position = new Vector2(other.Transform.Position.X + otherSpriteRenderer.Origin.X,
                                                                          other.Transform.Position.Y);
                            
                            GameWorld.Instance.Instantiate(portalObject);
                        }

                        createdPortal = true;
                    }
                }
            }
        }

        private string CheckSide(GameObject other)
        {
            string side = ""; 

            SpriteRenderer goSpriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;

            // calculate the sum of the width and height 
            float halfWidthSum = (goSpriteRenderer.Sprite.Width + otherSpriteRenderer.Sprite.Width) * 0.5f;
            float halfHeightSum = (goSpriteRenderer.Sprite.Height + otherSpriteRenderer.Sprite.Height) * 0.5f;

            // calculate the difference of the two centers 
            float xCenterDiff = Math.Abs(GameObject.Transform.Position.X - other.Transform.Position.X);
            float yCenterDiff = Math.Abs(GameObject.Transform.Position.Y - other.Transform.Position.Y);

            if (xCenterDiff <= halfWidthSum && yCenterDiff <= halfHeightSum)
            {
                // calculate the overlaps between two rectangles 
                float wyOverlap = halfWidthSum * (GameObject.Transform.Position.Y - other.Transform.Position.Y);
                float hxOverlap = halfHeightSum * (GameObject.Transform.Position.X - other.Transform.Position.X);

                // determine which side the collision occurred 
                if (wyOverlap > hxOverlap)
                {
                    if (wyOverlap > -hxOverlap)
                    {
                        side = "bottom"; 
                    }
                    else
                    {
                        side = "left"; 
                    }
                }
                else
                {
                    if (wyOverlap > -hxOverlap)
                    {
                        side = "right"; 
                    }
                    else
                    {
                        side = "top"; 
                    }
                }
            }

            return side; 
        }
    }
}
