using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForestRushShared
{
    public abstract partial class BaseGameState : DrawableGameComponent
    {
        #region Field Region

        protected ContentManager Content;
        protected readonly List<GameComponent> childComponents;

        #endregion

        #region Property Region

        public List<GameComponent> Components
        {
            get { return childComponents; }
        }

        public BaseGameState Tag => this;

        public SpriteBatch SpriteBatch
        {
            get; private set;
        }

        protected RenderTarget2D RenderTarget
        {
            get; private set;
        }

        #endregion

        #region Constructor Region

        public BaseGameState(Game game)
            : base(game)
        {
            childComponents = new List<GameComponent>();
            Content = Game.Content;
        }

        #endregion

        #region Method Region

        protected override void LoadContent()
        {
            SpriteBatch = Game.Services.GetService<SpriteBatch>();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent component in childComponents.Where(x => x.Enabled))
            {
                component.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (DrawableGameComponent component in childComponents.Where(
                x => x is DrawableGameComponent &&
                (x as DrawableGameComponent).Visible).Cast<DrawableGameComponent>())
            {
                component.Draw(gameTime);
            }
        }

        protected internal virtual void StateChanged(object sender, EventArgs e)
        {
        }

        public virtual void Show()
        {
            Enabled = true;
            Visible = true;

            foreach (GameComponent component in childComponents)
            {
                component.Enabled = true;
                if (component is DrawableGameComponent component1)
                    component1.Visible = true;
            }
        }

        public virtual void Hide()
        {
            Enabled = false;
            Visible = false;

            foreach (GameComponent component in childComponents)
            {
                component.Enabled = false;
                if (component is DrawableGameComponent component1)
                    component1.Visible = false;
            }
        }

        #endregion
    }
}
