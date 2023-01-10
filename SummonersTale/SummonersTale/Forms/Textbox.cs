using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummonersTale.Forms
{
    internal class Textbox : Control
    {
        private readonly Texture2D _background;
        private readonly Texture2D _caret;
        private double timer;
        private Color _tint;
        private List<string> validChars = new();

        public Textbox(Texture2D background, Texture2D caret)
            : base()
        {
            _text = "";
            _background = background;
            _caret = caret;
            _tint = Color.Black;
            foreach (char c in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTWYYXZ0123456789 -_".ToCharArray())
            {
                validChars.Add(c.ToString());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 dimensions = ControlManager.SpriteFont.MeasureString(_text);
            dimensions.Y = 0;
            spriteBatch.Draw(
                _background,
                new Rectangle(Helper.V2P(Position), Helper.V2P(Size)),
                Color.White);
            spriteBatch.DrawString(
                ControlManager.SpriteFont,
                Text,
                Helper.NearestInt(Position + Vector2.One * 5),
                Color.Black,
                0,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                1f);
            spriteBatch.Draw(
                _caret,
                Position + dimensions + Vector2.One * 5,
                _tint);
        }

        public override void HandleInput()
        {
            if (!HasFocus)
            {
                return;
            }

            List<Keys> keys = Xin.KeysPressed();

            foreach (Keys key in keys)
            {
                string value = Enum.GetName(typeof(Keys), key);

                if (value == "Back" && _text.Length > 0)
                {
                    _text = _text.Substring(0, _text.Length - 1);
                    return;
                }
                else if (value == "Back")
                {
                    Text = "";
                    return;
                }

                if (value.Length == 2 && value.Substring(0,1) == "D")
                {
                    value = value.Substring(1);
                }

                if (!Xin.IsKeyDown(Keys.LeftShift) && !Xin.IsKeyDown(Keys.RightShift) && !Xin.KeyboardState.CapsLock)
                {
                    value = value.ToLower();
                }

                if (validChars.Contains(value))
                {
                    if (ControlManager.SpriteFont.MeasureString(_text + value).X < Size.X)
                        _text += value;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            timer += 3 * gameTime.ElapsedGameTime.TotalSeconds;
            double sine = Math.Sin(timer);

            _tint = Color.Black * (int)Math.Round(Math.Abs(sine));
        }
    }
}
