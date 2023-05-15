using PortalGame.ComponentPattern.Portals;
using PortalGame.ComponentPattern;
using PortalGame;
using PortalGame.CreationalPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Portal.ComponentPattern;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using Portal.MenuStates;

namespace Portal.CreationalPattern
{
    public class RedPortalFactory : Factory
    {
        #region singleton 
        private static RedPortalFactory instance;

        public static RedPortalFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RedPortalFactory();
                }
                return instance;
            }
        }
        #endregion

        private GameObject topPrototype;
        private GameObject bottomPrototype;
        private GameObject leftPrototype;
        private GameObject rightPrototype;

        private RedPortalFactory()
        {
            CreateTopPrototype();
            CreateBottomPrototype();
            CreateLeftPrototype();
            CreateRightPrototype();
        }

        #region prototypes 
        private void CreateTopPrototype()
        {
            topPrototype = new GameObject();

            SpriteRenderer spriteRenderer = (SpriteRenderer)topPrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Portal\\Pink\\Top\\pink1");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;

            topPrototype.AddComponent(new RedPortal());
            topPrototype.AddComponent(new Collider());

            // add animation
            Animator animator = topPrototype.AddComponent(new Animator()) as Animator;
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Pink\\Top\\pink{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Top", sprites));
        }

        private void CreateBottomPrototype()
        {
            bottomPrototype = new GameObject();

            SpriteRenderer spriteRenderer = (SpriteRenderer)bottomPrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Portal\\Pink\\Bottom\\pink1");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;

            bottomPrototype.AddComponent(new RedPortal());
            bottomPrototype.AddComponent(new Collider());

            // add animation
            Animator animator = bottomPrototype.AddComponent(new Animator()) as Animator;
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Pink\\Bottom\\pink{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Bottom", sprites));
        }

        private void CreateLeftPrototype()
        {
            leftPrototype = new GameObject();

            SpriteRenderer spriteRenderer = (SpriteRenderer)leftPrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Portal\\Pink\\Left\\pink1");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;

            leftPrototype.AddComponent(new RedPortal());
            leftPrototype.AddComponent(new Collider());

            // add animation
            Animator animator = leftPrototype.AddComponent(new Animator()) as Animator;
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Pink\\Left\\pink{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Left", sprites));
        }

        private void CreateRightPrototype()
        {
            rightPrototype = new GameObject();

            SpriteRenderer spriteRenderer = (SpriteRenderer)rightPrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Portal\\Pink\\Right\\pink1");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;

            rightPrototype.AddComponent(new RedPortal());
            rightPrototype.AddComponent(new Collider());

            // add animation
            Animator animator = rightPrototype.AddComponent(new Animator()) as Animator;
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Pink\\Right\\pink{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Right", sprites));
        }
        #endregion

        /// <summary>
        /// Method that builds an animation using an array of sprite names
        /// </summary>
        /// <param name="animationName">A string name for the animation, used to tell animations apart</param>
        /// <param name="spriteNames">A string array for the sprites used in the animation</param>
        /// <returns></returns>
        private Animation BuildAnimation(string animationName, string[] spriteNames)
        {
            Texture2D[] sprites = new Texture2D[spriteNames.Length];

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = GameWorld.Instance.Content.Load<Texture2D>(spriteNames[i]);
            }

            Animation animation = new Animation(7.5f, animationName, sprites);

            return animation;
        }

        public override GameObject Create(Enum type)
        {
            // remove old portal 
            // find the red portal object 
            GameObject redPortalObject = GameState.GetObjectOfType<RedPortal>();
            // destroy red portal 
            if (redPortalObject != null)
            {
                GameState.Destroy(redPortalObject);
            }

            GameObject gameObject = new GameObject();
            Collider collider;
            RedPortal redPortal; 

            switch (type)
            {
                case Side.Top:
                    gameObject = (GameObject)topPrototype.Clone();
                    gameObject.Tag = PortalType.Red.ToString();

                    // attach collision event 
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal);

                    redPortal.PlayerDisplacement = new Vector2(0, -1);
                    redPortal.AnimationName = "Top"; 
                    break;
                case Side.Bottom:
                    gameObject = (GameObject)bottomPrototype.Clone();
                    gameObject.Tag = PortalType.Red.ToString();

                    // attach collision event 
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal);

                    redPortal.PlayerDisplacement = new Vector2(0, 1);
                    redPortal.AnimationName = "Bottom";
                    break;
                case Side.Left:
                    gameObject = (GameObject)leftPrototype.Clone();
                    gameObject.Tag = PortalType.Red.ToString();

                    // attach collision event 
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal);

                    redPortal.PlayerDisplacement = new Vector2(-1, 0);
                    redPortal.AnimationName = "Left";
                    break;
                case Side.Right:
                    gameObject = (GameObject)rightPrototype.Clone();
                    gameObject.Tag = PortalType.Red.ToString();

                    // attach collision event 
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal);

                    redPortal.PlayerDisplacement = new Vector2(1, 0);
                    redPortal.AnimationName = "Right";
                    break;
            }

            return gameObject;
        }
    }
}
