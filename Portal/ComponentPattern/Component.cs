using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern
{
    public abstract class Component : ICloneable
    {
        /// <summary>
        /// property for getting or setting a gameobject
        /// </summary>
        public GameObject GameObject { get; set; }

        #region methods
        /// <summary>
        /// handed down
        /// </summary>
        public virtual void Awake()
        {

        }

        /// <summary>
        /// handed down
        /// </summary>
        public virtual void Start()
        {

        }

        /// <summary>
        /// handed down
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// handed down
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// used for cloning a component
        /// </summary>
        /// <returns>the object that is cloned</returns>
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
