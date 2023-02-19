using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SummonersTale.StateManagement;
using SummonersTale;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SummonersTale.Forms;

namespace SummonersTale.StateManagement
{
    public interface IMainMenuState
    { 
        GameState GameState { get; }
    }

    public class MainMenuState : BaseGameState, IMainMenuState
    {
        #region Field Region

        private double timer;

        private Button newGameButton;
        private Button oldGameButton;
        private Button optionsButton;
        private Button creditsButton;
        private Button leaveButton;
        private RenderTarget2D renderTarget2D;

        public GameState GameState => this;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public MainMenuState(Game game) : base(game)
        {
            Game.Services.AddService<IMainMenuState>(this);
        }

        #endregion

        #region Method Region

        protected override void LoadContent()
        {
            base.LoadContent();
            renderTarget2D = new(GraphicsDevice, TargetWidth, TargetHeight);
            Texture2D texture = new(GraphicsDevice, 400, 50);

            Color[] buffer = new Color[400 * 50];

            for (int i = 0; i < 400 * 50; i++)
                buffer[i] = Color.Black;

            texture.SetData(buffer);

            newGameButton = new(content.Load<Texture2D>(@"GUI/g9202"), ButtonRole.Menu)
            {
                Text = "New Game",
                Position = new((TargetWidth - 200) / 2, 100),
                Color = Color.White,
                Size = new(200, 50)
            };

            oldGameButton = new(content.Load<Texture2D>(@"GUI/g9202"), ButtonRole.Menu)
            {
                Text = "Continue",
                Position = new((TargetWidth - 200) / 2, 200),
                Color = Color.White,
                Size = new(200, 50)
            };

            optionsButton = new(content.Load<Texture2D>(@"GUI/g9202"), ButtonRole.Menu)
            {
                Text = "Options",
                Position = new((TargetWidth - 200) / 2, 300),
                Color = Color.White,
                Size = new(200, 50)
            };

            creditsButton = new(content.Load<Texture2D>(@"GUI/g9202"), ButtonRole.Menu)
            {
                Text = "Credits",
                Position = new((TargetWidth - 200) / 2, 400),
                Color = Color.White,
                Size = new(200, 50)
            };

            leaveButton = new(content.Load<Texture2D>(@"GUI/g9202"), ButtonRole.Menu)
            {
                Text = "Leave",
                Position = new((TargetWidth - 200) / 2, 500),
                Color = Color.White,
                Size = new(200, 50)
            };

            newGameButton.Click += NewGameButton_Click;
            oldGameButton.Click += OldGameButton_Click;
            optionsButton.Click += OptionsButton_Click;
            creditsButton.Click += CreditsButton_Click;
            leaveButton.Click += LeaveButton_Click;

            Controls.Add(newGameButton);
            Controls.Add(oldGameButton);
            Controls.Add(optionsButton);
            Controls.Add(creditsButton);
            Controls.Add(leaveButton);
        }

        private void LeaveButton_Click(object sender, EventArgs e)
        {
            //Game.Exit();
        }

        private void CreditsButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OldGameButton_Click(object sender, EventArgs e)
        {
            manager.PushState(Game.Services.GetService<IConversationState>().GameState);
            Visible = true;
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            manager.PushState(Game.Services.GetService<INewGameState>().GameState);
        }

        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            Controls.Update(gameTime);

            if (timer < 0.25) return;

            if (Xin.WasKeyReleased(Keys.Escape))
            {
                //Game.Exit();
            }


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(renderTarget2D);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            Controls.Draw(SpriteBatch);

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin();

            SpriteBatch.Draw(
                renderTarget2D, 
                new Rectangle(0, 0, Settings.Resolution.X, Settings.Resolution.Y), 
                Color.White);

            SpriteBatch.End();
        }

        public override void Show()
        {
            timer = 0;

            base.Show();
        }

        #endregion
    }
}
