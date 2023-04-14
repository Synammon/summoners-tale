using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonsterEditor.Forms;
using SharpFont.Cache;
using SummonersTale;
using SummonersTale.StateManagement;

namespace MonsterEditor
{
    public class MonsterEditor : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameStateManager manager;
        private MainForm mainForm;

        public MonsterEditor()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080,
                IsFullScreen = false,
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Services.AddService(typeof(GraphicsDeviceManager), _graphics);

            manager = new GameStateManager(this);
            Components.Add(manager);
            Components.Add(new Xin(this));
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), _spriteBatch);

            mainForm = new(this, Vector2.Zero, Point.Zero);

            manager.ChangeState(mainForm);
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