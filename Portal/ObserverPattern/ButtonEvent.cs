using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ObserverPattern
{
    public enum ButtonState
    {
        UP,
        DOWN
    }

    public class ButtonEvent : GameEvent
    {
        public Keys Key { get; private set; }
        public ButtonState State { get; private set; }

        public void Notify(Keys key, ButtonState state)
        {
            Key = key;
            State = state;
            base.Notify();
        }
    }
}
