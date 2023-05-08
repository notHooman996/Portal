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

namespace Portal.CreationalPattern
{
    public class BluePortalFactory : Factory
    {
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

        private Animator animator; 

        private GameObject bluePrototype;
        private GameObject topPrototype;
        private GameObject bottomPrototype;
        private GameObject leftPrototype;
        private GameObject rightPrototype;

        private BluePortalFactory()
        {
            CreateBluePrototype();
            CreateTopPrototype();
            CreateBottomPrototype();
            CreateLeftPrototype();
            CreateRightPrototype();
        }

        private void CreateBluePrototype()
        {
            bluePrototype = new GameObject();
            SpriteRenderer spriteRenderer = (SpriteRenderer)bluePrototype.AddComponent(new SpriteRenderer());
            spriteRenderer.SetSprite("Portal\\blueportal_back");
            spriteRenderer.LayerDepth = 0.9f;
            spriteRenderer.Scale = 1f;
            bluePrototype.AddComponent(new BluePortal());
            bluePrototype.AddComponent(new Collider());
            animator = bluePrototype.AddComponent(new Animator()) as Animator;
            animator.AddAnimation(BuildAnimation("Default", new string[] { "Portal\\Green\\green1" }));
        }

        private void CreateTopPrototype()
        {
            topPrototype = (GameObject)bluePrototype.Clone();

            // add animation
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Green\\green{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Top", sprites));
        }

        private void CreateBottomPrototype()
        {
            bottomPrototype = (GameObject)bluePrototype.Clone();

            // add animation
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Green\\green{i + 1}";
            }
            animator.AddAnimation(BuildAnimation("Bottom", sprites));
        }

        private void CreateLeftPrototype()
        {
            leftPrototype = (GameObject)bluePrototype.Clone();

            // add animation
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Green\\green{i+1}";
            }
            animator.AddAnimation(BuildAnimation("Left", sprites));
        }

        private void CreateRightPrototype()
        {
            rightPrototype = (GameObject)bluePrototype.Clone();

            // add animation
            string[] sprites = new string[8];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = $"Portal\\Green\\green{i + 1}";
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
            // find the blue portal object 
            GameObject bluePortalObject = GameWorld.Instance.GetObjectOfType<BluePortal>();
            // destroy blue portal 
            if (bluePortalObject != null)
            {
                GameWorld.Instance.Destroy(bluePortalObject);
            }

            GameObject gameObject = new GameObject();
            Collider collider;
            BluePortal bluePortal;

            switch (type)
            {
                case Side.Top:
                    gameObject = (GameObject)topPrototype.Clone();
                    gameObject.Tag = BeamType.Blue.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);

                    bluePortal.PlayerDisplacement = new Vector2(0, -1);
                    bluePortal.AnimationName = "Top";
                    break;
                case Side.Bottom:
                    gameObject = (GameObject)bottomPrototype.Clone();
                    gameObject.Tag = BeamType.Blue.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);

                    bluePortal.PlayerDisplacement = new Vector2(0, 1);
                    bluePortal.AnimationName = "Bottom";
                    break;
                case Side.Left:
                    gameObject = (GameObject)leftPrototype.Clone();
                    gameObject.Tag = BeamType.Blue.ToString();
                    collider = gameObject.GetComponent<Collider>() as Collider;
                    bluePortal = gameObject.GetComponent<BluePortal>() as BluePortal;
                    collider.CollisionEvent.Attach(bluePortal);

                    bluePortal.PlayerDisplacement = new Vector2(-1, 0);
                    bluePortal.AnimationName = "Left";
                    break;
                case Side.Right:
                    gameObject = (GameObject)rightPrototype.Clone();
                    gameObject.Tag = BeamType.Blue.ToString();
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
