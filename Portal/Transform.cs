using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal
{
    public class Transform
    {
        #region properties
        /// <summary>
        /// Property for getting and setting the GameObjects Position
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Property for getting and setting the GameObjects Rotation
        /// </summary>
        public float Rotation { get; set; }
        #endregion

        #region methods
        /// <summary>
        /// Method for moving a GameObject during runtime
        /// </summary>
        /// <param name="translation">parameter we add to GameObjects position</param>
        public void Translate(Vector2 translation)
        {
            Position += translation;
        }

        /// <summary>
        /// Method for rotating a GameObject during runtime
        /// </summary>
        /// <param name="rotation">parameter we add to GameObjects rotation</param>
        public void Rotate(float rotation)
        {
            Rotation += rotation;
        }
        #endregion
    }
}
