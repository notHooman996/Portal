using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Portal.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Portal.CommandPattern
{
    public class InputHandler
    {
        #region singleton
        private static InputHandler instance;

        public static InputHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputHandler();
                }
                return instance;
            }
        }
        #endregion

        #region fields
        private Dictionary<KeyInfo, ICommand> keybinds = new Dictionary<KeyInfo, ICommand>();

        //private Dictionary<ButtonInfo, ICommand> mousebinds = new Dictionary<ButtonInfo, ICommand>();



        float leftClickCooldown = 5;
        float rightClickCooldown = 5;
        float cooldown = 1; 

        #endregion

        #region methods
        /// <summary>
        /// constructor for the inputhandler where we add keybinds
        /// </summary>
        private InputHandler()
        {
            keybinds.Add(new KeyInfo(Keys.A), new MoveCommand(new Vector2(-1, 0)));
            keybinds.Add(new KeyInfo(Keys.D), new MoveCommand(new Vector2(1, 0)));
            keybinds.Add(new KeyInfo(Keys.W), new JumpCommand());

            //mousebinds.Add(new ButtonInfo(MouseButtons.Left), new ShootCommand());
            //mousebinds.Add(new ButtonInfo(MouseButtons.Right), new ChangeBeamCommand());

            //mousebinds.Add(new ButtonInfo(Mouse.GetState().LeftButton), new ShootCommand());
            //mousebinds.Add(new ButtonInfo(Mouse.GetState().RightButton), new ChangeBeamCommand());
        }

        /// <summary>
        /// method for running through our keybinds and sees if the key is pressed
        /// </summary>
        /// <param name="player">the object which has the inputhandler</param>
        public void Execute(Player player)
        {
            KeyboardState keyState = Keyboard.GetState();

            foreach (KeyInfo keyInfo in keybinds.Keys)
            {
                if (keyState.IsKeyDown(keyInfo.Key))
                {
                    keybinds[keyInfo].Execute(player);
                    keyInfo.IsDown = true;
                }
                if (!keyState.IsKeyDown(keyInfo.Key) && keyInfo.IsDown)
                {
                    keyInfo.IsDown = false;
                }
            }




            MouseState mouseState = Mouse.GetState();

            leftClickCooldown += GameWorld.DeltaTime; 
            rightClickCooldown += GameWorld.DeltaTime;

            if (mouseState.LeftButton == ButtonState.Pressed && leftClickCooldown > cooldown)
            {
                player.Shoot();

                leftClickCooldown = 0; 
            }

            if(mouseState.RightButton == ButtonState.Pressed && rightClickCooldown > cooldown)
            {
                player.ChangeBeam();

                rightClickCooldown = 0; 
            }


            //foreach (ButtonInfo buttonInfo in mousebinds.Keys)
            //{
            //    if(mouseState.Equals(buttonInfo.MouseButton))
            //    {
            //        player.Shoot();
            //        buttonInfo.IsDown = true; 
            //    }
            //    if(!mouseState.Equals(buttonInfo.MouseButton) && buttonInfo.IsDown)
            //    {
            //        buttonInfo.IsDown = false;
            //    }
            //}



            //Debug.WriteLine(mouseState);
            //Debug.WriteLine(mouseState.LeftButton);
            //Debug.WriteLine(mouseState.RightButton);

            //foreach (ButtonInfo buttonInfo in mousebinds.Keys)
            //{

            //    Debug.WriteLine(buttonInfo.State);


            //    if (buttonInfo.State == ButtonState.Pressed)
            //    {
            //        Debug.WriteLine("test");

            //        mousebinds[buttonInfo].Execute(player);
            //        buttonInfo.IsDown = true; 
            //    }
            //    if(buttonInfo.State == ButtonState.Released && buttonInfo.IsDown)
            //    {
            //        buttonInfo.IsDown = false; 
            //    }
            //}

        }
        #endregion
    }
}
