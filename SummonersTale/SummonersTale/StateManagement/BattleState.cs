using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Psilibrary.ConversationComponents;
using SummonersTale.Forms;
using SummonersTale.ShadowMonsters;
using System.Collections.Generic;
using System.Linq;

namespace SummonersTale.StateManagement
{
    public interface IBattleState
    {
        void SetShadowMonsters(ShadowMonster player, ShadowMonster enemy);
        void StartBattle();
        void ChangePlayerShadowMonster(ShadowMonster selected);
    }

    public class BattleState : BaseGameState, IBattleState
    {
        #region Field Region

        private ShadowMonster player;
        private ShadowMonster enemy;
        private GameScene combatScene;
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

        public ShadowMonster EnemyShadowMonster { get { return enemy; } }

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public BattleState(Game game)
            : base(game)
        {
            Game.Services.AddService<IBattleState>(this);

            playerRect = new(10, 90, 300, 300);
            enemyRect = new(TargetWidth - 310, 10, 300, 300);

            playerBorderRect = new(10, 10, 300, 75);
            enemyBorderRect = new(TargetWidth - 310, 320, 300, 75);

            healthSourceRect = new(10, 50, 290, 20);
            playerHealthRect = new(
                playerBorderRect.X + 12,
                playerBorderRect.Y + 52,
                286,
                16);
            enemyHealthRect = new(enemyBorderRect.X + 12, enemyBorderRect.Y + 52, 286, 16);

            playerMiniRect = new(playerBorderRect.X + 11, playerBorderRect.Y + 11, 28, 28);
            enemyMiniRect = new(enemyBorderRect.X + 11, enemyBorderRect.Y + 11, 28, 28);

            playerName = new(playerBorderRect.X + 55, playerBorderRect.Y + 5);
            enemyName = new(enemyBorderRect.X + 55, enemyBorderRect.Y + 5);
        }

        #endregion

        #region Method Region

        protected override void LoadContent()
        {
            if (combatScene == null)
            {
                combatBackground = new(GraphicsDevice, 1280, 720);
                combatBackground.Fill(Color.White);

                avatarBorder = new(GraphicsDevice, 300, 75);
                avatarHealth = new(GraphicsDevice, 300, 25);

                avatarBorder.Fill(Color.Green);
                avatarHealth.Fill(Color.Red);

                combatScene = new(Game, "", new List<SceneOption>());
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Xin.WasKeyReleased(Keys.P))
            {
                manager.PopState();
            }

            combatScene.Update(gameTime);

            if (Xin.WasKeyReleased(Keys.Space) ||
                Xin.WasKeyReleased(Keys.Enter) ||
                (Xin.WasMouseReleased(MouseButton.Left) &&
                combatScene.IsOver))
            {
                DamageState damageState = (DamageState)Game.Services.GetService<IDamageState>();

                manager.PushState(damageState);
                damageState.SetShadowMonsters(player, enemy);

                Move enemyMove = null;

                int move = random.Next(0, enemy.Moves.Count);
                enemyMove = (Move)enemy.Moves[move];

                damageState.SetMoves(
                    player.Moves.Where(x => x.Name == combatScene.OptionText).FirstOrDefault(),
                    enemyMove);
                damageState.Start();
            }

            Visible = true;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();

            SpriteBatch.Draw(combatBackground, Vector2.Zero, Color.White);
            combatScene.Draw(gameTime, SpriteBatch, combatBackground);

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
                playerName,
                Color.White);
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
            this.player = player;
            this.enemy = enemy;

            player.StartCombat();
            enemy.StartCombat();

            List<SceneOption> moves = new();

            if (combatScene == null)
            {
                LoadContent();
            }

            foreach (Move move in player.Moves)
            {
                SceneOption option = new(move.Name, move.Name, new SceneAction());
                moves.Add(option);
            }

            combatScene.Options = moves;
        }

        public void StartBattle()
        {
            player.StartCombat();
            enemy.StartCombat();
            playerHealth = 100f;
            enemyHealth = 100f;
        }

        public void ChangePlayerShadowMonster(ShadowMonster selected)
        {
            this.player = selected;

            List<SceneOption> moves = new();

            foreach (Move move in player.Moves)
            {
                SceneOption option = new(move.Name, move.Name, new SceneAction());
                moves.Add(option);
            }

            combatScene.Options = moves;
        }

        #endregion
    }
}
