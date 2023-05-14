using PortalGame.ComponentPattern.Portals;
using PortalGame.ComponentPattern;
using PortalGame.CreationalPattern;
using PortalGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Portal.ComponentPattern;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Portal.CreationalPattern
{
    public class BluePortalFactory : Factory
    {
        #region singleton
        private static BluePortalFactory instance;

        public static BluePortalFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BluePortalFactory();
                }
                return instance;
            }
        }
        #endregion

        private GameObject topPrototype;
        private GameObject bottomPrototype;
        private GameObject leftPrototype;
        private GameObject rightPrototype;

        private BluePortalFactory()
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
            spriteRenderer.SetSprite("Portal\\Green\\Top\\green1"); 
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;

            topPrototype.AddComponent(new BluePortal());
            topPrototype.AddComponent(new Collider());

            // add animation
            Animator animator = topPrototype.AddComponent(new Animator()) as Animator;
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Green\\Top\\green{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Top", sprites));
        }

        private void CreateBottomPrototype()
        {
            bottomPrototype = new GameObject();

            SpriteRenderer spriteRenderer = (SpriteRenderer)bottomPrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Portal\\Green\\Bottom\\green1");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;

            bottomPrototype.AddComponent(new BluePortal());
            bottomPrototype.AddComponent(new Collider());

            // add animation
            Animator animator = bottomPrototype.AddComponent(new Animator()) as Animator;
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Green\\Bottom\\green{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Bottom", sprites));
        }

        private void CreateLeftPrototype()
        {
            leftPrototype = new GameObject();

            SpriteRenderer spriteRenderer = (SpriteRenderer)leftPrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Portal\\Green\\Left\\green1");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;

            leftPrototype.AddComponent(new BluePortal());
            leftPrototype.AddComponent(new Collider());

            // add animation
            Animator animator = leftPrototype.AddComponent(new Animator()) as Animator;
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Green\\Left\\green{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Left", sprites));
        }

        private void CreateRightPrototype()
        {
            rightPrototype = new GameObject();

            SpriteRenderer spriteRenderer = (SpriteRenderer)rightPrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Portal\\Green\\Right\\green1");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;

            rightPrototype.AddComponent(new BluePortal());
            rightPrototype.AddComponent(new Collider());

            // add animation
            Animator animator = rightPrototype.AddComponent(new Animator()) as Animator;
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Green\\Right\\green{i + 1}";
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
            // find the blue portal object 
            GameObject bluePortalObject = GameWorld.Instance.GetObjectOfType<BluePortal>();
            // destroy blue portal 
            if (bluePortalObject != null)
            {
                GameWorld.Instance.Destroy(bluePortalObject);
            }

            // then create new portal 
            GameObject gameObject = new GameObject();
            Collider collider;
            BluePortal bluePortal;

            switch (type)
            {
                case Side.Top:
                    gameObject = (GameObject)topPrototype.Clone();
                    gameObject.Tag = PortalType.Blue.ToString();

                    // attach collision event 
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);

                    bluePortal.PlayerDisplacement = new Vector2(0, -1);
                    bluePortal.AnimationName = "Top";
                    break;
                case Side.Bottom:
                    gameObject = (GameObject)bottomPrototype.Clone();
                    gameObject.Tag = PortalType.Blue.ToString();

                    // attach collision event 
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);

                    bluePortal.PlayerDisplacement = new Vector2(0, 1);
                    bluePortal.AnimationName = "Bottom";
                    break;
                case Side.Left:
                    gameObject = (GameObject)leftPrototype.Clone();
                    gameObject.Tag = PortalType.Blue.ToString();

                    // attach collision event 
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);

                    bluePortal.PlayerDisplacement = new Vector2(-1, 0);
                    bluePortal.AnimationName = "Left";
                    break;
                case Side.Right:
                    gameObject = (GameObject)rightPrototype.Clone();
                    gameObject.Tag = PortalType.Blue.ToString();

                    // attach collision event 
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);

                    bluePortal.PlayerDisplacement = new Vector2(1, 0);
                    bluePortal.AnimationName = "Right";
                    break;
            }

            return gameObject;
        }
    }
}
