﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummonersTale.StateManagement
{
    public interface ITitleState
    {
        GameState GameState { get; }
    }

    public class TitleState : BaseGameState, ITitleState
    {
        private SpriteFont _spriteFont;
        private double _timer;

        public GameState GameState => this;

        public TitleState(Game game) : base(game)
        {
            Game.Services.AddService((ITitleState)this);

            Initialize();
            LoadContent();
        }

        public override void Initialize()
        {
            _timer = 5;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteFont = content.Load<SpriteFont>(@"Fonts/MainFont");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _timer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer <= 0)
            {
                manager.ChangeState((GameState)Game.Services.GetService(typeof(IGamePlayState)));
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            string message = "Game with begin in " + ((int)_timer).ToString() + " seconds.";
            Vector2 size = _spriteFont.MeasureString(message);

            SpriteBatch.Begin();

            SpriteBatch.DrawString(
                _spriteFont, 
                message, 
                new((1280 - size.X) / 2, 720 - (_spriteFont.LineSpacing * 5)), 
                Color.White);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}