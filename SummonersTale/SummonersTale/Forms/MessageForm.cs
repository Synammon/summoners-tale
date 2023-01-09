using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummonersTale.Forms
{
    public enum CloseReason { OK, Cancel, Yes, No }
    public class MessageForm : Form
    {
        public string Message { get; set; }
        public CloseReason CloseReason { get; set; }

        public MessageForm(Game game, Vector2 position, Point size, string message, bool auto) : base(game, position, size)
        {
            Size = size;
            Message = message;

            Bounds = new(
                (_graphicsDevice.PreferredBackBufferWidth - size.X) / 2,
                (_graphicsDevice.PreferredBackBufferHeight - size.Y) / 2,
                size.X, 
                size.Y);

            Position = position;

            if (auto)
            {
                Position = new(Bounds.X, Bounds.Y);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Button okay = new(content.Load<Texture2D>("GUI/Button"), ButtonRole.Accept) 
            { 
                Text = "OK", 
                Color = Color.Black,
            };

            okay.Position = new((Bounds.Width - okay.Width) / 2, Bounds.Height - okay.Height - 10);
            okay.Offset = Position;

            Message = "Message box!";

            Label label = new()
            {
                Text = Message,
                Position = new(
                    (Bounds.Width - ControlManager.SpriteFont.MeasureString(Message).X) / 2,
                    (Bounds.Height - ControlManager.SpriteFont.MeasureString(Message).Y) / 2 - 10),
                Color = Color.Black,
            };

            Controls.Add(label);

            Button cancel = new(content.Load<Texture2D>("GUI/Button"), ButtonRole.Cancel) 
            { 
                Text = "Cancel", 
                Color = Color.Black
            };

            Controls.Add(okay);

            okay.Click += Okay_Click;
            Background.Position = new(Bounds.X, Bounds.Y);
        }

        private void Okay_Click(object sender, EventArgs e)
        {
            CloseReason = CloseReason.OK;
            manager.PopTopMost();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Control control in Controls)
            {
                control.Offset = Position + new Vector2(0, 20);
            }
            if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                manager.PopTopMost();
                CloseReason = CloseReason.Cancel;
            }

            if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                manager.PopTopMost();
                CloseReason = CloseReason.OK;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix m = Matrix.CreateTranslation(new Vector3(Position, 0));

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, m);

            Background.Draw(SpriteBatch);
            TitleBar.Draw(SpriteBatch);
            CloseButton.Draw(SpriteBatch);

            SpriteBatch.End();

            m = Matrix.CreateTranslation(new Vector3(0, 20, 0) + new Vector3(Position, 0));

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, m);

            Controls.Draw(SpriteBatch);

            SpriteBatch.End();
        }
    }
}
