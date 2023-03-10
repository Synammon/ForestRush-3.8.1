using ForestRush;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psilibrary.SpriteClasses;
using Psilibrary.TileEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ForestRushShared.GameStates
{
    public class GamePlayState : BaseGameState
    {
        private Player _player;
        private const float Gravity = 256;
        private const float Speed = 128;
        private const float Impulse = -640;
        private float _impulse = 0.0f;
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

            World.Load(Content, Game);
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            World.TileMap.Update(gameTime);

            _motion = new(0, 1);
            _motion.Y *= Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                _motion.X = -1;

                if (_player.CurrentAnimation != "walking")
                {
                    _player.ChangeAnimation("walking");
                    _player.Flip = true;
                }
            }
            else if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                _motion.X = 1;

                if (_player.CurrentAnimation != "walking")
                {
                    _player.ChangeAnimation("walking");
                    _player.Flip = false;
                }
            }

            if (Xin.CheckKeyPressed(Microsoft.Xna.Framework.Input.Keys.Space) && _impulse >= 0)
            {
                _impulse = Impulse;
                _player.ChangeAnimation("jumpstart");
            }

            if (_impulse < 0)
            {
                _motion.Y += _impulse * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _impulse += Gravity * 2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _motion.Y += Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            _motion.X *= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

 
            _player.Sprite.Position += _motion;

            _player.Sprite.LockToMap(
                new Point(
                    World.TileMap.WidthInPixels, 
                    World.TileMap.HeightInPixels));

            Engine.Camera.LockToSprite(_player.Sprite, World.TileMap);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            World.TileMap.DrawBackground(gameTime, SpriteBatch, Engine.Camera);

            SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                Engine.Camera.Transformation);

            _player.Draw(gameTime);

            SpriteBatch.End();

            World.TileMap.DrawForeground(gameTime, SpriteBatch, Engine.Camera);
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
