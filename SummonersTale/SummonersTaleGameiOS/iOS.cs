using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SummonersTale;
using SummonersTale.StateManagement;

namespace SummonersTaleGameiOS
{
    public class iOS : Game
    {
        private readonly GameStateManager _manager;
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GamePlayState _playState;
        private TitleState _titleState;
        private MainMenuState _mainMenuState;
        private NewGameState _newGameState;
        private ConversationState _conversationState;

        public SpriteBatch SpriteBatch => _spriteBatch;

        public TitleState TitleState => _titleState;
        public GamePlayState PlayState => _playState;
        public MainMenuState MainMenuState => _mainMenuState;
        public NewGameState NewGameState => _newGameState;
        public ConversationState ConversationState => _conversationState;

        public iOS()
        {
            Settings.Load();

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Settings.Resolution.X,
                PreferredBackBufferHeight = Settings.Resolution.Y,
            };

            _graphics.ApplyChanges();
            _manager = new GameStateManager(this);

            Services.AddService(typeof(GraphicsDeviceManager), _graphics);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Components.Add(_manager);
        }

        protected override void Initialize()
        {
            Components.Add(new FramesPerSecond(this));
            Components.Add(new Xin(this));

            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), _spriteBatch);

            _playState = new(this);
            _titleState = new(this);
            _mainMenuState = new(this);
            _newGameState = new(this);
            _conversationState = new(this);

            _manager.PushState(_titleState);
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}