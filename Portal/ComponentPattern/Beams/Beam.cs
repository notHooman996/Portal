using Microsoft.Xna.Framework;
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
        private int speed; 

        public Vector2 Direction { get; set; }

        public BeamType BeamType { get; set; }

        private bool createdPortal = false; 

        public override void Awake()
        {
            speed = 250; 

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

            //Debug.WriteLine($"x: {GameObject.Transform.Position.X}\ty: {GameObject.Transform.Position.Y}"); 

            base.Update(gameTime);
        }

        public void Notify(GameEvent gameEvent)
        {
            if(gameEvent is CollisionEvent)
            {
                GameObject other = (gameEvent as CollisionEvent).Other;

                if(other.Tag == "CollisionTile")
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
                        Vector2 otherOrigin = new Vector2((otherSpriteRenderer.Sprite.Width * otherSpriteRenderer.Scale) * 0.5f,
                                                          (otherSpriteRenderer.Sprite.Height * otherSpriteRenderer.Scale) * 0.5f);

                        GameObject portalObject = PortalFactory.Instance.Create(BeamType);

                        Debug.WriteLine(side);

                        // place portals in middle of side, and rotate it 
                        if (side == "top")
                        {
                            portalObject.Transform.Position = new Vector2(other.Transform.Position.X + otherOrigin.X, 
                                                                          other.Transform.Position.Y);
                            portalObject.Transform.Rotation = (float)Math.PI * 0.5f; 
                        }
                        else if (side == "bottom")
                        {
                            portalObject.Transform.Position = new Vector2(other.Transform.Position.X + otherOrigin.X,
                                                                          other.Transform.Position.Y + (otherSpriteRenderer.Sprite.Height * otherSpriteRenderer.Scale));
                            portalObject.Transform.Rotation = (float)Math.PI * 0.5f;
                        }
                        else if (side == "left")
                        {
                            portalObject.Transform.Position = new Vector2(other.Transform.Position.X,
                                                                          other.Transform.Position.Y + otherOrigin.Y);
                        }
                        else if (side == "right")
                        {
                            portalObject.Transform.Position = new Vector2(other.Transform.Position.X + (otherSpriteRenderer.Sprite.Width * otherSpriteRenderer.Scale),
                                                                          other.Transform.Position.Y + otherOrigin.Y);
                        }

                        GameWorld.Instance.Instantiate(portalObject); 

                        createdPortal = true;
                    }
                }
            }
        }

        private string CheckSide(GameObject other)
        {
            string side = ""; 

            // find this gameobject center position 
            SpriteRenderer goSpriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            Vector2 goOrigin = new Vector2((goSpriteRenderer.Sprite.Width * goSpriteRenderer.Scale) * 0.5f,
                                           (goSpriteRenderer.Sprite.Height * goSpriteRenderer.Scale) * 0.5f);
            Vector2 goCenter = new Vector2(GameObject.Transform.Position.X + goOrigin.X,
                                           GameObject.Transform.Position.Y + goOrigin.Y);

            // find other gameobject center position 
            SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;
            Vector2 otherOrigin = new Vector2((otherSpriteRenderer.Sprite.Width * otherSpriteRenderer.Scale) * 0.5f,
                                              (otherSpriteRenderer.Sprite.Height * otherSpriteRenderer.Scale) * 0.5f);
            Vector2 otherCenter = new Vector2(other.Transform.Position.X + otherOrigin.X,
                                              other.Transform.Position.Y + otherOrigin.Y);

            // calculate the sum of the width and height 
            float halfWidthSum = (goSpriteRenderer.Sprite.Width * goSpriteRenderer.Scale + otherSpriteRenderer.Sprite.Width * otherSpriteRenderer.Scale) * 0.5f;
            float halfHeightSum = (goSpriteRenderer.Sprite.Height * goSpriteRenderer.Scale + otherSpriteRenderer.Sprite.Height * otherSpriteRenderer.Scale) * 0.5f;

            // calculate the difference of the two centers 
            float xCenterDiff = Math.Abs(goCenter.X - otherCenter.X);
            float yCenterDiff = Math.Abs(goCenter.Y - otherCenter.Y);

            if (xCenterDiff <= halfWidthSum && yCenterDiff <= halfHeightSum)
            {
                // calculate the overlaps between two rectangles 
                float wyOverlap = halfWidthSum * (goCenter.Y - otherCenter.Y);
                float hxOverlap = halfHeightSum * (goCenter.X - otherCenter.X);

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
