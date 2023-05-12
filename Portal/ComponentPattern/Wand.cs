using Microsoft.Xna.Framework;
using Portal.CreationalPattern;
using PortalGame;
using PortalGame.ComponentPattern;
using PortalGame.ComponentPattern.Portals;
using PortalGame.CreationalPattern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoundingBox = Microsoft.Xna.Framework.BoundingBox;
using Ray = Microsoft.Xna.Framework.Ray;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace Portal.ComponentPattern
{
    public class Wand : Component
    {
        private SpriteRenderer spriteRenderer;
        private Vector2 aimDirection; 

        public Wand()
        {

        }

        public void Aim(Vector2 direction, Vector2 center)
        {
            int displacement = 10;

            aimDirection = new Vector2(direction.X - center.X, direction.Y - center.Y);

            if (aimDirection != Vector2.Zero)
            {
                aimDirection.Normalize();
            }

            // set position 
            GameObject.Transform.Position = center + displacement * aimDirection;

            // set rotation 
            GameObject.Transform.Rotation = (float)Math.Atan2(aimDirection.Y, aimDirection.X);
        }

        public void Shoot(PortalType portalType)
        {
            // define the rays start to the wands position (the Z is set to 0, since we work in 2D)
            Vector3 originPoint = new Vector3(GameObject.Transform.Position.X, GameObject.Transform.Position.Y, 0);
            // define the rays direction to the aimdirection (the Z is set to 0, since we work in 2D)
            Vector3 shootDirection = new Vector3(aimDirection.X, aimDirection.Y, 0); 
            // set the ray 
            Ray ray = new Ray(originPoint, shootDirection);

            // dictionary for hit boundingboxes, and their position 
            BoundingBox hitBoundingBox = GameWorld.Instance.BoundingBoxes.Keys.First();

            // get all boundingboxes 
            foreach (BoundingBox boundingBox in GameWorld.Instance.BoundingBoxes.Keys)
            {
                // check for intersection between ray and boundingbox 
                if(boundingBox.Intersects(ray) != null && 
                   Vector3.Distance(GameWorld.Instance.BoundingBoxes[boundingBox], ray.Position) < Vector3.Distance(GameWorld.Instance.BoundingBoxes[hitBoundingBox], ray.Position))
                {
                    hitBoundingBox = boundingBox; 
                }
            }

            // check hit boundingbox again 
            float? distance = ray.Intersects(hitBoundingBox);

            // get the boundingbox that the ray hits 
            if (distance.HasValue)
            {
                // get the point at which the ray intersects the boundingbox 
                Vector3 intersectionPoint = ray.Position + ray.Direction * distance.Value;

                // check which side the portal should be on 
                Side side = CheckSide(intersectionPoint, GameWorld.Instance.BoundingBoxes[hitBoundingBox]);

                // if no side set, then do not create object 
                if (side != Side.None)
                {
                    GameObject gameObject = new GameObject();

                    // create gameobject via portal factory, depending on portal type 
                    if (portalType == PortalType.Red)
                    {
                        gameObject = RedPortalFactory.Instance.Create(side);
                    }
                    else if (portalType == PortalType.Blue)
                    {
                        gameObject = BluePortalFactory.Instance.Create(side);
                    }

                    // set the portals position at intersectionpoint 
                    gameObject.Transform.Position = new Vector2(intersectionPoint.X, intersectionPoint.Y);

                    // add portal object to GameWorld 
                    GameWorld.Instance.Instantiate(gameObject);
                }
            }
        }

        private Side CheckSide(Vector3 intersectionPoint, Vector3 platformCenter)
        {
            float xDiff = intersectionPoint.X - platformCenter.X; 
            float yDiff = intersectionPoint.Y - platformCenter.Y; 
            float zDiff = intersectionPoint.Z - platformCenter.Z; 

            // check which side the intersectionpoint is closest to 
            if(Math.Abs(xDiff) > Math.Abs(yDiff))
            {
                if(xDiff > 0)
                {
                    return Side.Right; 
                }
                else
                {
                    return Side.Left; 
                }
            }
            else if(Math.Abs(yDiff) > Math.Abs(zDiff))
            {
                if(yDiff > 0)
                {
                    return Side.Bottom;
                }
                else
                {
                    return Side.Top;
                }
            }

            return Side.None; 
        }
    }
}
