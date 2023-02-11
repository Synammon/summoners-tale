using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psilibrary.TileEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummonersTale.Forms
{
    public class MainForm : Form
    {
        private MapDisplay _mapDisplay;
        private RenderTarget2D _renderTarget2D;
        private Button _menuButton;
        private IMenuForm _menuForm;
        private FileForm _fileForm;

        public MainForm(Game game, Vector2 position, Point size) : base(game, position, size)
        {
            Title = "";            
            FullScreen = true;
        }

        public override void Initialize()
        {
            Engine.Reset(new(0, 0, 1280, 1080), 32, 32);
            Engine.ViewportRectangle = new(0, 0, 1280, 1080);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            GraphicsDeviceManager gdm = Game.Services.GetService<GraphicsDeviceManager>();

            _menuButton = new(content.Load<Texture2D>(@"GUI/g21688"), ButtonRole.Menu)
            {
                Text = "",
                Position = new(gdm.PreferredBackBufferWidth - 84, 20)
            };

            _menuButton.Click += MenuButton_Click;

            _renderTarget2D = new(GraphicsDevice, 1280, 1080);

            _mapDisplay = new(null, 40, 32)
            {
                HasFocus = false,
                Position = new(0, 0)
            };

            TileSheet sheet = new(content.Load<Texture2D>(@"Tiles/Overworld"), "test", new(40, 36, 16, 16));
            TileSet set = new(sheet);

            TileLayer ground = new(100, 100, 0, 0);
            TileLayer edge = new(100, 100, -1, -1);
            TileLayer building = new(100, 100, -1, -1);
            TileLayer decore = new(100, 100, -1, -1);

            TileMap tileMap = new(set, ground, edge, building, decore, "test");

            _mapDisplay.SetMap(tileMap);

            Color[] data = new Color[1];
            data[0] = Color.Transparent;

            Texture2D b = new(GraphicsDevice, 1, 1);
            b.SetData(data);

            _fileForm = new(Game, Vector2.Zero, Size);
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            _menuForm = (IMenuForm)Game.Services.GetService<IMenuForm>();

            manager.PushTopMost(_menuForm.GameState);
            Visible = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _mapDisplay.Update(gameTime);
            _menuButton.Update(gameTime);

            if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                _fileForm.Role = FileFormRole.Save;
                manager.PushState(_fileForm);
            }

            if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.F2))
            {
                _fileForm.Role = FileFormRole.Open;
                manager.PushState(_fileForm);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(_renderTarget2D);

            _mapDisplay.Draw(spriteBatch);

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin();
            spriteBatch.Draw(_renderTarget2D, new Rectangle(0, 0, 1280, 1080), Color.White);

            Color[] data = new Color[640 * 1080];

            for (int i = 0; i < data.Length; i++)
                data[i] = Color.White;

            Texture2D background = new(GraphicsDevice, 640, 1080);
            background.SetData(data);

            spriteBatch.Draw(background, new Rectangle(1280, 0, 640, 1080), Color.White);

            _menuButton.Position = new(1920 - 84, 32);
            _menuButton.Draw(SpriteBatch);

            spriteBatch.End();
        }
    }
}
