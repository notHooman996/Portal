using Microsoft.Xna.Framework;
using PortalGame.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern
{
    public class Platform : Component
    {
        private SpriteRenderer spriteRenderer;
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
        }
    }
}
