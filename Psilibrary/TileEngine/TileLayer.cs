using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psilibrary.TileEngine
{ 
    public class TileLayer : ILayer
    {
        #region Field Region

        [ContentSerializer]
        public Dictionary<Rectangle, int> Tiles { get; private set; } = new();

        private readonly TileFrames _tileSet = new();

        int width;
        int height;

        Point cameraPoint;
        Point viewPoint;
        Point min;
        Point max;
 
        #endregion

        #region Property Region

        public bool Enabled { get; set; }

        public bool Visible { get; set; }

        [ContentSerializer]
        public int Width
        {
            get { return width; }
            private set { width = value; }
        }

        [ContentSerializer]
        public int Height
        {
            get { return height; }
            private set { height = value; }
        }

        #endregion

        #region Constructor Region

        private TileLayer()
        {
            Enabled = true;
            Visible = true;
        }

        public TileLayer(int width, int height, TileFrames frames)
            : this()
        {
            Tiles = new();

            foreach (TileFrame frame in frames.Frames)
            {
                _tileSet.Frames.Add(frame);
            }

            this.width = width;
            this.height = height;
        }

        #endregion

        #region Method Region

        public int GetTile(int x, int y)
        {
            if (x < 0 || y < 0)
                return -1;

            if (x >= width || y >= height)
                return -1;
            Rectangle tile = new(x, y, Engine.TileWidth, Engine.TileHeight);

            if (Tiles.ContainsKey(tile))
                return Tiles[tile];

            return -1;
        }

        public void SetTile(int x, int y, int tile)
        {
            if (x < 0 || y < 0)
                return;

            if (x >= width || y >= height)
                return;

            Point location = new(x, y);

            if (Tiles.ContainsKey(Helpers.PointToTile(location)))
            {
                Tiles[Helpers.PointToTile(location)] = tile;
            }
            else
            {
                Tiles.Add(Helpers.PointToTile(location), tile);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            if (!Visible)
                return;

            cameraPoint = Engine.VectorToCell(camera.Position);
            viewPoint = Engine.VectorToCell(
                new Vector2(
                    (camera.Position.X + Engine.ViewportRectangle.Width),
                    (camera.Position.Y + Engine.ViewportRectangle.Height)));

            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);
            max.X = Math.Min(viewPoint.X + 1, Width);
            max.Y = Math.Min(viewPoint.Y + 1, Height);

            foreach (Rectangle r in Tiles.Keys.Where(
                x => (x.X / Engine.TileWidth) >= min.X && (x.X / Engine.TileWidth) <= max.X && (x.Y / Engine.TileHeight) >= min.Y && (x.Y / Engine.TileHeight) <= max.Y)) 
            {
                spriteBatch.Draw(
                    _tileSet.Frames[Tiles[r]].Texture,
                    r,
                    null,
                    Color.White);
            }
        }

        #endregion
    }
}
