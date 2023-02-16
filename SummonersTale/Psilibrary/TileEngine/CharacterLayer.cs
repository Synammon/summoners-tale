using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psilibrary.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.TileEngine
{
    public class CharacterLayer : ILayer
    {
        public List<ICharacter> Characters { get; private set; } = new();

        public CharacterLayer() { }

        public void Update(GameTime gameTime)
        {
            foreach (var character in Characters.Where(x => x.Enabled))
            {
                character.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, List<TileSet> tilesets)
        {
            foreach (var character in Characters.Where(x => x.Visible))
            {
                character.Draw(spriteBatch);
            }
        }
    }
}
