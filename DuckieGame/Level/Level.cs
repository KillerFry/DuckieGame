using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MapGenerator;

namespace DuckieGame.Level
{
    class Level
    {
        readonly int FLOOR = 0;
        readonly int WALL = 1;

        public int[,] Map { get; private set; }

        private Dictionary<string, Texture2D> Textures { get; set; }

        public Level() { }

        public void CreateMap()
        {
            MapGenerator.Map map = new MapGenerator.Map(128, 128);
            MapGenerator.BSP.BSPGenerator.GenerateMap(128, 128, map);
            Map = map.TheGrid;
            //Map = DrunkCaveGenerator.GenerateMap(width: 80, height: 80, floorPercentage: 45, byHand: false);
        }

        public void UpdateMap(int di)
        {
            Map = DrunkCaveGenerator.UpdateMap(di);
        }

        public void TrimMap()
        {
            Map = DrunkCaveGenerator.TrimMap();
        }

        public void LoadTexture(string textureName, Texture2D texture)
        {
            if (Textures == null)
            {
                Textures = new Dictionary<string, Texture2D>();
            }

            Textures.Add(textureName, texture);
        }

        public Point GetLastCell()
        {
            return DrunkCaveGenerator.lastCell;
        }

        public Point GetRandomMapLocation()
        {
            return DrunkCaveGenerator.GetRandomMapPosition();
        }

        public bool CheckWall(Point position)
        {
            return Map[position.X, position.Y] == WALL;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    spriteBatch.Draw(Map[x, y] == WALL ? Textures["wall"] : Textures["floor"],
                        new Vector2(
                            20 + x * 10,
                            20 + y * 10), Color.White);
                }
            }
        }
    }
}
