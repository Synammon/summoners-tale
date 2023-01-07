using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SummonersTale.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SummonersTale
{
    public enum ButtonRole { Accept, Cancel, Menu }

    public class Button : Control
    {
        #region

        public event EventHandler Click;

        #endregion
        #region Field Region

        private readonly Texture2D _background;
        float _frames;

        public ButtonRole Role { get; set; }
        public int Width { get { return _background.Width; } }
        public int Height { get { return _background.Height; } }

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region  

        public Button(Texture2D background, ButtonRole role)
        {
            Role = role;
            _background = background;
        }

        #endregion

        #region Method Region

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new(
            (int)Position.X,
                (int)Position.Y,
                _background.Width,
                _background.Height);

            spriteBatch.Draw(_background, destination, Color.White);

            _spriteFont = ControlManager.SpriteFont;

            Vector2 size = _spriteFont.MeasureString(Text);
            Vector2 offset = new((_background.Width - size.X) / 2, (_background.Height / 2));

            spriteBatch.DrawString(_spriteFont, Text, ((Position + offset)), Color);
        }

        public override void HandleInput()
        {
            MouseState mouse = Mouse.GetState();
            Point position = new(mouse.X, mouse.Y);

            Rectangle destination = new(
                (int)_position.X,
                (int)_position.Y,
                _background.Width,
                _background.Height);

            if ((Role == ButtonRole.Accept && Xin.WasKeyReleased(Keys.Enter)) ||
                (Role == ButtonRole.Accept && Xin.WasKeyReleased(Keys.Space)))
            {
                OnClick();
                return;
            }

            if (Role == ButtonRole.Cancel && Xin.WasKeyReleased(Keys.Escape))
            {
                OnClick();
                return;
            }

            if (destination.Contains(position) && Xin.WasMouseReleased(MouseButton.Left) && _frames >= 5)
            {
                OnClick();
            }
        }

        private void OnClick()
        {
            Click?.Invoke(this, null);
        }

        public override void Update(GameTime gameTime)
        {
            _frames++;
            HandleInput();
        }

        public void Show()
        {
            _frames = 0;
        }

        #endregion
    }
}
