using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psilibrary.SpriteClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForestRushShared.GameStates
{
    public class GamePlayState : BaseGameState
    {
        private Player _player;
        private const float Gravity = 256;
        private Vector2 _motion;
        public GamePlayState(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            if (_player== null)
            {
                _player = new(Game);
                _player.LoadContent(Content);
            }
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            _motion = new(0, 1);
            _motion.Y *= Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _player.Sprite.Position += _motion;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            base.Draw(gameTime);
            _player.Draw(gameTime);

            SpriteBatch.End();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Show()
        {
            base.Show();

            if (_player == null)
            {
                LoadContent();
            }

        }
    }
}
