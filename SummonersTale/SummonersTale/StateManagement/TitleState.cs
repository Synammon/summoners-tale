using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummonersTale.StateManagement
{
    public class TitleState : BaseGameState
    {
        private SpriteFont _spriteFont;
        private double _timer;

        public TitleState(Game game) : base(game)
        {
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
                manager.ChangeState(GameRef.PlayState);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            string message = "Game with begin in " + ((int)_timer).ToString() + " seconds.";
            Vector2 size = _spriteFont.MeasureString(message);

            GameRef.SpriteBatch.Begin();

            GameRef.SpriteBatch.DrawString(
                _spriteFont, 
                message, 
                new((1280 - size.X) / 2, 720 - (_spriteFont.LineSpacing * 5)), 
                Color.White);

            GameRef.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
