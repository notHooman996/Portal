﻿using Microsoft.Xna.Framework;
using Portal.ComponentPattern;
using Portal.MenuStates;
using PortalGame.BuilderPattern;
using PortalGame.CreationalPattern;
using PortalGame.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame.ComponentPattern.Portals
{
    public enum PortalType
    {
        Red,
        Blue
    }

    public enum Side
    {
        Top,
        Bottom,
        Left,
        Right,
        None
    }

    public class Portal : Component, IGameListener
    {
        private SpriteRenderer spriteRenderer;
        private Collider collider; 
        private Animator animator;
        public string AnimationName { get; set; }

        public Vector2 PlayerDisplacement { get; set; }

        public override void Start()
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer; 
            animator = GameObject.GetComponent<Animator>() as Animator;
            animator.PlayAnimation(AnimationName);

            collider = GameObject.GetComponent<Collider>() as Collider;
            // set collisionbox 
            SetCollisionBox(); 
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void SetCollisionBox()
        {
            collider.CollisionBox = new Rectangle(
                                                  (int)(GameObject.Transform.Position.X - spriteRenderer.Sprite.Width / 2),
                                                  (int)(GameObject.Transform.Position.Y - spriteRenderer.Sprite.Height / 2),
                                                  (int)(spriteRenderer.Sprite.Width),
                                                  (int)(spriteRenderer.Sprite.Height)
                                                  );
        }

        public void Notify(GameEvent gameEvent)
        {
            if (gameEvent is CollisionEvent)
            {
                GameObject other = (gameEvent as CollisionEvent).Other;

                if (GameObject.Tag == PortalType.Red.ToString() && other.Tag == PortalType.Blue.ToString())
                {
                    // remove blue portal 
                    GameState.Destroy(other);
                }
                if (GameObject.Tag == PortalType.Blue.ToString() && other.Tag == PortalType.Red.ToString())
                {
                    // remove red portal 
                    GameState.Destroy(other);
                }
            }
        }
    }
}
