using Microsoft.Xna.Framework;
using Portal.ComponentPattern;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Portal
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

            Point position = new Point(x, y);

            Tile tile;

            switch (tileTypes[position])
            {
                case 0:
                    break; 
                case 1:
                    // obstacle - not walkable, collision
                    tile = (Tile)gameObject.AddComponent(new Tile(position, tileSize, tileSize, 1));
                    tile.Walkable = false;
                    break;
                case 2:
                    // grass - walkable, no collision 
                    tile = (Tile)gameObject.AddComponent(new Tile(position, tileSize, tileSize, 2));
                    tile.Walkable = true;
                    break;
            }

            return gameObject;
        }
    }
}
