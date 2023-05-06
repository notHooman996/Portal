using Microsoft.Xna.Framework;
using PortalGame.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern
{
    public class End : Component
    {
        private Vector2 position;
        private SpriteRenderer spriteRenderer; 

        public End(Vector2 position)
        {
            this.position = position; 
        }

        public override void Awake()
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            spriteRenderer.SetSprite("end");
            spriteRenderer.LayerDepth = 0.5f;
            spriteRenderer.Scale = 1f;

            GameObject.Transform.Position = position;
            GameObject.Tag = "End"; 
        }
    }
}
