using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.TileEngine
{
    public interface ILayer
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, Camera camera, List<TileSet> tilesets);
    }
}
