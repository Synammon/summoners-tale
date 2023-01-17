using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonersTale.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummonersTale.Forms
{
    public interface IMenuForm
    {
        GameState GameState { get; }
    }
    public class MenuForm : Form, IMenuForm
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        
        private Button _menuButton;
        private Button _saveButton;
        private Button _exitButton;

        private float _frames;
        private TimeSpan _timer;
        private bool _show;
        private float _offset;

        public GameState GameState => this;

        public MenuForm(Game game, Vector2 position, Point size) : base(game, position, size)
        {
        }

        protected override void LoadContent()
        {

            base.LoadContent();
            graphicsDeviceManager = Game.Services.GetService<GraphicsDeviceManager>();

            if (Game.Services.GetService<IMenuForm>() == null)
                Game.Services.AddService(typeof(IMenuForm), this);

            Texture2D background = new(GraphicsDevice, 1, 1);
            Color[] data = new Color[1 * 1];

            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            background.SetData(data);

            Background.Image = background;

            _menuButton = new(content.Load<Texture2D>("GUI/g21688"), ButtonRole.Menu)
            {
                Text = "",
                Position = new(graphicsDeviceManager.PreferredBackBufferWidth - 84, 32)
            };    

            _menuButton.Click += MenuButton_Click;

            _saveButton = new(content.Load<Texture2D>("GUI/g22337"), ButtonRole.Menu)
            {
                Text = "",
                Position= new(graphicsDeviceManager.PreferredBackBufferWidth, 112)
            };

            _saveButton.Click += SaveButton_Click;

            _exitButton = new(content.Load<Texture2D>("GUI/g22379"), ButtonRole.Menu)
            {
                Text = "",
                Position = new(graphicsDeviceManager.PreferredBackBufferWidth, 192)
            };

            _exitButton.Click += ExitButton_Click;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            _show = false;
            _timer = TimeSpan.FromSeconds(0);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            if (_frames < 10) return;

            _show = false;
            _timer = TimeSpan.FromSeconds(0); 
        }

        public override void Update(GameTime gameTime)
        {
            _frames++;

            if (_frames < 5) return;

            _timer += gameTime.ElapsedGameTime;

            if (_timer.TotalSeconds > .25f && _show)
            {
                _timer = TimeSpan.FromSeconds(.25f);
            }

            if (_show)
            {
                _offset = (float)(84 * (_timer.TotalSeconds / .25f));
            }
            else
            {
                _offset = (float)(84 - 84 * (_timer.TotalSeconds / .25f));
            }

            if (_timer.TotalSeconds >= .25f && !_show)
            {
                _timer = TimeSpan.FromSeconds(.25f);
                manager.PopTopMost();
            }

            if (_timer.TotalSeconds >= 0.25f)
            {
                _menuButton.Update(gameTime);
                _saveButton.Update(gameTime);
                _exitButton.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _exitButton.Position = new(graphicsDeviceManager.PreferredBackBufferWidth - _offset, _exitButton.Position.Y);
            _saveButton.Position = new(graphicsDeviceManager.PreferredBackBufferWidth - _offset, _saveButton.Position.Y);

            SpriteBatch.Begin();

            _menuButton.Draw(SpriteBatch);
            _exitButton.Draw(SpriteBatch);
            _saveButton.Draw(SpriteBatch);

            SpriteBatch.End();
        }

        public override void Show()
        {
            _frames = 0;
            _timer = TimeSpan.Zero;
            _show = true;
            base.Show();
        }
    }
}
