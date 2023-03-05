using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psilibrary.ShadowMonsters;
using SummonersTale.Forms;
using SummonersTale.ShadowMonsters;
using System;

namespace SummonersTale.StateManagement
{
    public enum CurrentTurn
    {
        Players, Enemies
    }

    public interface IDamageState
    {
        void SetShadowMonsters(ShadowMonster player, ShadowMonster enemy);
        void SetMoves(MoveData playerMove, MoveData enemyMove);
        void Start();
    }

    public class DamageState : BaseGameState, IDamageState
    {
        #region Field Region

        private CurrentTurn turn;
        private Texture2D combatBackground;
        private Rectangle playerRect;
        private Rectangle enemyRect;
        private TimeSpan cTimer;
        private TimeSpan dTimer;
        private ShadowMonster player;
        private ShadowMonster enemy;
        private MoveData playerMove;
        private MoveData enemyMove;
        private bool first;
        private Rectangle playerBorderRect;
        private Rectangle enemyBorderRect;
        private Rectangle playerMiniRect;
        private Rectangle enemyMiniRect;
        private Rectangle playerHealthRect;
        private Rectangle enemyHealthRect;
        private Rectangle healthSourceRect;
        private float playerHealth;
        private float enemyHealth;
        private Texture2D avatarBorder;
        private Texture2D avatarHealth;
        private Vector2 playerName;
        private Vector2 enemyName;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public DamageState(Game game)
            : base(game)
        {
            Game.Services.AddService<IDamageState>(this);

            playerRect = new Rectangle(10, 90, 300, 300);
            enemyRect = new Rectangle(Settings.BaseWidth - 310, 10, 300, 300);

            playerBorderRect = new Rectangle(10, 10, 300, 75);
            enemyBorderRect = new Rectangle(Settings. BaseWidth - 310, 320, 300, 75);

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

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            combatBackground = new Texture2D(GraphicsDevice, 1280, 720);
            combatBackground.Fill(Color.White);

            avatarBorder = new Texture2D(GraphicsDevice, 300, 75);
            avatarHealth = new Texture2D(GraphicsDevice, 300, 25);
            
            avatarBorder.Fill(Color.Green);
            avatarHealth.Fill(Color.Red);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if ((cTimer > TimeSpan.FromSeconds(3) ||
                enemy.Health.X <= 0 ||
                player.Health.X <= 0) &&
                dTimer > TimeSpan.FromSeconds(2))
            {
                if (enemy.Health.X <= 0 || player.Health.X <= 0)
                {
                    BattleOverState battleOverState = (BattleOverState)Game.Services.GetService<IBattleOverState>();
                    manager.PopState();
                    manager.PushState(battleOverState);
                    battleOverState.SetShadowMonsters(player, enemy);
                }
                else
                {
                    manager.PopState();
                }
            }
            else if (cTimer > TimeSpan.FromSeconds(2) && first && enemy.Health.X <= 0 && player.Health.X <= 0)
            {
                first = false;
                dTimer = TimeSpan.Zero;

                if (turn == CurrentTurn.Players)
                {
                    turn = CurrentTurn.Enemies;
                    enemy.ResolveMove(enemyMove, player);
                }
                else
                {
                    turn = CurrentTurn.Players;
                    player.ResolveMove(playerMove, enemy);
                }
            }
            else if (cTimer == TimeSpan.Zero)
            {
                dTimer = TimeSpan.Zero;

                if (turn == CurrentTurn.Players)
                {
                    player.ResolveMove(playerMove, enemy);
                }
                else
                {
                    enemy.ResolveMove(enemyMove, player);
                }
            }

            cTimer += gameTime.ElapsedGameTime;
            dTimer += gameTime.ElapsedGameTime;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();

            SpriteBatch.Draw(combatBackground, Vector2.Zero, Color.White);

            Vector2 location = new(25, 475);

            if (turn == CurrentTurn.Players)
            {
                SpriteBatch.DrawString(
                    ControlManager.SpriteFont,
                    player.Name + " uses " + playerMove.Name + ".",
                    location,
                    Color.Black);

                if (playerMove.Target == TargetType.Enemy && playerMove.Hurts)
                {
                    location.Y += ControlManager.SpriteFont.LineSpacing;

                    if (ShadowMonster.GetMoveModifier(playerMove, enemy) < 1f)
                    {
                        SpriteBatch.DrawString(
                            ControlManager.SpriteFont,
                            "It is not very effective.",
                            location,
                            Color.Black);
                    }
                    else if (ShadowMonster.GetMoveModifier(playerMove, enemy) > 1f)
                    {
                        SpriteBatch.DrawString(
                            ControlManager.SpriteFont,
                            "It is super effective.",
                            location,
                            Color.Black);
                    }
                }
            }
            else
            {
                SpriteBatch.DrawString(
                    ControlManager.SpriteFont,
                    "Enemy " + enemy.Name + " uses " + enemyMove.Name + ".",
                    location,
                    Color.Black);

                if (enemyMove.Target == TargetType.Enemy && playerMove.Hurts)
                {
                    location.Y += ControlManager.SpriteFont.LineSpacing;

                    if (ShadowMonster.GetMoveModifier(enemyMove, player) < 1f)
                    {
                        SpriteBatch.DrawString(
                            ControlManager.SpriteFont,
                            "It is not very effective.",
                            location,
                            Color.Black);
                    }
                    else if (ShadowMonster.GetMoveModifier(enemyMove, player) > 1f)
                    {
                        SpriteBatch.DrawString(
                            ControlManager.SpriteFont,
                            "It is super effective.",
                            location,
                            Color.Black);
                    }
                }
            }

            SpriteBatch.Draw(avatarBorder, playerBorderRect, Color.White);

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
            SpriteBatch.DrawString(ControlManager.SpriteFont, player.Name, playerName, Color.White);
            SpriteBatch.DrawString(ControlManager.SpriteFont, enemy.Name, enemyName, Color.White);

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin();
            SpriteBatch.Draw(renderTarget, new Rectangle(new Point(), Settings.Resolution), Color.White);
            spriteBatch.End();
        }

        public void SetShadowMonsters(ShadowMonster player, ShadowMonster enemy)
        {
            this.player = player;
            this.enemy = enemy;

            if (player.Speed + player.SpeedMod >= enemy.Speed + enemy.SpeedMod)
            {
                turn = CurrentTurn.Players;
            }
            else
            {
                turn = CurrentTurn.Enemies;
            }
        }

        public void SetMoves(MoveData playerMove, MoveData enemyMove)
        {
            this.playerMove = playerMove;
            this.enemyMove = enemyMove;
        }

        public void Start()
        {
            cTimer = TimeSpan.Zero;
            dTimer = TimeSpan.Zero;
            first = true;
        }

        #endregion
    }
}
