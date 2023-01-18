using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SummonersTale.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShadowMonsters.Controls;
using System.Reflection.Metadata.Ecma335;
using SummonersTale.SpriteClasses;

namespace SummonersTale.StateManagement
{
    public interface INewGameState
    {
        GameState GameState { get; }
    }

    public class NewGameState : BaseGameState, INewGameState
    {
        private Rectangle _destination = new(0, 0, TargetWidth, TargetHeight);
        private Rectangle _portraitDestination = new(599, 57, 633, 617);
        private RightLeftSelector _portraitSelector;
        private RightLeftSelector _genderSelector;
        private Textbox _nameTextBox;
        private readonly Dictionary<string, Texture2D> _femalePortraits;
        private readonly Dictionary<string, Texture2D> _malePortraits;
        private Button _create;
        private Button _back;
        private readonly Dictionary<string, Animation> animations = new();
        private AnimatedSprite _sprite;

        public GameState GameState => this;

        public NewGameState(Game game) : base(game)
        {
            Game.Services.AddService<INewGameState>(this);
            _femalePortraits = new();
            _malePortraits = new();
        }

        public override void Initialize()
        {
            animations.Clear();

            Animation animation = new(3, 32, 32, 0, 0) { CurrentFrame = 0, FramesPerSecond = 5 };
            animations.Add("walkdown", animation);

            animation = new(3, 32, 32, 0, 32) { CurrentFrame = 0, FramesPerSecond = 5 };
            animations.Add("walkleft", animation);

            animation = new(3, 32, 32, 0, 64) { CurrentFrame = 0, FramesPerSecond = 5 };
            animations.Add("walkright", animation);

            animation = new(3, 32, 32, 0, 96) { CurrentFrame = 0, FramesPerSecond = 5 };
            animations.Add("walkup", animation);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _genderSelector = new(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Position = new Vector2(207 - 70, 298)
            };
            _genderSelector.SelectionChanged += GenderSelector_SelectionChanged;
            _genderSelector.SetItems(new[] { "Female", "Male" }, 270);

            _portraitSelector = new(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Position = new Vector2(207 - 70, 458)
            };

            _portraitSelector.SelectionChanged += PortraitSelector_SelectionChanged;

            Color[] colourData = new Color[2 * 25];

            for (int i = 0; i < colourData.Length; i++)
            {
                colourData[i] = Color.White;
            }

            Texture2D caret = new(GraphicsDevice, 2, 25);
            caret.SetData(colourData);

            _nameTextBox = new(
                null,
                caret)
            {
                Position = new(207, 138),
                HasFocus = true,
                Enabled = true,
                Color = Color.White,
                Text = "Bethany"
            };

            _femalePortraits.Add(
                "Female 0",
                content.Load<Texture2D>(@"CharacterSprites/femalefighter"));
            _femalePortraits.Add(
                "Female 1",
                content.Load<Texture2D>(@"CharacterSprites/femalepriest"));
            _femalePortraits.Add(
                "Female 2",
                content.Load<Texture2D>(@"CharacterSprites/femalerogue"));
            _femalePortraits.Add(
                "Female 3",
                content.Load<Texture2D>(@"CharacterSprites/femalewizard"));

            _malePortraits.Add(
                "Male 0",
                content.Load<Texture2D>(@"CharacterSprites/malefighter"));
            _malePortraits.Add(
                "Male 1",
                content.Load<Texture2D>(@"CharacterSprites/malepriest"));
            _malePortraits.Add(
                "Male 2",
                content.Load<Texture2D>(@"CharacterSprites/malerogue"));
            _malePortraits.Add(
                "Male 3",
                content.Load<Texture2D>(@"CharacterSprites/malewizard"));        

            
            _portraitSelector.SetItems(_femalePortraits.Keys.ToArray(), 270);
            _sprite = new(_femalePortraits["Female 0"], animations);
            _sprite.CurrentAnimation = "walkdown";
            _sprite.IsAnimating = true;

            _create = new(
                content.Load<Texture2D>(@"GUI\g9202"),
                ButtonRole.Accept)
            {
                Text = "Create",
                Position = new(180, 640)
            };

            _create.Click += Create_Click;

            _back = new(
                content.Load<Texture2D>(@"GUI\g9202"),
                ButtonRole.Accept)
            {
                Text = "Back",
                Position = new(350, 640),
                Color = Color.White
            };

            _back.Click += Back_Click;

        }

        private void Back_Click(object sender, EventArgs e)
        {
            manager.PopState();
        }

        private void Create_Click(object sender, EventArgs e)
        {
            Player = new(
                Game,
                _nameTextBox.Text,
                _genderSelector.SelectedIndex == 0,
                null);

            IGamePlayState gamePlayState = Game.Services.GetService<IGamePlayState>();

            manager.PopState();
            manager.PushState(gamePlayState.GameState);

            gamePlayState.GameState.Enabled = true;
            gamePlayState.NewGame();
        }

        private void GenderSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (_genderSelector.SelectedIndex == 0)
            {
                _portraitSelector.SetItems(_femalePortraits.Keys.ToArray(), 270);
                _nameTextBox.Text = "Bethany";
                _sprite = new(
                    _femalePortraits[_portraitSelector.SelectedItem],
                    animations);
                _sprite.CurrentAnimation = "walkdown";
                _sprite.IsAnimating = true;
            }
            else
            {
                _portraitSelector.SetItems(_malePortraits.Keys.ToArray(), 270);
                _nameTextBox.Text = "Anthony";
                _sprite = new(
                    _malePortraits[_portraitSelector.SelectedItem],
                    animations);
                _sprite.CurrentAnimation = "walkdown";
                _sprite.IsAnimating = true;
            }
        }

        private void PortraitSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (_genderSelector.SelectedIndex == 0)
            {
                _sprite = new(
                    _femalePortraits[_portraitSelector.SelectedItem],
                    animations);
                _sprite.CurrentAnimation = "walkdown";
                _sprite.IsAnimating = true;
            }
            else
            {
                _sprite = new(
                    _malePortraits[_portraitSelector.SelectedItem],
                    animations);
                _sprite.CurrentAnimation = "walkdown";
                _sprite.IsAnimating = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            _portraitSelector.Update(gameTime);
            _genderSelector.Update(gameTime);
            _nameTextBox.Update(gameTime);
            _create.Update(gameTime);
            _back.Update(gameTime);
            _sprite.Update(gameTime);

            if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                Create_Click(this, new());
            }

            if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Back_Click(this, new());
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (_genderSelector.SelectedIndex == 0)
            {
                SpriteBatch.Draw(
                    _femalePortraits[_portraitSelector.SelectedItem],
                    _portraitDestination,
                    new Rectangle(0, 0, 32, 32),
                    Color.White);
            }
            else
            {
                SpriteBatch.Draw(
                    _malePortraits[_portraitSelector.SelectedItem],
                    _portraitDestination,
                    new Rectangle(0, 0, 32, 32),
                    Color.White);
            }

            _genderSelector.Draw(SpriteBatch);
            _portraitSelector.Draw(SpriteBatch);
            _nameTextBox.Draw(SpriteBatch);
            _create.Draw(SpriteBatch);
            _back.Draw(SpriteBatch);
            _sprite.Draw(SpriteBatch);

            base.Draw(gameTime);

            SpriteBatch.End();
        }
    }
}
