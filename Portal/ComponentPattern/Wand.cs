using Microsoft.Xna.Framework;
using Portal.CreationalPattern;
using Portal.MenuStates;
using PortalGame;
using PortalGame.ComponentPattern;
using PortalGame.ComponentPattern.Portals;
using PortalGame.CreationalPattern;
using SharpDX;
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
        private Vector2 playerPosition; 

        public void Aim(Vector2 direction, Vector2 playerPosition)
        {
            // save the players position 
            this.playerPosition = playerPosition; 

            int displacement = 10;

            aimDirection = new Vector2(direction.X - playerPosition.X, direction.Y - playerPosition.Y);

            if (aimDirection != Vector2.Zero)
            {
                aimDirection.Normalize();
            }

            // set position 
            GameObject.Transform.Position = playerPosition + displacement * aimDirection;

            // set rotation 
            GameObject.Transform.Rotation = (float)Math.Atan2(aimDirection.Y, aimDirection.X);
        }

        public void Shoot(PortalType portalType)
        {
            // define the rays start to the wands position (the Z is set to 0, since we work in 2D)
            Vector3 originPoint = new Vector3(playerPosition.X, playerPosition.Y, 0);
            // define the rays direction to the aimdirection (the Z is set to 0, since we work in 2D)
            Vector3 shootDirection = new Vector3(aimDirection.X, aimDirection.Y, 0); 
            // set the ray 
            Ray ray = new Ray(originPoint, shootDirection);

            // find the hit boundingbox 
            BoundingBox hitBoundingBox = FindHitBoundingBox(ray); 

            // check hit boundingbox again 
            float? distance = ray.Intersects(hitBoundingBox);

            // get the boundingbox that the ray hits 
            if (distance.HasValue)
            {
                // get the point at which the ray intersects the boundingbox 
                Vector3 intersectionPoint = ray.Position + ray.Direction * distance.Value;

                // check which side the portal should be on 
                Side side = CheckSide(intersectionPoint, GameState.BoundingBoxes[hitBoundingBox]);

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

                    // set the portals position 
                    gameObject.Transform.Position = SetPortalPosition(side, intersectionPoint, GameState.BoundingBoxes[hitBoundingBox]);

                    // add portal object to GameWorld 
                    GameState.Instantiate(gameObject);
                }
            }
        }

        private BoundingBox FindHitBoundingBox(Ray ray)
        {
            BoundingBox hitBoundingBox = GameState.BoundingBoxes.Keys.First();

            // get all boundingboxes 
            foreach (BoundingBox boundingBox in GameState.BoundingBoxes.Keys)
            {
                // check for intersection between ray and boundingbox 
                if (boundingBox.Intersects(ray) != null &&
                Vector3.Distance(GameState.BoundingBoxes[boundingBox], ray.Position) < Vector3.Distance(GameState.BoundingBoxes[hitBoundingBox], ray.Position))
                {
                    hitBoundingBox = boundingBox;
                }
            }

            return hitBoundingBox; 
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

        private Vector2 SetPortalPosition(Side side, Vector3 intersectionPoint, Vector3 platformCenter)
        {
            // set the portals position at intersectionpoint 
            Vector2 position = new Vector2(intersectionPoint.X, intersectionPoint.Y);

            // take care of outlier cases, for when platforms are in a corner, or are edges 
            switch (side)
            {
                case Side.Top:
                    // check left of 
                    if (intersectionPoint.X < platformCenter.X)
                    {
                        // corner case: check whether there is a platform up to the left
                        if (CheckIfOutlier(intersectionPoint, new Vector3(-1, -1, 0)))
                        {
                            position.X = platformCenter.X;
                        }

                        // edge case: check whether there is no a platform to the left 
                        else if (!CheckIfOutlier(intersectionPoint + new Vector3(0, -32, 0), new Vector3(-1, 1, 0)))
                        {
                            position.X = platformCenter.X;
                        }
                    }

                    // check right of 
                    else if (intersectionPoint.X > platformCenter.X)
                    {
                        // corner case: check whether there is a platform up to the right
                        if (CheckIfOutlier(intersectionPoint, new Vector3(1, -1, 0)))
                        {
                            position.X = platformCenter.X;
                        }

                        // edge case: check whether there is no a platform to the right 
                        else if (!CheckIfOutlier(intersectionPoint + new Vector3(0, -32, 0), new Vector3(1, 1, 0)))
                        {
                            position.X = platformCenter.X;
                        }
                    }

                    break;
                case Side.Bottom:
                    // check left of 
                    if (intersectionPoint.X < platformCenter.X)
                    {
                        // corner case: check whether there is a platform down to the left
                        if (CheckIfOutlier(intersectionPoint, new Vector3(-1, 1, 0)))
                        {
                            position.X = platformCenter.X;
                        }

                        // edge case: check whether there is no a platform to the left 
                        else if (!CheckIfOutlier(intersectionPoint + new Vector3(0, 32, 0), new Vector3(-1, -1, 0)))
                        {
                            position.X = platformCenter.X;
                        }
                    }

                    // check right of 
                    else if (intersectionPoint.X > platformCenter.X)
                    {
                        // corner case: check whether there is a platform down to the right
                        if (CheckIfOutlier(intersectionPoint, new Vector3(1, 1, 0)))
                        {
                            position.X = platformCenter.X;
                        }

                        // edge case: check whether there is no a platform to the right 
                        else if (!CheckIfOutlier(intersectionPoint + new Vector3(0, 32, 0), new Vector3(1, -1, 0)))
                        {
                            position.X = platformCenter.X;
                        }
                    }

                    break;
                case Side.Left:
                    // check above of 
                    if (intersectionPoint.Y < platformCenter.Y)
                    {
                        // corner case: check whether there is a platform up to the left
                        if (CheckIfOutlier(intersectionPoint, new Vector3(-1, -1, 0)))
                        {
                            position.Y = platformCenter.Y;
                        }

                        // edge case: check whether there is no a platform above 
                        else if (!CheckIfOutlier(intersectionPoint + new Vector3(-32, 0, 0), new Vector3(1, -1, 0)))
                        {
                            position.Y = platformCenter.Y;
                        }
                    }

                    // check below of 
                    else if (intersectionPoint.Y > platformCenter.Y)
                    {
                        // corner case: check whether there is a platform down to the left
                        if (CheckIfOutlier(intersectionPoint, new Vector3(-1, 1, 0)))
                        {
                            position.Y = platformCenter.Y;
                        }

                        // edge case: check whether there is no a platform below 
                        else if (!CheckIfOutlier(intersectionPoint + new Vector3(-32, 0, 0), new Vector3(1, 1, 0)))
                        {
                            position.Y = platformCenter.Y;
                        }
                    }

                    break;
                case Side.Right:
                    // check above of 
                    if (intersectionPoint.Y < platformCenter.Y)
                    {
                        // corner case: check whether there is a platform up to the right
                        if (CheckIfOutlier(intersectionPoint, new Vector3(1, -1, 0)))
                        {
                            position.Y = platformCenter.Y;
                        }

                        // edge case: check whether there is no a platform above 
                        else if (!CheckIfOutlier(intersectionPoint + new Vector3(32, 0, 0), new Vector3(-1, -1, 0)))
                        {
                            position.Y = platformCenter.Y;
                        }
                    }

                    // check below of 
                    else if (intersectionPoint.Y > platformCenter.Y)
                    {
                        // corner case: check whether there is a platform down to the right
                        if (CheckIfOutlier(intersectionPoint, new Vector3(1, 1, 0)))
                        {
                            position.Y = platformCenter.Y;
                        }

                        // edge case: check whether there is no a platform below 
                        else if (!CheckIfOutlier(intersectionPoint + new Vector3(32, 0, 0), new Vector3(-1, 1, 0)))
                        {
                            position.Y = platformCenter.Y;
                        }
                    }

                    break;
            }

            return position; 
        }

        private bool CheckIfOutlier(Vector3 rayStartpoint, Vector3 rayDirection)
        {
            float distance = 100;

            // make ray that shoots diagonally 
            Ray ray = new Ray(rayStartpoint, rayDirection);

            // find closest boundingbox the ray hits 
            BoundingBox hitBoundingBox = FindHitBoundingBox(ray);

            // get the distance for the hit boundingbox 
            float? boundingboxDistance = ray.Intersects(hitBoundingBox);

            if (boundingboxDistance.HasValue)
            {
                // check if boundingbox is inside corner distance 
                if (boundingboxDistance < distance)
                {
                    return true; 
                }
            }

            return false; 
        }
    }
}
