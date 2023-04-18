using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern
{
    public class Tile : Component
    {
        #region fields, properties
        protected SpriteRenderer spriteRenderer;

        protected int textureID;

        public Point Position { get; protected set; }

        public int Height { get; protected set; }

        public int Width { get; protected set; }

        //public bool Walkable { get; set; }

        // for collision check 
        //private Rectangle topLine;
        //private Rectangle bottomLine;
        //private Rectangle rightLine;
        //private Rectangle leftLine;
        //private Rectangle background;
        #endregion

        public Tile(Point position, int width, int height, int textureID)
        {
            Position = position;
            Width = width;
            Height = height;
            this.textureID = textureID;
        }

        #region methods 
        public override void Awake()
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            spriteRenderer.SetSprite($"Tiles\\tile{1}");
            spriteRenderer.LayerDepth = 0.1f;
            spriteRenderer.Scale = 2f;
            GameObject.Transform.Position = new Vector2(Position.X * Width + (Width / 2), Position.Y * Height + (Height / 2));
            GameObject.Tag = "Tile"; 
        }
        #endregion
    }
}
