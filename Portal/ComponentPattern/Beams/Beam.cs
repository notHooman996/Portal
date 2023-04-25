using Microsoft.Xna.Framework;
using Portal.BuilderPattern;
using Portal.CreationalPattern;
using Portal.ObserverPattern;
using Portal.ObserverPattern.TileCollisionEvents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern.Beams
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
                        GameWorld.Instance.Destroy(GameObject);
                        return; 
                    }

                    createdPortal = true;

                    // this gameobject center position 
                    SpriteRenderer goSpriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
                    Vector2 goOrigin = new Vector2((goSpriteRenderer.Sprite.Width * goSpriteRenderer.Scale) * 0.5f,
                                                   (goSpriteRenderer.Sprite.Height * goSpriteRenderer.Scale) * 0.5f);
                    Vector2 goCenter = new Vector2(GameObject.Transform.Position.X + goOrigin.X,
                                                   GameObject.Transform.Position.Y + goOrigin.Y);
                    // other gameobject center position 
                    SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>() as SpriteRenderer;
                    Vector2 otherOrigin = new Vector2((otherSpriteRenderer.Sprite.Width * otherSpriteRenderer.Scale) * 0.5f, 
                                                      (otherSpriteRenderer.Sprite.Height * otherSpriteRenderer.Scale) * 0.5f); 
                    Vector2 otherCenter = new Vector2(other.Transform.Position.X + otherOrigin.X,
                                                      other.Transform.Position.Y + otherOrigin.Y);

                    //if(goCenter.Y < otherCenter.Y)
                    //{
                    //    Debug.WriteLine("top");
                    //    createdPortal = true; 
                    //}
                    //else if (goCenter.Y > otherCenter.Y)
                    //{
                    //    Debug.WriteLine("bottom");
                    //    createdPortal = true;
                    //}

                    //if (goCenter.X < otherCenter.X)
                    //{
                    //    Debug.WriteLine("left");
                    //    createdPortal = true;
                    //}
                    //else if (goCenter.X > otherCenter.X)
                    //{
                    //    Debug.WriteLine("right");
                    //    createdPortal = true;
                    //}

                    //float w = (goSpriteRenderer.Sprite.Width * goSpriteRenderer.Scale + otherSpriteRenderer.Sprite.Width * otherSpriteRenderer.Scale) * 0.5f;
                    //float h = (goSpriteRenderer.Sprite.Height * goSpriteRenderer.Scale + otherSpriteRenderer.Sprite.Height * otherSpriteRenderer.Scale) * 0.5f;

                    //float dx = Math.Abs(goCenter.X - otherCenter.X);
                    //float dy = Math.Abs(goCenter.Y - otherCenter.Y);

                    //if (dx <= w && dy <= h)
                    //{
                    //    float wy = w * (goCenter.Y - otherCenter.Y);
                    //    float hx = h * (goCenter.X - otherCenter.X);

                    //    if (wy > hx)
                    //    {
                    //        if (wy > -hx)
                    //        {
                    //            Debug.WriteLine("bottom");
                    //            createdPortal = true;
                    //        }
                    //        else
                    //        {
                    //            Debug.WriteLine("left");
                    //            createdPortal = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (wy > -hx)
                    //        {
                    //            Debug.WriteLine("right");
                    //            createdPortal = true;
                    //        }
                    //        else
                    //        {
                    //            Debug.WriteLine("top");
                    //            createdPortal = true;
                    //        }
                    //    }
                    //}

                    //float direction = (float)Math.Atan2(goCenter.Y - otherCenter.Y, goCenter.X - otherCenter.X);

                    //Debug.WriteLine(direction);

                    ////createdPortal = true;

                    //if (direction > Math.Atan2(-1, -1) && direction < Math.Atan2(-1, 1))
                    //{
                    //    // top
                    //    Debug.WriteLine("top");
                    //    createdPortal = true;
                    //}
                    //if (direction < Math.Atan2(1, -1) && direction > Math.Atan2(1, 1))
                    //{
                    //    // bottom
                    //    Debug.WriteLine("bottom");
                    //    createdPortal = true;
                    //}
                    //if (direction > Math.Atan2(-1, -1) && direction > Math.Atan2(1, -1))
                    //{
                    //    // left 
                    //    Debug.WriteLine("left");
                    //    createdPortal = true;
                    //}
                    //if (direction > Math.Atan2(-1, 1) && direction < Math.Atan2(1, 1))
                    //{
                    //    // right
                    //    Debug.WriteLine("right");
                    //    createdPortal = true;
                    //}


                    //float angle = (float)Math.Atan2(goCenter.Y - otherCenter.Y, goCenter.X - otherCenter.X);

                    //float distX = Math.Abs(otherCenter.X - goCenter.X);
                    //float distY = Math.Abs(otherCenter.Y - goCenter.Y);

                    //float halfW = (otherSpriteRenderer.Sprite.Width * otherSpriteRenderer.Scale + goSpriteRenderer.Sprite.Width * goSpriteRenderer.Scale) * 0.5f; 
                    //float halfH = (otherSpriteRenderer.Sprite.Height * otherSpriteRenderer.Scale + goSpriteRenderer.Sprite.Height * goSpriteRenderer.Scale) * 0.5f;

                    //Debug.WriteLine(otherSpriteRenderer.Sprite.Width * otherSpriteRenderer.Scale);
                    //Debug.WriteLine(goSpriteRenderer.Sprite.Width * goSpriteRenderer.Scale);

                    //if(distX < halfW && distY < halfH)
                    //{
                    //    if(angle >= Math.PI/4 && angle < Math.PI / 4)
                    //    {
                    //        Debug.WriteLine("right");
                    //        createdPortal = true;
                    //    }
                    //    else if(angle >= Math.PI / 4 && angle < 3*Math.PI / 4)
                    //    {
                    //        Debug.WriteLine("top");
                    //        createdPortal = true;
                    //    }
                    //    else if (angle >= -3*Math.PI / 4 && angle < -Math.PI / 4)
                    //    {
                    //        Debug.WriteLine("bottom");
                    //        createdPortal = true;
                    //    }
                    //    else
                    //    {
                    //        Debug.WriteLine("left");
                    //        createdPortal = true;
                    //    }
                    //}

                }
            }





            //if(gameEvent is TopCollisionEvent)
            //{
            //    Debug.WriteLine("top");

            //    GameObject other = (gameEvent as TopCollisionEvent).Other;

            //    if(other.Tag == "CollisionTile")
            //    {
            //        GameObject portalObject = PortalFactory.Instance.Create(BeamType);
            //        portalObject.Transform.Position = GameObject.Transform.Position;

            //        portalObject.Transform.Rotation = 0; 

            //        GameWorld.Instance.Instantiate(portalObject);

            //        createdPortal = true; 
            //    }
            //}
            //if (gameEvent is BottomCollisionEvent)
            //{
            //    Debug.WriteLine("bottom");

            //    GameObject other = (gameEvent as BottomCollisionEvent).Other;

            //    if (other.Tag == "CollisionTile")
            //    {
            //        GameObject portalObject = PortalFactory.Instance.Create(BeamType);
            //        portalObject.Transform.Position = GameObject.Transform.Position;

            //        portalObject.Transform.Rotation = 0;

            //        GameWorld.Instance.Instantiate(portalObject);

            //        createdPortal = true;
            //    }
            //}
            //if (gameEvent is RightCollisionEvent)
            //{
            //    Debug.WriteLine("right");

            //    GameObject other = (gameEvent as RightCollisionEvent).Other;

            //    if (other.Tag == "CollisionTile")
            //    {
            //        GameObject portalObject = PortalFactory.Instance.Create(BeamType);
            //        portalObject.Transform.Position = GameObject.Transform.Position;

            //        portalObject.Transform.Rotation = -3.14f;

            //        GameWorld.Instance.Instantiate(portalObject);

            //        createdPortal = true;
            //    }
            //}
            //if (gameEvent is LeftCollisionEvent)
            //{
            //    Debug.WriteLine("left");

            //    GameObject other = (gameEvent as LeftCollisionEvent).Other;

            //    if (other.Tag == "CollisionTile")
            //    {
            //        GameObject portalObject = PortalFactory.Instance.Create(BeamType);
            //        portalObject.Transform.Position = GameObject.Transform.Position;

            //        portalObject.Transform.Rotation = 3.14f;

            //        GameWorld.Instance.Instantiate(portalObject);

            //        createdPortal = true;
            //    }
            //}

            //if (createdPortal)
            //{
            //    if (gameEvent is CollisionEvent)
            //    {
            //        GameObject other = (gameEvent as CollisionEvent).Other;

            //        if (other.Tag == "CollisionTile")
            //        {
            //            GameWorld.Instance.Destroy(GameObject);
            //        }
            //    }
            //}
        }
    }
}
