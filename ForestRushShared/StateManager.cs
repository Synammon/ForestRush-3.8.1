using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForestRushShared

{
    public interface IStateManager
    {
        BaseGameState CurrentState { get; }

        event EventHandler StateChanged;

        void PushTopMost(BaseGameState state);
        void PushState(BaseGameState state);
        void ChangeState(BaseGameState state);
        void PopState();
        void PopTopMost();
        bool ContainsState(BaseGameState state);
    }

    public class StateManager : GameComponent, IStateManager
    {
        #region Field Region

        private readonly Stack<BaseGameState> gameStates = new();

        private const int startDrawOrder = 5000;
        private const int drawOrderInc = 50;
        private const int MaxDrawOrder = 5000;

        private int drawOrder;

        #endregion

        #region Event Handler Region

        public event EventHandler StateChanged;

        #endregion

        #region Property Region

        public BaseGameState CurrentState
        {
            get { return gameStates.Peek(); }
        }

        #endregion

        #region Constructor Region

        public StateManager(Game game)
            : base(game)
        {
            Game.Services.AddService(typeof(IStateManager), this);
            drawOrder = startDrawOrder;
        }

        #endregion

        #region Method Region

        public void PushTopMost(BaseGameState state)
        {
            drawOrder += MaxDrawOrder;
            state.DrawOrder = drawOrder;
            gameStates.Push(state);
            Game.Components.Add(state);
            StateChanged += state.StateChanged;
            OnStateChanged();
        }

        public void PushState(BaseGameState state)
        {
            drawOrder += drawOrderInc;
            state.DrawOrder = drawOrder;
            AddState(state);
            OnStateChanged();
        }

        private void AddState(BaseGameState state)
        {
            gameStates.Push(state);
            if (!Game.Components.Contains(state))
                Game.Components.Add(state);
            StateChanged += state.StateChanged;
        }

        public void PopState()
        {
            if (gameStates.Count != 0)
            {
                RemoveState();
                drawOrder -= drawOrderInc;
                OnStateChanged();
            }
        }

        public void PopTopMost()
        {
            if (gameStates.Count > 0)
            {
                RemoveState();
                drawOrder -= MaxDrawOrder;
                OnStateChanged();
            }
        }

        private void RemoveState()
        {
            BaseGameState state = gameStates.Peek();

            StateChanged -= state.StateChanged;
            Game.Components.Remove(state);
            gameStates.Pop();
        }

        public void ChangeState(BaseGameState state)
        {
            while (gameStates.Count > 0)
            {
                RemoveState();
            }

            drawOrder = startDrawOrder;
            state.DrawOrder = drawOrder;
            drawOrder += drawOrderInc;

            AddState(state);
            OnStateChanged();
        }

        public bool ContainsState(BaseGameState state)
        {
            return gameStates.Contains(state);
        }

        protected internal virtual void OnStateChanged()
        {
            StateChanged?.Invoke(this, null);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion
    }
}
