using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.TileEngine
{
    public enum CollisionType 
    { 
        CollisionUp, 
        CollisionDown, 
        CollisionRight, 
        CollisionLeft, 
        CollisionAll, 
        Damage,
        Death
    }

    public class CollisionLayer
    {
        [ContentSerializer]
        public Dictionary<Rectangle, CollisionType> Collisions { get; private set; } = new();

        public CollisionLayer() 
        { 
        }
    }
}
