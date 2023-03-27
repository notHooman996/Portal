using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ObserverPattern
{
    public class GameEvent
    {
        private List<IGameListener> listeners = new List<IGameListener>();

        #region methods
        /// <summary>
        /// method for adding a listener to an object
        /// </summary>
        /// <param name="listener">the listener to be added</param>
        public void Attach(IGameListener listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// method for removing a listener from an object
        /// </summary>
        /// <param name="listener">the listener to be removed</param>
        public void Detach(IGameListener listener)
        {
            listeners.Remove(listener);
        }

        /// <summary>
        /// method which calls notify on all listeners
        /// </summary>
        public void Notify()
        {
            foreach (IGameListener listener in listeners)
            {
                listener.Notify(this);
            }
        }
        #endregion
    }
}
