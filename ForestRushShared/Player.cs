using ForestRushShared.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Psilibrary.SpriteClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ForestRushShared
{
    public class TransitionEventArgs : EventArgs
    {
        public string Animation;
    }

    public class Player : DrawableGameComponent
    {
        public event EventHandler<TransitionEventArgs> Transition;
        public Dictionary<string, FrameAnimatedSprite> Sprites { get; private set; } = new();
        protected string _currentAnimation;
        protected readonly SpriteBatch _spriteBatch;

        public bool Flip { get; set; }

        public FrameAnimatedSprite Sprite
        {
            get
            {
                if (Sprites.ContainsKey(_currentAnimation))
                    return Sprites[_currentAnimation];
                else
                    return null;
            }
        }

        public string CurrentAnimation { get { return _currentAnimation; } }

        public Player(Game game) : base(game)
        {
            _spriteBatch = Game.Services.GetService<SpriteBatch>();
            _currentAnimation = "idle";
        }

        public virtual void LoadContent(ContentManager Content)
        {
            Sprites.Clear();

            try
            {
                string folder = string.Format("{0}/PlayerSprite/", Content.RootDirectory);
                foreach (var f in Directory.GetDirectories(folder))
                {
                    string root = f.Replace("Content/", "");
                    string animation = root.Replace("PlayerSprite/", "");

                    List<Texture2D> list = new();
                    foreach (var r in Directory.GetFiles(f))
                    {
                        string path = Path.GetFileNameWithoutExtension(r);

                        StringBuilder build = new StringBuilder()
                            .Append(root)
                            .Append('/')
                            .Append(path);

                        list.Add(Content.Load<Texture2D>(build.ToString()));
                    }

                    FrameAnimatedSprite sprite = new(list);
                    Sprites.Add(animation.ToLower(), sprite);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Sprites.Count == 0)
            {
                LoadContent(Game.Content);
                return;
            }

            Sprites[_currentAnimation].Update(gameTime);

            if (Sprites[_currentAnimation].CurrentFrame >= Sprites[_currentAnimation].Frames && _currentAnimation.ToLower() != "walk" && _currentAnimation.ToLower() != "run")
            {
                OnTransition("idle");
                ChangeAnimation("idle");
            }
            else if (Sprites[_currentAnimation].CurrentFrame >= Sprites[_currentAnimation].Frames)
            {
                OnTransition(_currentAnimation);
                ChangeAnimation(_currentAnimation);
            }

            base.Update(gameTime);
        }

        private void OnTransition(string animation)
        {
            Transition?.Invoke(this, new() { Animation = animation });
        }
        public override void Draw(GameTime gameTime)
        {
            if (Sprites.Count == 0) return;

            if (Flip)
                Sprites[_currentAnimation].Draw(_spriteBatch, Sprite.Position, SpriteEffects.FlipHorizontally);
            else
                Sprites[_currentAnimation].Draw(_spriteBatch, Sprite.Position, SpriteEffects.None);

            base.Draw(gameTime);
        }

        public void ChangeAnimation(string animation)
        {
            if (Sprites.ContainsKey(animation))
            {
                Sprites[animation].Position = Sprites[_currentAnimation].Position;
                _currentAnimation = animation;
                Sprites[_currentAnimation].Reset();
            }
        }

        public void Reset()
        {
            if (Sprite != null)
            {
                Sprite.Position = Vector2.Zero;
            }
        }
    }
}
