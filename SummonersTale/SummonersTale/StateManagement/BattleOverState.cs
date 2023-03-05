using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SummonersTale.Forms;
using SummonersTale.ShadowMonsters;

namespace SummonersTale.StateManagement
{
    public interface IBattleOverState
    {
        void SetShadowMonsters(ShadowMonster player, ShadowMonster enemy);
    }

    public class BattleOverState : BaseGameState, IBattleOverState
    {
        #region Field Region

        private ShadowMonster player;
        private ShadowMonster enemy;
        private Texture2D combatBackground;
        private Rectangle playerRect;
        private Rectangle enemyRect;
        private Rectangle playerBorderRect;
        private Rectangle enemyBorderRect;
        private Rectangle playerMiniRect;
        private Rectangle enemyMiniRect;
        private Rectangle playerHealthRect;
        private Rectangle enemyHealthRect;
        private Rectangle healthSourceRect;
        private Vector2 playerName;
        private Vector2 enemyName;
        private float playerHealth;
        private float enemyHealth;
        private Texture2D avatarBorder;
        private Texture2D avatarHealth;
        private readonly string[] battleState;
        private Vector2 battlePosition;
        private bool levelUp;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public BattleOverState(Game game)
            : base(game)
        {
            Game.Services.AddService<IBattleOverState>(this);

            battleState = new string[3];

            battleState[0] = "The battle was won!";
            battleState[1] = " gained ";
            battleState[2] = "Continue";

            battlePosition = new Vector2(25, 475);

            playerRect = new Rectangle(10, 90, 300, 300);
            enemyRect = new Rectangle(game.Window.ClientBounds.Width - 310, 10, 300, 300);

            playerBorderRect = new Rectangle(10, 10, 300, 75);
            enemyBorderRect = new Rectangle(game.Window.ClientBounds.Width - 310, 320, 300, 75);

            healthSourceRect = new Rectangle(10, 50, 290, 20);
            playerHealthRect = new Rectangle(playerBorderRect.X + 12, playerBorderRect.Y + 52, 286, 16);
            enemyHealthRect = new Rectangle(enemyBorderRect.X + 12, enemyBorderRect.Y + 52, 286, 16);

            playerMiniRect = new Rectangle(playerBorderRect.X + 11, playerBorderRect.Y + 11, 28, 28);
            enemyMiniRect = new Rectangle(enemyBorderRect.X + 11, enemyBorderRect.Y + 11, 28, 28);

            playerName = new Vector2(playerBorderRect.X + 55, playerBorderRect.Y + 5);
            enemyName = new Vector2(enemyBorderRect.X + 55, enemyBorderRect.Y + 5);
        }

        #endregion

        #region Method Region

        protected override void LoadContent()
        {
            combatBackground = new Texture2D(GraphicsDevice, 1280, 720);
            Color[] buffer = new Color[1280 * 720];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.White;
            }

            combatBackground.SetData(buffer);

            avatarBorder = new Texture2D(GraphicsDevice, 300, 75);
            avatarHealth = new Texture2D(GraphicsDevice, 300, 25);

            buffer = new Color[300 * 75];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.Green;
            }

            avatarBorder.SetData(buffer);

            buffer = new Color[300 * 25];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.Red;
            }

            avatarHealth.SetData(buffer);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Xin.WasKeyReleased(Keys.Space) || Xin.WasKeyReleased(Keys.Enter))
            {
                if (levelUp)
                {
                    LevelUpState levelUpState = Game.Services.GetService<LevelUpState>();
                    manager.PushState(levelUpState);
                    levelUpState.SetShadowMonster(player);

                    this.Visible = true;
                }
                else if (Player.Alive())
                {
                    manager.PopState();
                    manager.PopState();
                }
                else
                {
                    manager.PopState();
                    manager.PopState();
                    // should warp to a location since the player has no shadow monsters
                    // with no access to the world it is hard to say where to warp to
                    // at this time. Will handle this in a future tutorial
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 position = battlePosition;

            base.Draw(gameTime);

            SpriteBatch.Begin();

            SpriteBatch.Draw(combatBackground, Vector2.Zero, Color.White);

            for (int i = 0; i < 2; i++)
            {
                SpriteBatch.DrawString(
                    ControlManager.SpriteFont,
                    battleState[i],
                    position,
                    Color.Black);
                position.Y += ControlManager.SpriteFont.LineSpacing;
            }

            SpriteBatch.DrawString(
                ControlManager.SpriteFont,
                battleState[2],
                position,
                Color.Red);

            SpriteBatch.Draw(player.Sprite, playerRect, Color.White);
            SpriteBatch.Draw(enemy.Sprite, enemyRect, Color.White);

            SpriteBatch.Draw(avatarBorder, playerBorderRect, Color.White);

            playerHealth = (float)player.Health.X / (float)player.Health.Y;
            MathHelper.Clamp(playerHealth, 0f, 1f);
            playerHealthRect.Width = (int)(playerHealth * 286);

            SpriteBatch.Draw(avatarHealth, playerHealthRect, healthSourceRect, Color.White);

            SpriteBatch.Draw(avatarBorder, enemyBorderRect, Color.White);

            enemyHealth = (float)enemy.Health.X / (float)enemy.Health.Y;
            MathHelper.Clamp(enemyHealth, 0f, 1f);
            enemyHealthRect.Width = (int)(enemyHealth * 286);

            SpriteBatch.Draw(avatarHealth, enemyHealthRect, healthSourceRect, Color.White);
            SpriteBatch.DrawString(
                ControlManager.SpriteFont,
                player.Name,
                playerName, Color.
                White);
            SpriteBatch.DrawString(
                ControlManager.SpriteFont,
                enemy.Name,
                enemyName,
                Color.White);

            SpriteBatch.Draw(player.Sprite, playerMiniRect, Color.White);
            SpriteBatch.Draw(enemy.Sprite, enemyMiniRect, Color.White);

            SpriteBatch.End();
        }

        public void SetShadowMonsters(ShadowMonster player, ShadowMonster enemy)
        {
            levelUp = false;
            this.player = player;
            this.enemy = enemy;


            long expGained;
            if (player.Health.X >= 0)
            {
                expGained = player.WinBattle(enemy);

                battleState[0] = player.Name + " has won the battle!";
                battleState[1] = player.Name + " has gained " + expGained + " experience";

                if (player.CheckLevelUp())
                {
                    battleState[1] += " and gained a level!";

                    foreach (Move move in player.LockedMoves)
                    {
                        if (player.Level >= move.Level)
                        {
                            player.UnlockedMoves.Add(move);
                            battleState[1] += " " + move.Name + " was unlocked!";
                        }
                    }

                    levelUp = true;
                }
                else
                {
                    battleState[1] += ".";
                }
            }
            else
            {
                battleState[0] = player.Name + " has lost the battle.";
            }
        }

        #endregion
    }
}
