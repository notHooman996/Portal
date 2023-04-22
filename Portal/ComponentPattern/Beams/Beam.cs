using Microsoft.Xna.Framework;
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

        public override void Awake()
        {
            speed = 250; 

            //GameObject.Transform.Position = new Vector2(0, 900); 

            base.Awake();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 velocity = new Vector2(1, -1); 

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            
            GameObject.Transform.Translate(velocity * speed * GameWorld.DeltaTime);

            Debug.WriteLine($"x: {GameObject.Transform.Position.X}\ty: {GameObject.Transform.Position.Y}"); 

            base.Update(gameTime);
        }

        public void Notify(GameEvent gameEvent)
        {
            
        }
    }
}
