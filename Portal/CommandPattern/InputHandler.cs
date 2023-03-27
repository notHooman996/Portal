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
        #endregion

        #region methods
        /// <summary>
        /// constructor for the inputhandler where we add keybinds
        /// </summary>
        private InputHandler()
        {

        }

        /// <summary>
        /// method for running through our keybinds and sees if the key is pressed
        /// </summary>
        /// <param name="player">the object which has the inputhandler</param>
        public void Execute(Player player)
        {
            
        }
        #endregion
    }
}
