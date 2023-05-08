using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame.ObserverPattern
{
    public enum KeyButtonState
    {
        UP,
        DOWN
    }

    public class ButtonEvent : GameEvent
    {
        public Keys Key { get; private set; }
        public KeyButtonState State { get; private set; }

        public void Notify(Keys key, KeyButtonState state)
        {
            Key = key;
            State = state;
            base.Notify();
        }
    }
}
