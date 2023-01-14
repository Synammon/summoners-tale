using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummonersTale.StateManagement
{
    public class BaseGameState : GameState
    {
        #region Field Region

        protected const int TargetWidth = 1280;
        protected const int TargetHeight = 720;

        protected readonly static Random random = new();
        protected readonly SpriteBatch spriteBatch;
        protected readonly RenderTarget2D renderTarget;

        #endregion

        #region Proptery Region

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        #endregion

        #region Constructor Region

        public BaseGameState(Game game)
            : base(game)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            renderTarget = new(GraphicsDevice, TargetWidth, TargetHeight);
        }

        #endregion
    }
}
