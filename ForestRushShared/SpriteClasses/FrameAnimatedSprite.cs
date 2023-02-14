using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.SpriteClasses
{
    public class FrameAnimatedSprite
    {
        private readonly List<Texture2D> _frames = new();
        protected int _currentFrame;
        private TimeSpan _frameTimer;
        private readonly TimeSpan _frameLength;

        public int Frames { get { return _frames.Count; } }
        public int CurrentFrame { get { return _currentFrame; } }
        public Texture2D CurrentTexture { get { return _frames[_currentFrame]; } }
        public Vector2 Position { get; set; }

        public int Width { get { return _frames[0].Width; } }
        public int Height { get { return _frames[0].Height; } }

        public Rectangle Bounds
        {
            get { return new((int)Position.X, (int)Position.Y, Width, Height); }
        }

        public FrameAnimatedSprite(List<Texture2D> frames)
        {
            foreach (var frame in frames)
            {
                _frames.Add(frame);
            }

            _frameLength = TimeSpan.FromSeconds(1.0 / 20.0);
        }

        public void Reset()
        {
            _currentFrame = 0;
            _frameTimer = TimeSpan.Zero;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects flip)
        {
            Rectangle location = new(new((int)position.X, (int)position.Y), new(128, 128));
            spriteBatch.Draw(_frames[_currentFrame], location, null, Color.White, 0f, Vector2.Zero, flip, 1f);
        }

        public void Update(GameTime gameTime)
        {
            _frameTimer += gameTime.ElapsedGameTime;

            if (_frameTimer >= _frameLength)
            {
                _frameTimer = TimeSpan.Zero;
                _currentFrame++;
            }
        }
    }
}
