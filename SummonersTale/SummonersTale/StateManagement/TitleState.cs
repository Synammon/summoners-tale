using Microsoft.Xna.Framework;
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
                manager.ChangeState((GameState)Game.Services.GetService(typeof(IMainMenuState)));
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            string message = "Game with begin in " + ((int)_timer).ToString() + " seconds.";
            Vector2 size = _spriteFont.MeasureString(message);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            Vector2 location = new((TargetWidth - size.X) / 2, TargetHeight - (_spriteFont.LineSpacing * 5));
            SpriteBatch.Begin();

            SpriteBatch.DrawString(
                _spriteFont, 
                message, 
                Helper.NearestInt(location), 
                Color.White);

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin();

            SpriteBatch.Draw(renderTarget, new Rectangle(new(0,0), new(1920,1080)), Color.White);

            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
