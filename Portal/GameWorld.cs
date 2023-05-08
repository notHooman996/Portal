using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Portal.BuilderPattern;
using PortalGame.BuilderPattern;
using PortalGame.ComponentPattern;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Component = PortalGame.ComponentPattern.Component;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;

namespace PortalGame
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

        // level map fields 
        private GameObject playerObject;
        public Camera Camera { get; private set; }
        public Vector2 LevelSize { get; private set; }
        #endregion

        #region properties
        public static float DeltaTime { get; private set; }

        public List<Collider> Colliders { get; private set; } = new List<Collider>();

        public static Vector2 ScreenSize { get; private set; }
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
            AddPlatforms(".\\..\\..\\..\\TileMapFiles\\TileMapTestLevel.txt"); 

            

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

        private void AddPlatforms(string filepath)
        {
            // make tile map 
            //string filePath = filepath;
            //Map = new TileMap(filePath);

            // number of tiles in x and y depends on the size of the file 
            string firstLine = File.ReadLines(filepath).First();
            int tileCountX = firstLine.Length - firstLine.Replace(" ", "").Length; // returns the number of intergers in the first line 
            int tileCountY = File.ReadLines(filepath).Count(); // returns the number of lines in the file 

            // set the sie of the tile used for the platforms 
            int tilesize = 64; 

            // set maps width and height 
            LevelSize = new Vector2(tileCountX * tilesize, tileCountY * tilesize - tilesize);
            //Width = TileCountX * tileSize;
            //Height = TileCountY * tileSize;

            // read the file, to get tiletypes into dictionary 
            Dictionary<Vector2, string> tileIds = new Dictionary<Vector2, string>();
            string[] lines = File.ReadAllLines(filepath);
            string[] type;
            string typeID;
            for (int i = 0; i < lines.Length; i++)
            {
                type = lines[i].Split(" ");

                for (int j = 0; j < type.Length; j++)
                {
                    typeID = type[j];
                    tileIds.Add(new Vector2(j, i), typeID);
                }
            }


            // get all tiles set 
            //for (int y = 0; y < Map.TileCountY; y++)
            //{
            //    for (int x = 0; x < Map.TileCountX; x++)
            //    {
            //        gameObjects.Add(Map.AddTile(x, y));
            //    }
            //}

            for (int i = 0; i <= tileCountX; i++)
            {
                for (int j = 0; j < tileCountY; j++)
                {
                    // s marks the starting position of the player 
                    if(tileIds[new Vector2(i, j)] == "s")
                    {
                        Director playerDirector = new Director(new PlayerBuilder(i, j, tilesize));
                        playerObject = playerDirector.Construct();
                        gameObjects.Add(playerObject);
                    }
                    // e marks the end, or the goal, the spot the player needs to get to 
                    else if(tileIds[new Vector2(i, j)] == "e")
                    {
                        Director endDirector = new Director(new EndBuilder(i, j, tilesize));
                        gameObjects.Add(endDirector.Construct()); 
                    }
                    // any tile with id other than 0, is a platform 
                    else if (tileIds[new Vector2(i, j)] != "0")
                    {
                        int platformId = Int32.Parse(tileIds[new Vector2(i, j)]);
                        Director platformDirector = new Director(new PlatformBuilder(i, j, tilesize, platformId));
                        gameObjects.Add(platformDirector.Construct());
                    }
                }
            }



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
            //MouseState = Mouse.GetState();

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

        public GameObject GetObjectOfType<T>() where T : Component
        {
            foreach (GameObject gameObject in gameObjects)
            {
                Component c = gameObject.GetComponent<T>();

                if (c != null)
                {
                    return gameObject;
                }
            }
            return null;
        }
        #endregion
    }
}