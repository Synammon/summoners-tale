using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummonersTale.Forms
{
    internal class Checkbox : Control
    {
        public bool Checked { get; set; } = true;
        private Dictionary<bool, Texture2D> textures = new Dictionary<bool, Texture2D>();
        public int Width { get { return textures[Checked].Width; } }
        public int Height { get { return textures[Checked].Height; } }
        private double timer = 0;

        public Checkbox(Texture2D selected, Texture2D unselected) 
        { 
            textures.Add(true, selected);
            textures.Add(false, unselected);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new(
            (int)Position.X,
            (int)Position.Y,
            (int)Size.X,
            (int)Size.Y);

            spriteBatch.Draw(textures[Checked], destination, Color.White);

            _spriteFont = ControlManager.SpriteFont;

            Vector2 size = _spriteFont.MeasureString(Text);
            Vector2 offset = new((Size.X + 10), ((Size.Y - size.Y) / 2));

            spriteBatch.DrawString(_spriteFont, Text, Helper.NearestInt((Position + offset)), Color);
        }

        public override void HandleInput()
        {
            if (timer < .25) return;

            Point mouse = Xin.MouseAsPoint;
            Rectangle destination = new(
                (int)Position.X,
                (int)Position.Y,
                (int)Size.X,
                (int)Size.Y);

            _mouseOver = destination.Contains(mouse);

            if (HasFocus && Xin.WasKeyReleased(Keys.Space))
            {
                Checked = !Checked;
                timer = 0;
            }
            else if (_mouseOver && Xin.WasMouseReleased(MouseButton.Left))
            {
                Checked = !Checked;
                timer = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            HandleInput();
        }
    }
}
