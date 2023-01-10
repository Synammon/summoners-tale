using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SummonersTale.Forms
{
    public class MainForm : Form
    {
        private Button _test;
        private Textbox _textbox;

        public MainForm(Game game, Vector2 position, Point size) : base(game, position, size)
        {
            Title = "";            
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _test = new(content.Load<Texture2D>(@"GUI/Button"), ButtonRole.Menu)
            {
                Text = "Click Me",
                Position = new(100, 100),
                Size = new(300, 30),
                Color = Color.Black,
                Visible = true,
                Enabled = true,
                Offset = new(0, TitleBar.Height)
            };

            _test.Click += Test_Click;
            Controls.Add(_test);

            Color[] buffer = new Color[3 * 20];

            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = Color.Black;

            Texture2D caret = new(GraphicsDevice, 3, 20);
            caret.SetData(buffer);

            _textbox = new(content.Load<Texture2D>(@"GUI/TextBox"), caret)
            {
                HasFocus = true,
                Visible = true,
                Position = new(100, 20),
                Enabled = true,
                Size = new(300, 30),
                Color = Color.Black,
            };
            Controls.Add(_textbox);
        }

        private void Test_Click(object sender, EventArgs e)
        {
            MessageForm frm = new(Game, new(500, 500), new(300, 100), "Message box!", true);
            manager.PushTopMost(frm);
            Visible = true;
            Enabled = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
