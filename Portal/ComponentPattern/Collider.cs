using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.ObserverPattern;

namespace Portal.ComponentPattern
{
    public class Collider : Component
    {
        #region fields
        private SpriteRenderer spriteRenderer = new SpriteRenderer();
        private Texture2D texture;

        private Lazy<List<RectangleData>> rectangles;

        private bool loaded = false;
        #endregion

        #region properties
        /// <summary>
        /// used to get or set the collsion event
        /// </summary>
        public CollisionEvent CollisionEvent { get; private set; } = new CollisionEvent();

        /// <summary>
        /// used to return a rectangle based on the objects position and its sprite
        /// </summary>
        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle
                    (
                        (int)(GameObject.Transform.Position.X - (spriteRenderer.Sprite.Width * spriteRenderer.Scale) / 2),
                        (int)(GameObject.Transform.Position.Y - (spriteRenderer.Sprite.Height * spriteRenderer.Scale) / 2),
                        (int)(spriteRenderer.Sprite.Width * spriteRenderer.Scale),
                        (int)(spriteRenderer.Sprite.Height * spriteRenderer.Scale)
                    );
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// runs at the begining
        /// </summary>
        public override void Start()
        {
            //returns a lazy list from createrectangles
            rectangles = new Lazy<List<RectangleData>>(() => CreateRectangles());
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent<SpriteRenderer>();
            texture = GameWorld.Instance.Content.Load<Texture2D>("Pixel");

            CreateRectangles();
        }

        /// <summary>
        /// updates each frame
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //UpdatePixelCollider();
            CheckCollision();
        }

        //use this if you want to draw the collisionboxes and pixelcollision
        public override void Draw(SpriteBatch spriteBatch)
        {
            //foreach (RectangleData rd in rectangles.Value)
            //{
            //    DrawRectangle(rd.Rectangle, spriteBatch);
            //}

            //DrawRectangle(CollisionBox, spriteBatch);
        }

        public void DrawRectangle(Rectangle collisionBox, SpriteBatch spriteBatch)
        {
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

            spriteBatch.Draw(texture, topLine, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, bottomLine, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, rightLine, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, leftLine, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 1);
        }

        /// <summary>
        /// method for checking if objects collide
        /// </summary>
        private void CheckCollision()
        {
            foreach (Collider other in GameWorld.Instance.Colliders)
            {
                if (other != this && other.CollisionBox.Intersects(CollisionBox))
                {
                    CollisionEvent.Notify(other.GameObject);
                    //return;

                    //foreach (RectangleData myRectangleData in rectangles.Value)
                    //{
                    //    foreach (RectangleData otherRectangleData in other.rectangles.Value)
                    //    {
                    //        if (myRectangleData.Rectangle.Intersects(otherRectangleData.Rectangle))
                    //        {
                    //            CollisionEvent.Notify(other.GameObject);
                    //            return;
                    //        }
                    //    }
                    //}
                }
            }
        }

        /// <summary>
        /// creates pixel collision by making pixels at the border of a sprite where the alpha lvl is not transparent
        /// </summary>
        /// <returns>a list of pixels</returns>
        private List<RectangleData> CreateRectangles()
        {
            loaded = true;

            List<RectangleData> pixels = new List<RectangleData>();

            List<Color[]> lines = new List<Color[]>();

            for (int y = 0; y < spriteRenderer.Sprite.Height; y++)
            {
                Color[] colors = new Color[spriteRenderer.Sprite.Width];
                spriteRenderer.Sprite.GetData(0, new Rectangle(0, y, spriteRenderer.Sprite.Width, 1), colors, 0, spriteRenderer.Sprite.Width);
                lines.Add(colors);
            }

            for (int y = 0; y < lines.Count; y++)
            {
                //lines in the list [y]
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x].A != 0)
                    {
                        if ((x == 0)    // check leftside corner
                            || (x == lines[y].Length) //right side corner
                            || (x > 0 && lines[y][x - 1].A == 0) //looks at the pixel to the left

                            || (x < lines[y].Length - 1 && lines[y][x + 1].A == 0)    //looks at the pixel to the right
                            || (y == 0) // checsk rightside corner
                            || (y > 0 && lines[y - 1][x].A == 0) //looks at the pixel above               
                            || (y < lines.Count - 1 && lines[y + 1][x].A == 0)) //looks at the pixel below
                        {
                            RectangleData rd = new RectangleData(x, y);
                            pixels.Add(rd);
                        }

                    }
                }
            }
            return pixels;
        }

        /// <summary>
        /// updates the position of the pixels
        /// </summary>
        private void UpdatePixelCollider()
        {
            if (loaded)
            {
                for (int i = 0; i < rectangles.Value.Count; i++)
                {
                    rectangles.Value[i].UpdatePosition(GameObject, spriteRenderer.Sprite.Width, spriteRenderer.Sprite.Height);
                }
            }

        }
        #endregion
    }

    public class RectangleData
    {
        #region properties
        /// <summary>
        /// Property for getting and setting a pixel
        /// </summary>
        public Rectangle Rectangle { get; set; }

        /// <summary>
        /// Property for getting and setting the value of X used in updating the position of the rectangle
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Property for getting and setting the value of Y used in updating the position of the rectangle
        /// </summary>
        public int Y { get; private set; }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor which takes 2 parameters
        /// </summary>
        /// <param name="x">positions x value</param>
        /// <param name="y">positions y value</param>
        public RectangleData(int x, int y)
        {
            this.Rectangle = new Rectangle();
            this.X = x;
            this.Y = y;
        }
        #endregion

        #region methods
        /// <summary>
        /// Updates the position of the pixel based on the parameters
        /// </summary>
        /// <param name="gameObject">object which contains the pixel</param>
        /// <param name="width">objects width</param>
        /// <param name="height">objects height</param>
        public void UpdatePosition(GameObject gameObject, int width, int height)
        {
            Rectangle = new Rectangle((int)gameObject.Transform.Position.X + X - width / 2, (int)gameObject.Transform.Position.Y + Y - height / 2, 1, 1);
        }
        #endregion
    }
}
