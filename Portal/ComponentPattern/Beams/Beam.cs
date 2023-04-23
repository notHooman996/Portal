using Microsoft.Xna.Framework;
using Portal.BuilderPattern;
using Portal.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern.Beams
{
    public class Beam : Component, IGameListener
    {
        private int speed; 

        public Vector2 Direction { get; set; }

        public override void Awake()
        {
            speed = 250; 

            base.Awake();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 d = Direction; 

            if (d != Vector2.Zero)
            {
                d.Normalize();
            }

            d *= speed; 

            GameObject.Transform.Translate(d * GameWorld.DeltaTime);

            Debug.WriteLine($"x: {GameObject.Transform.Position.X}\ty: {GameObject.Transform.Position.Y}"); 

            base.Update(gameTime);
        }

        public void Notify(GameEvent gameEvent)
        {
            
        }
    }
}
