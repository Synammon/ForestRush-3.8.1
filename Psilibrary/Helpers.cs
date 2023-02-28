using Microsoft.Xna.Framework;
using Psilibrary.TileEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary
{
    internal class Helpers
    {
        public static Rectangle PointToTile(Point p)
        {
            return new(
                p.X * Engine.TileWidth, 
                p.Y * Engine.TileHeight, 
                Engine.TileWidth, 
                Engine.TileHeight);
        }
    }
}
