using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame.ObserverPattern
{
    public class CollisionEvent : GameEvent
    {
        /// <summary>
        /// Property for getting the other gameobject which this object collides with
        /// </summary>
        public GameObject Other { get; set; }

        /// <summary>
        /// method for calling notify on the object it is attached to
        /// </summary>
        /// <param name="other">the object it collided with</param>
        public void Notify(GameObject other)
        {
            this.Other = other;

            base.Notify();
        }
    }
}
