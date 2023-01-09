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
                Position = new Vector2(100, 100),
                Size = new Vector2(300, 30),
                Color = Color.Black,
                Visible = true,
                Enabled = true,
                Offset = new Vector2(0, TitleBar.Height)
            };

            _test.Click += Test_Click;
            Controls.Add(_test);
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
