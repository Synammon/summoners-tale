using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummonersTale.StateManagement
{
    public interface IStateManager
    {
        GameState CurrentState { get; }

        event EventHandler StateChanged;

        void PushTopMost(GameState state);
        void PushState(GameState state);
        void ChangeState(GameState state);
        void PopState();
        void PopTopMost();
        bool ContainsState(GameState state);
    }

    public class GameStateManager : GameComponent, IStateManager
    {
        #region Field Region

        private readonly Stack<GameState> gameStates = new();

        private const int startDrawOrder = 5000;
        private const int drawOrderInc = 50;
        private const int MaxDrawOrder = 5000;

        private int drawOrder;

        #endregion

        #region Event Handler Region

        public event EventHandler StateChanged;

        #endregion

        #region Property Region

        public GameState CurrentState
        {
            get { return gameStates.Peek(); }
        }

        #endregion

        #region Constructor Region

        public GameStateManager(Game game)
            : base(game)
        {
            Game.Services.AddService(typeof(IStateManager), this);
            drawOrder = startDrawOrder;
        }

        #endregion

        #region Method Region

        public void PushTopMost(GameState state)
        {
            drawOrder += MaxDrawOrder;
            state.DrawOrder = drawOrder;
            gameStates.Push(state);
            Game.Components.Add(state);
            StateChanged += state.StateChanged;
            OnStateChanged();
        }

        public void PushState(GameState state)
        {
            drawOrder += drawOrderInc;
            state.DrawOrder = drawOrder;
            AddState(state);
            OnStateChanged();
        }

        private void AddState(GameState state)
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
            GameState state = gameStates.Peek();

            StateChanged -= state.StateChanged;
            Game.Components.Remove(state);
            gameStates.Pop();
        }

        public void ChangeState(GameState state)
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

        public bool ContainsState(GameState state)
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
