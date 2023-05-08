using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Portal.ComponentPattern;
using PortalGame.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalGame.BuilderPattern
{
    public class PlayerBuilder : IBuilder
    {
        private GameObject gameObject;
        private Vector2 position;

        // set animation sprites 
        private string[] idle = new string[] { "Player\\Idle\\idle" };
        private string[] leftWalk = new string[] { "Player\\WalkingLeft\\left1", "Player\\WalkingLeft\\left2", "Player\\WalkingLeft\\left3" };
        private string[] rightWalk = new string[] { "Player\\WalkingRight\\right1", "Player\\WalkingRight\\right2", "Player\\WalkingRight\\right3" };

        public PlayerBuilder(int x, int y, int tileSize)
        {
            position = new Vector2(x * tileSize, y * tileSize); 
        }

        /// <summary>
        /// creates a new gameobject
        /// </summary>
        public void BuildGameObject()
        {
            gameObject = new GameObject();

            BuildComponents();
        }

        /// <summary>
        /// adds components to the gameobject
        /// </summary>
        private void BuildComponents()
        {
            // add components 
            Player player = (Player)gameObject.AddComponent(new Player(position));
            gameObject.AddComponent(new SpriteRenderer());

            // attach collision detection
            Collider collider = (Collider)gameObject.AddComponent(new Collider());
            collider.CollisionEvent.Attach(player);
            collider.TopCollisionEvent.Attach(player); 
            collider.BottomCollisionEvent.Attach(player); 
            collider.RightCollisionEvent.Attach(player); 
            collider.LeftCollisionEvent.Attach(player);

            // animation
            Animator animator = gameObject.AddComponent(new Animator()) as Animator;
            animator.AddAnimation(BuildAnimation("Idle", idle));
            animator.AddAnimation(BuildAnimation("WalkLeft", leftWalk));
            animator.AddAnimation(BuildAnimation("WalkRight", rightWalk));
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

        /// <summary>
        /// returns the object that was built
        /// </summary>
        /// <returns>gameobject which was just built out of components</returns>
        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
