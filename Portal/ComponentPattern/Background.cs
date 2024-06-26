﻿using Microsoft.Xna.Framework;
using Portal.MenuStates;
using PortalGame;
using PortalGame.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.ComponentPattern
{
    public class Background : Component
    {
        private SpriteRenderer spriteRenderer;

        public override void Awake()
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            spriteRenderer.SetSprite("BG\\bg");
            spriteRenderer.Scale = 1f;
            spriteRenderer.LayerDepth = 0.1f;
            GameObject.Transform.Position = new Vector2(GameState.LevelSize.X / 2, GameState.LevelSize.Y / 2);
        }
    }
}
