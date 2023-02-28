using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.TileEngine
{
    public interface ILayer
    {
        int Width { get; }
        int Height { get; }
        bool Enabled { get; set; }
        bool Visible { get; set; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, Camera camera);
    }
}
