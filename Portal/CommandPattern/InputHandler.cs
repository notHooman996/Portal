using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Portal.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        #endregion

        #region methods
        /// <summary>
        /// constructor for the inputhandler where we add keybinds
        /// </summary>
        private InputHandler()
        {
            keybinds.Add(new KeyInfo(Keys.A), new MoveCommand(new Vector2(-1, 0)));
            keybinds.Add(new KeyInfo(Keys.D), new MoveCommand(new Vector2(1, 0)));
            keybinds.Add(new KeyInfo(Keys.W), new MoveCommand(new Vector2(0, -1)));
            keybinds.Add(new KeyInfo(Keys.S), new MoveCommand(new Vector2(0, 1)));
            //keybinds.Add(new KeyInfo(Keys.Space), new ShootCommand());
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
                if (!keyState.IsKeyDown(keyInfo.Key) && keyInfo.IsDown == true)
                {
                    keyInfo.IsDown = false;
                }
            }
        }
        #endregion
    }
}
