using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame
{
    public class Camera
    {
        private Vector2 center;

        private Viewport viewport;

        public Matrix Transform { get; private set; }

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void Update(Vector2 position)
        {
            float xOffset = GameWorld.Instance.LevelSize.X;
            float yOffset = GameWorld.Instance.LevelSize.Y;

            if (position.X < viewport.Width / 2)
            {
                // when player gets close to the left side of the map, the camera will stop following
                center.X = viewport.Width / 2;
            }
            else if (position.X > xOffset - (viewport.Width / 2))
            {
                // when player gets close to the right side of the map, the camera will stop following
                center.X = xOffset - (viewport.Width / 2);
            }
            else
            {
                // follow player 
                center.X = position.X;
            }

            if (position.Y < viewport.Height / 2)
            {
                // when player gets close to the top of the map, the camera will stop following
                center.Y = viewport.Height / 2;
            }
            else if (position.Y > yOffset - (viewport.Height / 2))
            {
                // when player gets close to the bottom of the map, the camera will stop following
                center.Y = yOffset - (viewport.Height / 2);
            }
            else
            {
                // follow player 
                center.Y = position.Y;
            }

            Transform = Matrix.CreateTranslation(new Vector3(-center.X + (viewport.Width / 2), -center.Y + (viewport.Height / 2), 0));
        }
    }
}
