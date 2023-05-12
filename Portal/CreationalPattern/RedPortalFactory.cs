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

namespace Portal.CreationalPattern
{
    public class RedPortalFactory : Factory
    {
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

        private Animator animator;

        private GameObject redPrototype; 
        private GameObject topPrototype;
        private GameObject bottomPrototype;
        private GameObject leftPrototype;
        private GameObject rightPrototype;

        private RedPortalFactory()
        {
            CreateRedPrototype(); 
            CreateTopPrototype();
            CreateBottomPrototype();
            CreateLeftPrototype();
            CreateRightPrototype();
        }

        private void CreateRedPrototype()
        {
            redPrototype = new GameObject();
            SpriteRenderer spriteRenderer = (SpriteRenderer)redPrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Portal\\redportal_back");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;
            redPrototype.AddComponent(new RedPortal());
            redPrototype.AddComponent(new Collider());
            animator = redPrototype.AddComponent(new Animator()) as Animator;
            animator.AddAnimation(BuildAnimation("Default", new string[] { "Portal\\Purple\\purple1" }));
        }

        private void CreateTopPrototype()
        {
            topPrototype = (GameObject)redPrototype.Clone();

            // add animation
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Purple\\purple{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Top", sprites));
        }

        private void CreateBottomPrototype()
        {
            bottomPrototype = (GameObject)redPrototype.Clone();

            // add animation
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Purple\\purple{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Bottom", sprites));
        }

        private void CreateLeftPrototype()
        {
            leftPrototype = (GameObject)redPrototype.Clone();

            // add animation
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Purple\\purple{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Left", sprites));
        }

        private void CreateRightPrototype()
        {
            rightPrototype = (GameObject)redPrototype.Clone();

            // add animation
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Purple\\purple{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Right", sprites));
        }

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
            GameObject redPortalObject = GameWorld.Instance.GetObjectOfType<RedPortal>();
            // destroy red portal 
            if (redPortalObject != null)
            {
                GameWorld.Instance.Destroy(redPortalObject);
            }

            GameObject gameObject = new GameObject();
            Collider collider;
            RedPortal redPortal; 

            switch (type)
            {
                case Side.Top:
                    gameObject = (GameObject)topPrototype.Clone();
                    gameObject.Tag = PortalType.Red.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal);

                    redPortal.PlayerDisplacement = new Vector2(0, -1);
                    redPortal.AnimationName = "Top"; 
                    break;
                case Side.Bottom:
                    gameObject = (GameObject)bottomPrototype.Clone();
                    gameObject.Tag = PortalType.Red.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal);

                    redPortal.PlayerDisplacement = new Vector2(0, 1);
                    redPortal.AnimationName = "Bottom";
                    break;
                case Side.Left:
                    gameObject = (GameObject)leftPrototype.Clone();
                    gameObject.Tag = PortalType.Red.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    redPortal = gameObject.GetComponent<RedPortal>() as RedPortal;
                    collider.CollisionEvent.Attach(redPortal);

                    redPortal.PlayerDisplacement = new Vector2(-1, 0);
                    redPortal.AnimationName = "Left";
                    break;
                case Side.Right:
                    gameObject = (GameObject)rightPrototype.Clone();
                    gameObject.Tag = PortalType.Red.ToString();
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
