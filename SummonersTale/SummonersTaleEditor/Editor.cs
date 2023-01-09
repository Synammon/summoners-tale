using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SummonersTale;
using SummonersTale.Forms;
using SummonersTale.StateManagement;

namespace SummonersTaleEditor
{
    public class Editor : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly GameStateManager manager;
        private MainForm _mainForm;

        public Editor()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080,
                IsFullScreen = false,
                SynchronizeWithVerticalRetrace = false
            };

            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Services.AddService(typeof(GraphicsDeviceManager), _graphics);

            manager = new GameStateManager(this);
            Components.Add(manager);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Components.Add(new FramesPerSecond(this));
            Components.Add(new Xin(this));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            Services.AddService(typeof(SpriteBatch), _spriteBatch);

            _mainForm = new(this, Vector2.Zero, new(1920, 1080)) 
            { 
                FullScreen = false,
                Title = "A Summoner's Tale Editor"
            };

            manager.ChangeState(_mainForm);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}