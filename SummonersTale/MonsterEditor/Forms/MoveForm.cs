using Assimp.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psilibrary.ShadowMonsters;
using ShadowMonsters.Controls;
using SummonersTale;
using SummonersTale.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterEditor.Forms
{
    public class MoveForm : Form
    {
        public event EventHandler Closing;

        public MoveData Move { get; set; }
        internal Button Okay { get; set; }
        internal Button Cancel { get; set; }
        internal Textbox Name { get; set; }
        internal Label NameLabel { get; set; }

        internal Textbox Elements { get; set; }
        internal Label ElementsLabel { get; set; }

        internal Textbox Level { get; set; }
        internal Label LevelLabel { get; set; }

        internal RightLeftSelector Target { get; set; }
        internal Label TargetLabel { get; set; }

        internal RightLeftSelector TargetAttribute { get; set; }
        internal Label AttributeLabel { get; set; }

        internal Textbox Mana { get; set; }
        internal Label ManaLabel { get; set; }
        internal Textbox Range { get; set; }
        internal Label RangeLabel { get; set; }
        internal RightLeftSelector Status { get; set; }
        internal Label StatusLabel { get; set; }
        internal Textbox Power { get; set; }
        internal Label PowerLabel { get; set; }
        internal Checkbox Hurts { get; set; }
        internal Checkbox IsTemporary { get; set; }

        public MoveForm(Game game, Vector2 position, Point size) : base(game, position, size)
        {
            FullScreen = false;
            Size = new(600, 580);
            Position = position;

            Texture2D texture2D = new(GraphicsDevice, 100, 100);
            texture2D.Fill(new Color(240, 240, 240));

            PictureBox bg = new(GraphicsDevice, texture2D, Bounds);
            Background = bg;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Okay = new(
                content.Load<Texture2D>(@"GUI/button"),
                ButtonRole.Accept)
            {
                Position = new(this.Bounds.Right - 125, 30),
                Color = Color.Black,
                Text = "OK"
            };

            Okay.Click += Okay_Click;
            Controls.Add(Okay);

            Cancel = new(
                content.Load<Texture2D>(@"GUI/button"),
                ButtonRole.Cancel)
            {
                Position = new(this.Bounds.Right - 125, 80),
                Color = Color.Black,
                Text = "Cancel"
            };

            Cancel.Click += Cancel_Click;
            Controls.Add(Cancel);

            NameLabel = new()
            {
                Position = new(10, 30),
                Color = Color.Black,
                Text = "Name:"
            };

            Controls.Add(NameLabel);

            Name = new(GraphicsDevice, new(300, 30))
            {
                Text = "",
                Position = new(125, 30),
                Color = Color.White,
            };

            Controls.Add(Name);

            ElementsLabel = new()
            { 
                Position = new(10, 80), 
                Color = Color.Black ,
                Text = "Elements:"
            };

            Controls.Add(ElementsLabel);

            Elements = new(GraphicsDevice, new(300, 30))
            {
                Text = "",
                Position = new(125, 80),
                Color = Color.White,
            };

            Controls.Add(Elements);

            LevelLabel = new()
            {
                Position = new(10, 130),
                Color = Color.Black,
                Text = "Level:"
            };

            Controls.Add(LevelLabel);

            Level= new(GraphicsDevice, new(300, 30))
            {
                Text = "",
                Position = new(125, 130),
                Color = Color.White,
            };

            Controls.Add(Level);

            Target = new(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Text = "",
                Position = new(125, 180),
                Color = Color.Black,
                Size = new(32, 32),
            };

            string[] targets = Enum.GetNames(typeof(TargetType));

            Target.SetItems(targets, 200);
            Target.SelectedIndex = 1;

            Controls.Add(Target);

            TargetLabel = new()
            {
                Position = new(10, 180),
                Color = Color.Black,
                Text = "Target:"
            };

            Controls.Add(TargetLabel);

            AttributeLabel = new()
            {
                Position = new(10, 230),
                Color = Color.Black,
                Text = "Attribute:"
            };

            Controls.Add(AttributeLabel);

            TargetAttribute = new(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Position = new(125, 230),
                MaxItemWidth = 100,
                Size = new(32, 32),
                Color = Color.Black,
            };

            TargetAttribute.SetItems(new string[] { "Health", "Attack", "Defense", "SpecialAttack", "SpecialDefence", "Speed", "Accuracy" }, 200);
            Controls.Add(TargetAttribute);

            ManaLabel = new()
            {
                Position = new(10, 280),
                Color = Color.Black,
                Text = "Mana:"
            };

            Mana = new(GraphicsDevice, new(200, 30))
            {
                Text = "",
                Position = new(125, 280),
                Color = new Color(255, 255, 255),
                Size = new(300, 30)
            };
            Controls.Add(ManaLabel);
            Controls.Add(Mana);

            RangeLabel = new()
            {
                Position = new(10, 330),
                Color = Color.Black,
                Text = "Range:"
            };

            Range = new(GraphicsDevice, new(200, 30))
            {
                Text = "",
                Position = new(125, 330),
                Color = new Color(2553, 255, 255),
                Size = new(300, 30)
            };

            Controls.Add(RangeLabel);
            Controls.Add(Range);

            PowerLabel = new()
            {
                Position = new(10, 380),
                Color = Color.Black,
                Text = "Power:"
            };

            Power = new(GraphicsDevice, new(300, 30))
            {
                Text = "",
                Position = new(125, 380),
                Color = new Color(2553, 255, 255),
                Size = new(300, 30)
            };

            Controls.Add(PowerLabel);
            Controls.Add(Power);

            Status = new(
                content.Load<Texture2D>(@"GUI\g22987"),
                content.Load<Texture2D>(@"GUI\g21245"))
            {
                Text = "",
                Position = new(125, 430),
                Color = Color.Black,
                Size = new(32, 32),
            };

            Status.SetItems(new string[] { "Normal", "Sleep", "Confused", "Poisoned", "Paralysis", "Burned", "Frozen" }, 200);
            Controls.Add(Status);

            StatusLabel = new()
            {
                Position = new(10, 430),
                Color = Color.Black,
                Text = "Status:"
            };

            Controls.Add(StatusLabel);

            Hurts = new(content.Load<Texture2D>(@"GUI/selected"), content.Load<Texture2D>(@"GUI/unselected"))
            {
                Size = new(32, 32),
                Position = new(10, 480),
                Text = "Hurts?",
                Color = Color.Black,
                Enabled = true,
                Visible = true,
            };

            Controls.Add(Hurts);

            IsTemporary = new(content.Load<Texture2D>(@"GUI/selected"), content.Load<Texture2D>(@"GUI/unselected"))
            {
                Size = new(32, 32),
                Position = new(10, 530),
                Text = "Is Temporary?",
                Color = Color.Black,
                Enabled = true,
                Visible = true,
                Checked = false
            };

            Controls.Add(IsTemporary);
        }

        private void Okay_Click(object sender, EventArgs e)
        {
            bool error = false;

            if (string.IsNullOrEmpty(Name.Text))
            {
                error = true;
                Name.Text = "Please enter a name.";
                Name.Color = Color.Red;
            }

            if (!int.TryParse(Elements.Text, out int elements))
            {
                error = true;
                Elements.Text = "Please select elements.";
                Elements.Color = Color.Red;
            }

            if (!int.TryParse(Level.Text, out int level))
            {
                error = true;
                Level.Text = "Please select level.";
                Level.Color = Color.Red;
            }

            if (!int.TryParse(Power.Text, out int power))
            {
                error = true;
                Power.Text = "Please select power.";
                Power.Color = Color.Red;
            }

            if (!int.TryParse(Mana.Text, out int mana))
            {
                error = true;
                Mana.Text = "Please select mana.";
                Mana.Color = Color.Red;
            }

            string[] range = Range.Text.Split(':');
            if (range.Length < 2) 
            {
                error = true;
                Range.Text = "Please enter colon separated.";
            }
            if (!int.TryParse(range[0], out int x))
            {
                error = true;
                Range.Text = "Range must be numeric.";
            }

            int y = 0;

            if (range.Length == 2 && !int.TryParse(range[1], out y))
            {
                error = true;
                Range.Text = "Range must be numeric.";
            }

            if (error)
            {
                return;
            }

            Move = new()
            {
                Name = Name.Text,
                Elements = elements,
                Level = level,
                Target = Enum.Parse<TargetType>(Target.SelectedItem),
                Power = power,
                Mana = new(mana, mana),
                Range = new(x, y),
                Status = Status.SelectedIndex,
                TargetAttribute = Enum.Parse<TargetAttribute>(TargetAttribute.SelectedItem),
                Hurts = Hurts.Checked,
                IsTemporary = IsTemporary.Checked,
            };

            manager.PopState();
            OnClosing();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Move = null;
            manager.PopState();
            OnClosing();
        }

        private void OnClosing()
        {
            Closing?.Invoke(this, EventArgs.Empty);
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
