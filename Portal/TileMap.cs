using Microsoft.Xna.Framework;
using PortalGame.ComponentPattern;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PortalGame
{
    public class TileMap
    {
        private Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

        private int tileSize = 100;
        public int TileCountX { get; private set; }
        public int TileCountY { get; private set; }

        // field variable for the file 
        private string filePath;

        // the file 
        private Dictionary<Point, int> tileTypes = new Dictionary<Point, int>();

        public int Width { get; private set; }
        public int Height { get; private set; }

        public TileMap(string filePath)
        {
            // gets file name through parameter to base the map out of 
            this.filePath = filePath;

            // number of tiles in x and y depends on the size of the file 
            string firstLine = File.ReadLines(filePath).First();
            TileCountX = firstLine.Length - firstLine.Replace(" ", "").Length; // returns the number of intergers in the first line 

            TileCountY = File.ReadLines(filePath).Count(); // returns the number of lines in the file 

            // set maps width and height 
            Width = TileCountX * tileSize;
            Height = TileCountY * tileSize;

            // read the file, to get tiletypes into dictionary 
            ReadFile();
        }

        private void ReadFile()
        {
            string[] lines = File.ReadAllLines(filePath);

            string[] type;

            int typeID;

            for (int i = 0; i < lines.Length; i++)
            {
                type = lines[i].Split(" ");

                for (int j = 0; j < type.Length; j++)
                {
                    typeID = Int32.Parse(type[j]);

                    tileTypes.Add(new Point(j, i), typeID);
                }
            }
        }

        public GameObject AddTile(int x, int y)
        {
            GameObject gameObject = new GameObject();
            gameObject.AddComponent(new SpriteRenderer());
            gameObject.AddComponent(new Collider());

            Point position = new Point(x, y);

            CollisionTile tile;
            //Collider collider; 

            switch (tileTypes[position])
            {
                case 0:
                    gameObject.AddComponent(new Tile(position, tileSize, tileSize, 1));
                    break; 
                case 1:
                    tile = (CollisionTile)gameObject.AddComponent(new CollisionTile(position, tileSize, tileSize, 1));
                    CheckNeighbour(tile);
                    //gameObject.AddComponent(new SpriteRenderer());
                    //collider = (Collider)gameObject.AddComponent(new Collider());
                    //collider.CollisionEvent.Attach(tile);
                    break;
                case 2:
                    tile = (CollisionTile)gameObject.AddComponent(new CollisionTile(position, tileSize, tileSize, 2));
                    CheckNeighbour(tile);
                    //gameObject.AddComponent(new SpriteRenderer());
                    //collider = (Collider)gameObject.AddComponent(new Collider());
                    //collider.CollisionEvent.Attach(tile);
                    break;
            }

            return gameObject;
        }

        private void CheckNeighbour(CollisionTile tile)
        {
            Point topPoint = new Point(tile.Position.X, tile.Position.Y - 1); 
            Point bottomPoint = new Point(tile.Position.X, tile.Position.Y + 1); 
            Point leftPoint = new Point(tile.Position.X - 1, tile.Position.Y); 
            Point rightPoint = new Point(tile.Position.X + 1, tile.Position.Y); 

            // check top 
            if (tileTypes.ContainsKey(topPoint) && tileTypes[topPoint] != 0)
            {
                tile.HasTop = true; 
            }
            // check bottom 
            if (tileTypes.ContainsKey(bottomPoint) && tileTypes[bottomPoint] != 0)
            {
                tile.HasBottom = true; 
            }
            // check left 
            if (tileTypes.ContainsKey(leftPoint) && tileTypes[leftPoint] != 0)
            {
                tile.HasLeft = true; 
            }
            // check right 
            if (tileTypes.ContainsKey(rightPoint) && tileTypes[rightPoint] != 0)
            {
                tile.HasRight = true; 
            }
        }
    }
}
