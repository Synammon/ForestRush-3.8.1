using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Psilibrary.TileEngine
{
    public class TileMap
    {
        #region Field Region

        string mapName;

        public List<ILayer> Background { get; private set; } = new();
        public List<ILayer> Foreground { get; private set; } = new();

        int mapWidth;
        int mapHeight;

        TileFrames tileSet;

        #endregion

        #region Property Region

        [ContentSerializer(Optional = true)]
        public string MapName
        {
            get { return mapName; }
            private set { mapName = value; }
        }

        [ContentSerializer]
        public TileFrames TileSet
        {
            get { return tileSet; }
            set { tileSet = value; }
        }

        [ContentSerializer]
        public int MapWidth
        {
            get { return mapWidth; }
            private set { mapWidth = value; }
        }

        [ContentSerializer]
        public int MapHeight
        {
            get { return mapHeight; }
            private set { mapHeight = value; }
        }

        public int WidthInPixels
        {
            get { return mapWidth * Engine.TileWidth; }
        }

        public int HeightInPixels
        {
            get { return mapHeight * Engine.TileHeight; }
        }

        #endregion

        #region Constructor Region

        private TileMap()
        {
        }

        private TileMap(TileFrames tileSet, string mapName)
            : this()
        {
            this.tileSet = tileSet;
            this.mapName = mapName;
        }

        public TileMap(
            TileFrames tileSet,
            List<ILayer> backgroundLayers,
            List<ILayer> foregroundLayers,
            string mapName)
            : this(tileSet, mapName)
        {
            int maxHeight = 0;
            int maxWidth = 0;

            foreach (var layer in backgroundLayers)
            {
                if (layer.Width > maxWidth) maxWidth = layer.Width;
                if (layer.Height > maxHeight) maxHeight = layer.Height;

                Background.Add(layer);
            }

            foreach (var layer in foregroundLayers)
            {
                if (layer.Width > maxWidth) maxWidth = layer.Width;
                if (layer.Height > maxHeight) maxHeight = layer.Height;

                Foreground.Add(layer);
            }

            mapWidth = maxWidth;
            mapHeight = maxHeight;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            foreach (ILayer layer in Background)
            { 
                if (layer.Enabled)
                {
                    layer.Update(gameTime);
                }
            }

            foreach (ILayer layer in Foreground)
            {
                if (layer.Enabled)
                {
                    layer.Update(gameTime);
                }
            }
        }

        public void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, Camera camera, bool debug = false)
        {
            if (WidthInPixels >= Engine.TargetWidth || debug)
            {
                spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    camera.Transformation);
            }
            else
            {
                Matrix m = Matrix.CreateTranslation(
                    new Vector3((Engine.TargetWidth) / 2, (Engine.TargetHeight - HeightInPixels) / 2, 0));
                spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    m);
            }

            foreach (ILayer layer in Background)
            {
                if (layer.Visible)
                {
                    layer.Draw(spriteBatch, camera);
                }
            }

            spriteBatch.End();
        }

        public void DrawForeground(GameTime gameTime, SpriteBatch spriteBatch, Camera camera, bool debug = false)
        {
            if (WidthInPixels >= Engine.TargetWidth || debug)
            {
                spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    camera.Transformation);
            }
            else
            {
                Matrix m = Matrix.CreateTranslation(
                    new Vector3((Engine.TargetWidth) / 2, (Engine.TargetHeight - HeightInPixels) / 2, 0));
                spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    m);
            }

            foreach (ILayer layer in Foreground)
            {
                if (layer.Visible)
                {
                    layer.Draw(spriteBatch, camera);
                }
            }

            spriteBatch.End();
        }

        #endregion
    }
}
