using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PortalGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.MenuStates
{
    public abstract class State
    {
        protected ContentManager content;
        protected GraphicsDevice graphicsDevice;
        protected GameWorld game;

        /// <summary>
        /// Constructor for State - sets the initial variables 
        /// </summary>
        /// <param name="content">The ContentManager</param>
        /// <param name="graphicsDevice">The GraphicsDevice</param>
        /// <param name="game">The GameWorld</param>
        public State(ContentManager content, GraphicsDevice graphicsDevice, GameWorld game)
        {
            this.content = content;
            this.graphicsDevice = graphicsDevice;
            this.game = game;
        }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
