using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern
{
    public class SpriteRenderer : Component
    {
        #region properties
        /// <summary>
        /// Property for getting or setting the sprite
        /// </summary>
        public Texture2D Sprite { get; set; }

        /// <summary>
        /// Property for getting or setting the origin of the sprite
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Property for getting or setting the layerdepth of the image
        /// </summary>
        public float LayerDepth { get; set; }

        /// <summary>
        /// property for getting or setting the scale of the image
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// No flip: 0
        /// Flip vertically: 2 
        /// </summary>
        public SpriteEffects Flip { get; set; }

        public Color Color { get; set; }
        #endregion

        #region methods
        /// <summary>
        /// method for setting the origin for every sprite in the middle of the image
        /// </summary>
        public override void Start()
        {
            Origin = new Vector2((Sprite.Width * Scale) / 2, (Sprite.Height * Scale) / 2); 
        }

        /// <summary>
        /// method for setting the sprite based on the input string
        /// </summary>
        /// <param name="spriteName">path and name of the file</param>
        public void SetSprite(string spriteName)
        {
            Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
        }

        /// <summary>
        /// method for drawing a sprite to the screen
        /// </summary>
        /// <param name="spriteBatch">passed in from gameworld so we can draw through it</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GameObject.Transform.Position, null, Color, GameObject.Transform.Rotation, Origin, Scale, Flip, LayerDepth);
        }
        #endregion
    }
}
