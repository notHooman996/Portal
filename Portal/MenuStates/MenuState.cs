using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PortalGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.MenuStates
{
    public class MenuState : State
    {
        private Texture2D menuBackground;
        private Texture2D title;
        private Button newGameButton;
        private Button quitButton; 

        public MenuState(ContentManager content, GraphicsDevice graphicsDevice, GameWorld game) : base(content, graphicsDevice, game)
        {
            Vector2 titlePosition = new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 2);

            Vector2 buttonPosition = new Vector2(GameWorld.ScreenSize.X / 1.2f, GameWorld.ScreenSize.Y / 1.25f);

            // set buttons 
            newGameButton = new Button(buttonPosition, "Play", Color.White);
            quitButton = new Button(buttonPosition + new Vector2(0, 100), "Quit", Color.White);
        }

        public override void LoadContent()
        {
            // set background 
            menuBackground = content.Load<Texture2D>("Menus\\MenuBG");

            // set title 
            title = content.Load<Texture2D>("Menus\\Title");

            // set buttons 
            newGameButton.LoadContent(content);
            quitButton.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            newGameButton.Update(gameTime);
            quitButton.Update(gameTime);

            // when button is clicked 
            if (newGameButton.isClicked)
            {
                newGameButton.isClicked = false;

                // create new GameState 
                GameWorld.Instance.GameState = new GameState(content, graphicsDevice, game);
                game.ChangeState(GameWorld.Instance.GameState);
            }
            if (quitButton.isClicked)
            {
                quitButton.isClicked = false;

                // exit game 
                game.Exit(); 
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

            // draw background
            spriteBatch.Draw(menuBackground, new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 2), null, Color.White, 0f, new Vector2(menuBackground.Width / 2, menuBackground.Height / 2), 1f, SpriteEffects.None, 0.1f);

            // draw game title 
            spriteBatch.Draw(title, new Vector2(GameWorld.ScreenSize.X / 2, GameWorld.ScreenSize.Y / 1.15f), null, Color.White, 0f, new Vector2(title.Width / 2, title.Height / 2), 1f, SpriteEffects.None, 0.2f);

            // draw buttons 
            newGameButton.Draw(gameTime, spriteBatch);
            quitButton.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
