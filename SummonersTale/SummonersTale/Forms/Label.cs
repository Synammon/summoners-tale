using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummonersTale.Forms
{
    public class Label : Control
    {
        public Label()
        {
            _visible = true;
            _enabled = true;
            _tabStop = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_spriteFont, Text, Helper.NearestInt(Position), Color);
        }

        public override void HandleInput()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
