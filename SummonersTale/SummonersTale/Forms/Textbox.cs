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
        private readonly Texture2D _border;
        private readonly Texture2D _background;
        private readonly Texture2D _caret;
        
        private double timer;
        private Color _tint;
        private readonly List<string> validChars = new();

        public Textbox(GraphicsDevice graphicsDevice, Vector2 size)
            : base()
        {

            _text = "";

            Size = size;

            _background = new Texture2D(graphicsDevice, (int)size.X, (int)size.Y);
            _background.Fill(Color.White);

            _border = new Texture2D(graphicsDevice, (int)size.X, (int)size.Y);
            _border.Fill(Color.Black);

            _caret = new Texture2D(graphicsDevice, 2, (int)size.Y - 8);
            _caret.Fill(Color.Black);

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

            Rectangle location = new(
                new((int)Position.X, (int)Position.Y), 
                new((int)Size.X, (int)Size.Y));

            spriteBatch.Draw(_border, location.Grow(1), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            spriteBatch.Draw(_background, location, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

            spriteBatch.DrawString(
                ControlManager.SpriteFont,
                Text,
                Helper.NearestInt(Position + Vector2.One * 5),
                Color.Black,
                0,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f);
            if (!HasFocus) { return; }
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

                if (key == Keys.OemSemicolon && !(Xin.IsKeyDown(Keys.RightShift) && !Xin.IsKeyDown(Keys.LeftShift)))
                {
                    _text += ';';
                }
                else if (key == Keys.OemSemicolon && (Xin.IsKeyDown(Keys.RightShift) || Xin.IsKeyDown(Keys.LeftShift)))
                    {
                    _text += ":";
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
