﻿using Microsoft.Xna.Framework;
using PortalGame.ComponentPattern;
using PortalGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Content;
using Portal.BuilderPattern;
using PortalGame.BuilderPattern;
using System.IO;
using System.Diagnostics;

namespace Portal.MenuStates
{
    public class GameState : State
    {
        #region fields 
        private static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> destroyGameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();

        private UI userInterface = UI.Instance;

        // level map fields 
        private GameObject playerObject;
        public static Camera Camera { get; private set; }
        public static Vector2 LevelSize { get; private set; }
        #endregion

        #region properties
        public static float DeltaTime { get; private set; }

        public static List<Collider> Colliders { get; private set; } = new List<Collider>();

        public static Dictionary<BoundingBox, Vector3> BoundingBoxes { get; set; } = new Dictionary<BoundingBox, Vector3>();
        #endregion

        public GameState(ContentManager content, GraphicsDevice graphicsDevice, GameWorld game) : base(content, graphicsDevice, game)
        {
        }

        #region methods 
        public override void LoadContent()
        {
            //set up the level
            AddPlatforms(".\\..\\..\\..\\TileMapFiles\\TileMapTestLevel.txt");

            // add background 
            //Director bgDirector = new Director(new BackgroundBuilder());
            //gameObjects.Add(bgDirector.Construct());

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

            Camera = new Camera(graphicsDevice.Viewport);
            Debug.WriteLine("test camera: " + Camera);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Start();
            }

            userInterface.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            userInterface.Update(gameTime);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);

                // update camera 
                Camera.Update(playerObject.Transform.Position);
            }

            Cleanup();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            graphicsDevice.Clear(Color.DeepSkyBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack,
                               BlendState.AlphaBlend,
                               samplerState: SamplerState.PointClamp,
                               null, null, null,
                               Camera.Transform);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch);
            }

            userInterface.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// method for adding gameobjects to a list slated to be added
        /// </summary>
        /// <param name="gameObject"></param>
        public static void Instantiate(GameObject gameObject)
        {
            newGameObjects.Add(gameObject);
        }

        /// <summary>
        /// method for adding gameobjects to a list slated for removal
        /// </summary>
        /// <param name="gameObject"></param>
        public static void Destroy(GameObject gameObject)
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
        public static Component FindObjectOfType<T>() where T : Component
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

        public static GameObject GetObjectOfType<T>() where T : Component
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

        private void AddPlatforms(string filepath)
        {
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

            // set all tiles 
            for (int i = 0; i <= tileCountX; i++)
            {
                for (int j = 0; j < tileCountY; j++)
                {
                    // s marks the starting position of the player 
                    if (tileIds[new Vector2(i, j)] == "s")
                    {
                        Director playerDirector = new Director(new PlayerBuilder(i, j, tilesize));
                        playerObject = playerDirector.Construct();

                        Debug.WriteLine(playerObject);

                        gameObjects.Add(playerObject);
                    }
                    // e marks the end, or the goal, the spot the player needs to get to 
                    else if (tileIds[new Vector2(i, j)] == "e")
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
        #endregion
    }
}
