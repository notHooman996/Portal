using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.CommandPattern
{
    public class KeyInfo
    {
        /// <summary>
        /// property for getting or setting wether a key is down
        /// </summary>
        public bool IsDown { get; set; }

        /// <summary>
        /// property for getting or setting a key
        /// </summary>
        public Keys Key { get; set; }

        /// <summary>
        /// constructor which takes an argument so we can define a key
        /// </summary>
        /// <param name="key">the key we want as a keybind</param>
        public KeyInfo(Keys key)
        {
            Key = key;
        }
    }
}

