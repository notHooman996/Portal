using Microsoft.Xna.Framework;
using Portal.BuilderPattern;
using Portal.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern.Portals
{
    public class Portal : Component, IGameListener
    {
        public override void Awake()
        {
            base.Awake();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Notify(GameEvent gameEvent)
        {
            
        }
    }
}
