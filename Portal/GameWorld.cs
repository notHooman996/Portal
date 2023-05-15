using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Portal.BuilderPattern;
using Portal.MenuStates;
using PortalGame.BuilderPattern;
using PortalGame.ComponentPattern;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Component = PortalGame.ComponentPattern.Component;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;

namespace PortalGame
{
    public class GameWorld : Game
    {
        #region singleton
        private static GameWorld instance;

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }
        #endregion

        #region fields 
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // handle states 
        private State currentState;
        private State nextState;
        #endregion

        #region properties
        public static Vector2 ScreenSize { get; private set; }

        public State MenuState { get; set; }
        public State GameState { get; set; }
        #endregion

        private GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 600;

            ScreenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }

        #region methods 
        protected override void Initialize()
        {
            this.Window.Title = "Last Hope";

            // set initial states 
            GameState = new GameState(Content, GraphicsDevice, this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // handle states 
            currentState = GameState;
            currentState.LoadContent();
            nextState = null; 
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // check if a new state is available 
            if(nextState != null)
            {
                currentState = nextState;
                nextState = null; 
            }

            currentState.Update(gameTime);

            base.Update(gameTime); 
        }

        public void ChangeState(State state)
        {
            nextState = state; 
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Coral);

            currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
        #endregion
    }
}