using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonersTale.ShadowMonsters;
using SummonersTale.SpriteClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SummonersTale
{
    public class Player : DrawableGameComponent
    {
        private SpriteBatch _spriteBatch;

        public Player(Game game, string name, bool gender, AnimatedSprite sprite)
            :base(game)
        {
            _spriteBatch = game.Services.GetService<SpriteBatch>();

            Name = name;
            Gender = gender;
            Sprite = sprite;
        }

        public string Name { get; private set; }
        public bool Gender { get; private set; }  
        public AnimatedSprite Sprite { get; private set; }

        public List<ShadowMonster> ShadowMonsters { get; private set; } = new();

        public List<ShadowMonster> BattleMonsters { get; private set; } = new();

        public override void Update(GameTime gameTime)
        {
            if (Sprite != null)
            {
                Sprite.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (Sprite != null)
            {
                _spriteBatch.Begin();
                Sprite.Draw(_spriteBatch);
                _spriteBatch.End();
            }
        }

        internal bool Alive()
        {
            return BattleMonsters.Where(x => x.Health.X > 0).Any();
        }
    }
}
