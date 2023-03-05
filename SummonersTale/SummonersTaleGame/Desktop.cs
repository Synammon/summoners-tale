using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Psilibrary.ShadowMonsters;
using Psilibrary.TileEngine;
using SummonersTale;
using SummonersTale.ShadowMonsters;
using SummonersTale.StateManagement;
using System;
using System.Collections.Generic;

namespace SummonersTaleGame
{
    public class Desktop : Game
    {
        private readonly GameStateManager _manager;
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GamePlayState _playState;
        private TitleState _titleState;
        private MainMenuState _mainMenuState;
        private NewGameState _newGameState;
        private ConversationState _conversationState;
        private DamageState _damageState;
        private BattleState _battleState;
        private BattleOverState _battleOverState;
        private LevelUpState _levelUpState;

        private ConversationManager _conversationManager;

        public SpriteBatch SpriteBatch => _spriteBatch;

        public TitleState TitleState => _titleState;
        public GamePlayState PlayState => _playState;
        public MainMenuState MainMenuState=> _mainMenuState;
        public NewGameState NewGameState => _newGameState;
        public ConversationState ConversationState => _conversationState;
        public DamageState DamageState => _damageState;
        public BattleState BattleState => _battleState;
        public BattleOverState BattleOverState => _battleOverState;
        public LevelUpState LevelUpState => _levelUpState;

        public Desktop()
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

            _conversationManager = new(this);
            _conversationManager.LoadConverstions(this);
            _conversationManager.WriteConversations();

            Components.Add(_conversationManager);

            _conversationState = new(this);
            _playState = new(this);
            _titleState = new(this);
            _mainMenuState = new(this);
            _newGameState = new(this);
            _damageState = new(this);
            _battleOverState = new(this);
            _battleState = new(this);
            _levelUpState = new(this);

            _manager.PushState(_titleState);

            Move smash = new()
            {
                Name = "Smash",
                Range = new(1, 6),
                Mana = new(40, 40),
                Target = TargetType.Enemy,
                TargetAttribute = TargetAttribute.Health,
                IsTemporary = false,
                Elements = 0
            };

            string m = smash.ToString();

            smash = Move.FromString(m);

            List<MoveData> moves = new()
            {
                smash
            };

            Move bash = new()
            {
                Name = "Bash",
                Range = new(2, 8),
                Mana = new(30, 30),
                Target = TargetType.Enemy,
                TargetAttribute = TargetAttribute.Health,
                IsTemporary = false,
                Elements = 0
            };

            ShadowMonster monster = new()
            {
                Name = "Goblin",
                Moves = moves,
                Elements = 0,
                Health = new(25, 25),
            };

            monster.LockedMoves.Add(bash);
            monster.UnlockedMoves.Add(smash);

            m = monster.ToString();

            monster = ShadowMonster.FromString(m);
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

            base.Draw(gameTime);
        }
    }
}