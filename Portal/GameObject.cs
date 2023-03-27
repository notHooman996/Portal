using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Portal.ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal
{
    public class GameObject
    {
        #region fields
        private List<Component> components = new List<Component>();
        #endregion

        #region properties
        /// <summary>
        /// property for giving the object a transform, which includes position and rotation
        /// </summary>
        public Transform Transform { get; set; } = new Transform();

        /// <summary>
        /// Gives the object a tag so we can use it to identify an object
        /// </summary>
        public string Tag { get; set; }
        #endregion

        #region methods
        /// <summary>
        /// method for adding a component to the gameobject
        /// </summary>
        /// <param name="component">the component we wish to add</param>
        /// <returns>returns the chosen component</returns>
        public Component AddComponent(Component component)
        {
            component.GameObject = this;
            components.Add(component);
            return component;
        }

        /// <summary>
        /// Method for getting the component so we can access the component
        /// </summary>
        /// <typeparam name="T">the type of component we want</typeparam>
        /// <returns>the requested component</returns>
        public Component GetComponent<T>() where T : Component
        {
            return components.Find(x => x.GetType() == typeof(T));
        }

        /// <summary>
        /// calls awake on all components
        /// </summary>
        public void Awake()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Awake();
            }
        }

        /// <summary>
        /// calls start on all the components
        /// </summary>
        public void Start()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Start();
            }
        }

        /// <summary>
        /// calls update on all the components
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Update(gameTime);
            }
        }

        /// <summary>
        /// calls draw on all the components
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Draw(spriteBatch);
            }
        }

        /// <summary>
        /// method for cloning and object and all its components
        /// </summary>
        /// <returns>returns the cloned object</returns>
        public object Clone()
        {
            GameObject gameObject = new GameObject();

            foreach (Component component in components)
            {
                gameObject.AddComponent(component.Clone() as Component);
            }
            return gameObject;
        }
        #endregion
    }
}
