﻿using Microsoft.Xna.Framework;
using PortalGame.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame.ComponentPattern
{
    public class CollisionTile : Tile, IGameListener
    {
        public bool HasTop { get; set; }
        public bool HasBottom { get; set; }
        public bool HasLeft { get; set; }
        public bool HasRight { get; set; }


        public CollisionTile(Point position, int width, int height, int textureID) : base(position, width, height, textureID)
        {
            Position = position;
            Width = width;
            Height = height;
            this.textureID = textureID; 
        }

        public override void Awake()
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            spriteRenderer.SetSprite($"Tiles\\tile{textureID}");
            spriteRenderer.LayerDepth = 0.1f;
            spriteRenderer.Scale = 2f;

            GameObject.Transform.Position = new Vector2(Position.X * Width, Position.Y * Height);
            GameObject.Tag = "CollisionTile"; 
        }

        public override void Start()
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            spriteRenderer.Origin = new Vector2(0, 0); 
        }

        public void Notify(GameEvent gameEvent)
        {
            //if (gameEvent is CollisionEvent)
            //{
            //    GameObject other = (gameEvent as CollisionEvent).Other;

            //    if (other.Tag == "Player")
            //    {
            //        Debug.WriteLine("test");
            //    }
            //}
        }
    }
}
