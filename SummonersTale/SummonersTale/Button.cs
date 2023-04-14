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
        public event EventHandler Down;

        #endregion
        #region Field Region

        private readonly Texture2D _background;
        float _frames;

        public ButtonRole Role { get; set; }
        public int Width { get { return _background.Width; } }
        public int Height { get { return _background.Height; } }
        public int? Index { get; set; } = null;
        public bool Scale {  get; set; }

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region  

        public Button(Texture2D background, ButtonRole role)
        {
            Role = role;
            _background = background;
            Size = new(background.Width, background.Height);
            Text = "";
            Scale = true;
        }

        #endregion

        #region Method Region

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new(
            (int)Position.X,
            (int)Position.Y,
            (int)Size.X,
            (int)Size.Y);

            spriteBatch.Draw(_background, destination, Color.White);

            _spriteFont = ControlManager.SpriteFont;

            Vector2 size = _spriteFont.MeasureString(Text);
            Vector2 offset = new((Size.X - size.X) / 2, ((Size.Y - size.Y) / 2));

            spriteBatch.DrawString(_spriteFont, Text, Helper.NearestInt((Position + offset)), Color);
        }

        public override void HandleInput()
        {
            MouseState mouse = Mouse.GetState();
            Point position = new(mouse.X, mouse.Y);
            Rectangle destination = new(
                (int)(Position.X + Offset.X),
                (int)(Position.Y + Offset.Y),
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

            if (Xin.WasMouseReleased(MouseButton.Left) && _frames >= 5)
            {
                Rectangle r = Scale ? destination.Scale(Settings.Scale) : destination;

                if (r.Contains(position))
                {
                    OnClick();
                    return;
                }
            }

            if (Xin.TouchReleased() && _frames >= 5)
            {
                Rectangle rectangle= destination.Scale(Settings.Scale);

                if (rectangle.Contains(Xin.TouchReleasedAt))
                {
                    OnClick();
                    return;
                }
            }

            if (Xin.IsMouseDown(MouseButton.Left))
            {
                Rectangle rectangle = destination.Scale(Settings.Scale);

                if (rectangle.Contains(Xin.MouseAsPoint))
                {
                    OnDown();
                    return;
                }
            }

            if (Xin.TouchLocation != new Vector2(-1, -1))
            {
                Rectangle rectangle = new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

                if (rectangle.Scale(Settings.Scale).Contains(Xin.TouchLocation))
                {
                    OnDown();
                    return;
                }
            }
        }

        private void OnDown()
        {
            Down?.Invoke(this, null);
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
