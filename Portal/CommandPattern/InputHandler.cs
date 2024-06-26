﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Portal.MenuStates;
using PortalGame.ComponentPattern;
using PortalGame.ComponentPattern.Portals;
using PortalGame.CreationalPattern;
using PortalGame.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace PortalGame.CommandPattern
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
        private ButtonEvent buttonEvent = new ButtonEvent();

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
            Player player = GameState.FindObjectOfType<Player>() as Player;
            buttonEvent.Attach(player); 

            keybinds.Add(new KeyInfo(Keys.A), new MoveCommand(new Vector2(-1, 0)));
            keybinds.Add(new KeyInfo(Keys.D), new MoveCommand(new Vector2(1, 0)));
            keybinds.Add(new KeyInfo(Keys.W), new JumpCommand());
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
                    buttonEvent.Notify(keyInfo.Key, KeyButtonState.DOWN);
                    keyInfo.IsDown = true;
                }
                if (!keyState.IsKeyDown(keyInfo.Key) && keyInfo.IsDown)
                {
                    buttonEvent.Notify(keyInfo.Key, KeyButtonState.UP);
                    keyInfo.IsDown = false;
                }
            }


            MouseState mouseState = Mouse.GetState();

            leftClickCooldown += GameState.DeltaTime; 
            rightClickCooldown += GameState.DeltaTime;

            if (mouseState.X >= 0 && mouseState.Y >= 0 &&
               mouseState.X <= GameWorld.ScreenSize.X && mouseState.Y <= GameWorld.ScreenSize.Y)
            {
                // get the mouse position 
                Vector2 mousePoint = new Vector2(mouseState.X, mouseState.Y);
                // invert the camera transform matrix 
                Matrix invertedMatrix = Matrix.Invert(GameState.Camera.Transform);
                // transform the mouse point with the inverted matrix 
                Vector2 direction = Vector2.Transform(mousePoint, invertedMatrix);

                player.Aim(direction); 

                if (mouseState.LeftButton == ButtonState.Pressed && leftClickCooldown > cooldown)
                {
                    player.Shoot(PortalType.Red); 

                    leftClickCooldown = 0;
                }

                if (mouseState.RightButton == ButtonState.Pressed && rightClickCooldown > cooldown)
                {
                    player.Shoot(PortalType.Blue);

                    rightClickCooldown = 0;
                }
            }
        }
        #endregion
    }
}
