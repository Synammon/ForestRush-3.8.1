using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.TileEngine
{
    public static class World
    {
        public static string CurrentMap { get; private set; }
        public static Dictionary<string, TileMap> Maps { get; private set; } = new();
        public static TileMap TileMap 
        { 
            get 
            { 
                if (Maps.ContainsKey(CurrentMap)) 
                    return Maps[CurrentMap]; 
                else 
                    return null; 
            } 
        }
        static World() 
        { 
        }

        public static void ChangeMap(string mapName)
        {
            if (!Maps.ContainsKey(mapName)) return;

            CurrentMap = mapName;
        }

        public static void Load(ContentManager content, Game game)
        {
            TileFrames tileSet = new();
            tileSet.LoadContent(content);

            List<ILayer> layers = new();

            TileLayer groundLayer = new(100, 25, tileSet);

            for (int x = 0; x < 100; x++)
            {
                int y = 24;

                groundLayer.SetTile(x, y, 14);                
            }

            layers.Add(groundLayer);

            List<ILayer> foreground = new();

            TileMap map = new(tileSet, layers, foreground, "Test");

            CollisionLayer collisionLayer = new();

            for (int x = 0; x < 100; x++)
            {

                collisionLayer.Collisions.Add(
                    new(
                        new(x * Engine.TileWidth, 24 * Engine.TileHeight), 
                        new(Engine.TileWidth, Engine.TileHeight)),
                    CollisionType.CollisionDown);
            }

            World.Maps.Add("Test", map);
            World.ChangeMap("Test");
        }
    }
}
