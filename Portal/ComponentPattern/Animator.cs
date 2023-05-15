using PortalGame.ComponentPattern;
using PortalGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Portal.MenuStates;

namespace Portal.ComponentPattern
{
    public class Animator : Component
    {
        #region fields
        private float timeElapsed;
        private SpriteRenderer spriteRenderer;
        //dictionaries for name and animation
        private Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
        private Animation currentAnimation;
        #endregion

        public int CurrentIndex { get; private set; }

        #region methods
        public override void Start()
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
        }

        public override void Update(GameTime gameTime)
        {
            timeElapsed += GameState.DeltaTime;

            CurrentIndex = (int)(timeElapsed * currentAnimation.FPS);

            if (CurrentIndex > currentAnimation.Sprites.Length - 1)
            {
                timeElapsed = 0;
                CurrentIndex = 0;
            }

            spriteRenderer.Sprite = currentAnimation.Sprites[CurrentIndex];
        }

        public void AddAnimation(Animation animation)
        {
            animations.Add(animation.Name, animation);

            if (currentAnimation == null)
            {
                currentAnimation = animation;
            }
        }

        public void PlayAnimation(string animationName)
        {
            if (animationName != currentAnimation.Name)
            {
                currentAnimation = animations[animationName];
                timeElapsed = 0;
                CurrentIndex = 0;
            }
        }

        #endregion
    }

    public class Animation
    {
        #region properties
        public float FPS { get; private set; }

        public string Name { get; private set; }

        public Texture2D[] Sprites { get; private set; }
        #endregion

        #region constructors
        public Animation(float fps, string name, Texture2D[] sprites)
        {
            FPS = fps;
            Name = name;
            Sprites = sprites;
        }
        #endregion
    }
}
