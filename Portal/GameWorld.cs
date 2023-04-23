using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Portal.BuilderPattern;
using Portal.ComponentPattern;
using System.Collections.Generic;
using System.ComponentModel;
using Component = Portal.ComponentPattern.Component;

namespace Portal
{
    public class GameWorld : Game
    {
        #region singleton
        private static GameWorld instance;

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }
        #endregion

        #region fields 
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<GameObject> gameObjects = new List<GameObject>();
        private List<GameObject> destroyGameObjects = new List<GameObject>();
        private List<GameObject> newGameObjects = new List<GameObject>();

        private UI userInterface = UI.Instance;

        public Camera Camera { get; private set; }

        public TileMap Map { get; private set; }

        private GameObject playerObject;
        #endregion

        #region properties
        public static float DeltaTime { get; private set; }

        public List<Collider> Colliders { get; private set; } = new List<Collider>();

        public static Vector2 ScreenSize { get; private set; }

        public static MouseState MouseState { get; private set; }
        #endregion

        private GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 600;

            ScreenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }

        #region methods 
        protected override void Initialize()
        {
            // make tile map 
            string filePath = ".\\..\\..\\..\\TileMapFiles\\TileMapTestLevel.txt";
            Map = new TileMap(filePath);

            // get all tiles set 
            for (int y = 0; y < Map.TileCountY; y++)
            {
                for (int x = 0; x < Map.TileCountX; x++)
                {
                    gameObjects.Add(Map.AddTile(x, y));
                }
            }

            Director playerDirector = new Director(new PlayerBuilder());
            playerObject = playerDirector.Construct();
            gameObjects.Add(playerObject);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Awake();

                Collider collider = (Collider)gameObject.GetComponent<Collider>();
                if (collider != null)
                {
                    Colliders.Add(collider);
                }
            }

            userInterface.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Camera = new Camera(GraphicsDevice.Viewport);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Start();
            }

            userInterface.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            MouseState = Mouse.GetState();

            userInterface.Update(gameTime);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
                Camera.Update(playerObject.Transform.Position);
            }

            base.Update(gameTime);

            Cleanup();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Coral);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack,
                               BlendState.AlphaBlend,
                               samplerState: SamplerState.PointClamp,
                               null, null, null,
                               Camera.Transform);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(_spriteBatch);
            }

            userInterface.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// method for adding gameobjects to a list slated to be added
        /// </summary>
        /// <param name="gameObject"></param>
        public void Instantiate(GameObject gameObject)
        {
            newGameObjects.Add(gameObject);
        }

        /// <summary>
        /// method for adding gameobjects to a list slated for removal
        /// </summary>
        /// <param name="gameObject"></param>
        public void Destroy(GameObject gameObject)
        {
            destroyGameObjects.Add(gameObject);
        }

        /// <summary>
        /// Method for removing and adding gameobjects during runtime, including colliders
        /// </summary>
        private void Cleanup()
        {
            //add to gameobjects 
            for (int i = 0; i < newGameObjects.Count; i++)
            {
                gameObjects.Add(newGameObjects[i]);
                newGameObjects[i].Awake();
                newGameObjects[i].Start();

                Collider collider = (Collider)newGameObjects[i].GetComponent<Collider>();
                if (collider != null)
                {
                    Colliders.Add(collider);
                }
            }

            //remove from gameobjects
            for (int i = 0; i < destroyGameObjects.Count; i++)
            {
                gameObjects.Remove(destroyGameObjects[i]);
                Collider collider = (Collider)destroyGameObjects[i].GetComponent<Collider>();

                // if gameobject is soldier or worker, remove the collision event 
                //if (destroyGameObjects[i].GetComponent<Soldier>() != null)
                //{
                //    Soldier soldier = destroyGameObjects[i].GetComponent<Soldier>() as Soldier;
                //    collider.CollisionEvent.Detach(soldier);
                //}

                if (collider != null)
                {
                    Colliders.Remove(collider);
                }
            }

            //clear the lists
            destroyGameObjects.Clear();
            newGameObjects.Clear();
        }

        /// <summary>
        /// Method for finding an object of a type, so we can reference it
        /// </summary>
        /// <typeparam name="T">The type of component we are searching for</typeparam>
        /// <returns>returns the object of the type we are looking for</returns>
        public Component FindObjectOfType<T>() where T : Component
        {
            foreach (GameObject gameObject in gameObjects)
            {
                Component c = gameObject.GetComponent<T>();

                if (c != null)
                {
                    return c;
                }
            }
            return null;
        }
        #endregion
    }
}