using Microsoft.Xna.Framework;
using Portal.CreationalPattern;
using PortalGame.ComponentPattern;
using PortalGame.CreationalPattern;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern
{
    public class Wand : Component
    {
        private SpriteRenderer spriteRenderer; 

        public Wand()
        {

        }

        public void Shoot(Vector2 direction, BeamType beamType)
        {
            // use raycast 

            //Ray ray = new Ray(new Vector3(GameObject.Transform.Position.X, GameObject.Transform.Position.Y, 0), new Vector3(direction.X, direction.Y, 0));

            //// calculate which side the portal should spawn 
            //float? d = ray.Intersects(box);
            //if (d.HasValue)
            //{
            //    Vector3 intersection = ray.Position + ray.Direction * d.Value;
            //}


            //if (beamType == BeamType.Red)
            //{
            //    RedPortalFactory.Instance.Create();
            //}
            //else if (beamType == BeamType.Blue)
            //{

            //}
        }
    }
}
