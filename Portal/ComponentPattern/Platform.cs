using Microsoft.Xna.Framework;
using Portal.MenuStates;
using PortalGame;
using PortalGame.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoundingBox = Microsoft.Xna.Framework.BoundingBox;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace Portal.ComponentPattern
{
    public class Platform : Component
    {
        private SpriteRenderer spriteRenderer;
        private Collider collider; 
        private Vector2 position;
        private int Id; 

        public Platform(Vector2 position, int id)
        {
            this.position = position;
            Id = id; 
        }

        public override void Awake()
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            spriteRenderer.SetSprite($"Tiles\\tile{Id}");
            spriteRenderer.LayerDepth = 0.5f;
            spriteRenderer.Scale = 1f; 

            GameObject.Transform.Position = new Vector2(position.X, position.Y);

            collider = GameObject.GetComponent<Collider>() as Collider;

            // set collisionbox 
            collider.CollisionBox = new Rectangle(
                                                  (int)(GameObject.Transform.Position.X - spriteRenderer.Sprite.Width / 2),
                                                  (int)(GameObject.Transform.Position.Y - spriteRenderer.Sprite.Height / 2),
                                                  (int)(spriteRenderer.Sprite.Width),
                                                  (int)(spriteRenderer.Sprite.Height)
                                                  );

            // set boundingbox (Z is 0, since we work in 2D)
            Vector3 topLeftCorner = new Vector3(
                                                GameObject.Transform.Position.X - spriteRenderer.Sprite.Width / 2,
                                                GameObject.Transform.Position.Y - spriteRenderer.Sprite.Height / 2, 
                                                0
                                                );
            Vector3 bottomRightCorner = new Vector3(
                                                GameObject.Transform.Position.X + spriteRenderer.Sprite.Width / 2,
                                                GameObject.Transform.Position.Y + spriteRenderer.Sprite.Height / 2, 
                                                0
                                                );

            GameState.BoundingBoxes.Add(
                                                 new BoundingBox(topLeftCorner, bottomRightCorner), 
                                                 new Vector3(GameObject.Transform.Position.X, GameObject.Transform.Position.Y, 0)
                                                 ); 
        }
    }
}
