using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psilibrary.ShadowMonsters;
using SummonersTale;
using SummonersTale.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterEditor.Forms
{
    public class MainForm : Form
    {
        private ListBox _movesList;
        private ListBox _monstersList;
        private Button _addMoveButton;
        private Button _editMoveButton;
        private Button _deleteMoveButton;
        private Button _addMonsterButton;
        private Button _editMonsterButton;
        private Button _deleteMonsterButton;

        private readonly ShadowMonsterManager shadowMonsterManager = new();

        public MainForm(Game game, Vector2 position, Point size) : base(game, position, size)
        {
            Position = new(0, 0);

            Size = new(_graphicsDevice.PreferredBackBufferWidth, _graphicsDevice.PreferredBackBufferHeight);
            Bounds = new(0, 0, _graphicsDevice.PreferredBackBufferWidth, _graphicsDevice.PreferredBackBufferHeight);
            FullScreen = true;

            Texture2D texture2D = new(GraphicsDevice, 100, 100);
            texture2D.Fill(new Color(240, 240, 240));

            PictureBox bg = new(GraphicsDevice, texture2D, Bounds);
            Background = bg;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Label movesLabel = new()
            {
                Text = "Moves",
                Color = Color.Black
            };

            Controls.Add(movesLabel);

            _movesList = new(GraphicsDevice,
                Game.Content.Load<Texture2D>(@"GUI/DownButton"),
                Game.Content.Load<Texture2D>(@"GUI/UpButton"),
                new(800, 1000))
            {
                Position = new(0,50)
            };

            Controls.Add(_movesList);

            _addMoveButton = new(
                Game.Content.Load<Texture2D>(@"GUI/Button"),
                ButtonRole.Menu)
            {
                Color = Color.Black,
                Text = "Add",
                Position = new(810, 50),
            };

            Controls.Add(_addMoveButton);

            _editMoveButton = new(
                Game.Content.Load<Texture2D>(@"GUI/Button"),
                ButtonRole.Menu)
            {
                Color = Color.Black,
                Text = "Edit",
                Position = new(810, 100),
            };

            Controls.Add(_editMoveButton);

            _deleteMoveButton = new(
                Game.Content.Load<Texture2D>(@"GUI/Button"),
                ButtonRole.Menu)
            {
                Color = Color.Black,
                Text = "Remove",
                Position = new(810, 150),
            };

            Controls.Add(_deleteMoveButton);

            Label monstersLabel = new()
            {
                Text = "Monsters",
                Color = Color.Black,
                Position = new(930, 0)
            };

            Controls.Add(monstersLabel);

            _monstersList = new(GraphicsDevice,
                Game.Content.Load<Texture2D>(@"GUI/DownButton"),
                Game.Content.Load<Texture2D>(@"GUI/UpButton"),
                new(800, 1000))
            {
                Position = new(930, 50)
            };

            Controls.Add(_monstersList);

            _addMonsterButton = new(
                Game.Content.Load<Texture2D>(@"GUI/Button"),
                ButtonRole.Menu)
            {
                Color = Color.Black,
                Text = "Add",
                Position = new(1750, 50),
            };

            Controls.Add(_addMonsterButton);

            _editMonsterButton = new(
                Game.Content.Load<Texture2D>(@"GUI/Button"),
                ButtonRole.Menu)
            {
                Color = Color.Black,
                Text = "Edit",
                Position = new(1750, 100),
            };

            Controls.Add(_editMonsterButton);

            _deleteMonsterButton = new(
                Game.Content.Load<Texture2D>(@"GUI/Button"),
                ButtonRole.Menu)
            {
                Color = Color.Black,
                Text = "Remove",
                Position = new(1750, 150),
            };

            Controls.Add(_deleteMonsterButton);

            _addMoveButton.Click += AddMoveButton_Click;
            _editMoveButton.Click += EditMoveButton_Click;
            _deleteMoveButton.Click += DeleteMoveButton_Click;

            _addMonsterButton.Click += AddMonsterButton_Click;
            _editMonsterButton.Click += EditMonsterButton_Click;
            _deleteMonsterButton.Click += DeleteMonsterButton_Click;
        }

        private void DeleteMoveButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EditMoveButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddMoveButton_Click(object sender, EventArgs e)
        {
            MoveForm frm = new(Game, new((1920 - 600) / 2, (1000 - 800) / 2), new(600, 800));
            manager.PushState(frm);
            this.Visible = true;
            frm.Closing += Frm_Closing;
        }

        private void Frm_Closing(object sender, EventArgs e)
        {
            if (sender is MoveForm form && form.Move != null)
            {
                shadowMonsterManager.Moves.Add(form.Move.Name, form.Move);
                _movesList.Items.Add(form.Move.Name);
            }
        }

        private void DeleteMonsterButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EditMonsterButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddMonsterButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Control c in Controls)
            {
                c.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
