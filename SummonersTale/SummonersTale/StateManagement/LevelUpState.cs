using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SummonersTale.Forms;
using SummonersTale.ShadowMonsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersTale.StateManagement
{
    public interface ILevelUpState
    {
        void SetShadowMonster(ShadowMonster playerShadowMonster);
    }

    public class LevelUpState : BaseGameState, ILevelUpState
    {
        #region Field Region

        private Rectangle destination;
        private int points;
        private int selected;
        private ShadowMonster player;
        private readonly Dictionary<string, int> attributes = new Dictionary<string, int>();
        private readonly Dictionary<string, int> assignedTo = new Dictionary<string, int>();
        private Texture2D levelUpBackground;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public LevelUpState(Game game)
            : base(game)
        {
            Game.Services.AddService<ILevelUpState>(this);

            attributes.Add("Attack", 0);
            attributes.Add("Defense", 0);
            attributes.Add("Speed", 0);
            attributes.Add("Health", 0);
            attributes.Add("Done", 0);

            foreach (string s in attributes.Keys)
                assignedTo.Add(s, 0);
        }

        #endregion

        #region Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            levelUpBackground = new Texture2D(GraphicsDevice, 500, 400);

            Color[] buffer = new Color[500 * 400];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.Gray;
            }

            levelUpBackground.SetData(buffer);

            destination = new Rectangle(
                (TargetWidth - levelUpBackground.Width) / 2,
                (TargetHeight - levelUpBackground.Height) / 2,
                levelUpBackground.Width,
                levelUpBackground.Height);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            int i = 0;
            string attribute = "";

            if (Xin.WasKeyReleased(Keys.Down) || Xin.WasKeyReleased(Keys.S))
            {
                selected++;

                if (selected >= attributes.Count)
                {
                    selected = attributes.Count - 1;
                }
            }
            else if (Xin.WasKeyReleased(Keys.Up) || Xin.WasKeyReleased(Keys.W))
            {
                selected--;

                if (selected < 0)
                {
                    selected = 0;
                }
            }

            if (Xin.WasKeyReleased(Keys.Space) || Xin.WasKeyReleased(Keys.Enter))
            {
                if (selected == 4 && points == 0)
                {
                    foreach (string s in assignedTo.Keys)
                    {
                        player.AssignPoint(s, assignedTo[s]);
                    }

                    manager.PopState();
                    manager.PopState();
                    manager.PopState();
                    return;
                }
            }

            int increment = 1;

            if ((Xin.WasKeyReleased(Keys.Right) || Xin.WasKeyReleased(Keys.D)) && points > 0)
            {
                foreach (string s in assignedTo.Keys)
                {
                    if (s == "Done")
                    {
                        return;
                    }

                    if (i == selected)
                    {
                        attribute = s;
                        break;
                    }

                    i++;
                }


                if (attribute == "Health")
                {
                    increment *= 5;
                }

                points--;
                assignedTo[attribute] += increment;

                if (points == 0)
                {
                    selected = 4;
                }
            }
            else if ((Xin.WasKeyReleased(Keys.Left) || Xin.WasKeyReleased(Keys.A)) && points <= 3)
            {
                foreach (string s in assignedTo.Keys)
                {
                    if (s == "Done")
                    {
                        return;
                    }

                    if (i == selected)
                    {
                        attribute = s;
                        break;
                    }

                    i++;
                }

                if (assignedTo[attribute] != attributes[attribute])
                {
                    if (attribute == "Health")
                    {
                        increment *= 5;
                    }

                    points++;
                    assignedTo[attribute] -= increment;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);


            SpriteBatch.Begin();
            SpriteBatch.Draw(levelUpBackground, destination, Color.White);

            Vector2 textPosition = new Vector2(destination.X + 5, destination.Y + 5);

            SpriteBatch.DrawString(ControlManager.SpriteFont, player.Name, textPosition, Color.White);
            textPosition.Y += ControlManager.SpriteFont.LineSpacing * 2;

            int i = 0;

            foreach (string s in attributes.Keys)
            {
                Color tint = Color.White;

                if (i == selected)
                    tint = Color.Red;

                if (s != "Done")
                {
                    SpriteBatch.DrawString(ControlManager.SpriteFont, s + ":", textPosition, tint);
                    textPosition.X += 125;

                    SpriteBatch.DrawString(ControlManager.SpriteFont, attributes[s].ToString(), textPosition, tint);
                    textPosition.X += 40;

                    SpriteBatch.DrawString(ControlManager.SpriteFont, assignedTo[s].ToString(), textPosition, tint);
                    textPosition.X = destination.X + 5;

                    textPosition.Y += ControlManager.SpriteFont.LineSpacing;
                }
                else
                {
                    SpriteBatch.DrawString(ControlManager.SpriteFont, "Done", textPosition, tint);
                    textPosition.Y += ControlManager.SpriteFont.LineSpacing * 2;
                }
                i++;
            }

            SpriteBatch.DrawString(
                ControlManager.SpriteFont,
                points.ToString() + " point left.",
                textPosition,
                Color.White);
            SpriteBatch.End();
        }

        public void SetShadowMonster(ShadowMonster playerShadowMonster)
        {
            player = playerShadowMonster;

            attributes["Attack"] = player.Attack;
            attributes["Defense"] = player.Defence;
            attributes["Speed"] = player.Speed;
            attributes["Health"] = player.Health.X;

            assignedTo["Attack"] = random.Next(0, 2);
            assignedTo["Defense"] = random.Next(0, 2);
            assignedTo["Speed"] = random.Next(0, 2);
            assignedTo["Health"] = random.Next(5, 11);

            points = 3;
            selected = 0;
        }

        #endregion
    }
}
