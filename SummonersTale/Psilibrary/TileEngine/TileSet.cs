using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.TileEngine
{
    public class TileSheet
    {
        private Rectangle dimensions;
        private readonly Texture2D texture;
        private string name;
        private Rectangle[] sourceRectangles;

        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return texture; }
        }

        public Rectangle Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public string TextureName
        {
            get { return name; }
            set { name = value; }
        }

        [ContentSerializer]
        public Rectangle[] SourceRectangles
        {
            get { return sourceRectangles; }
            private set { sourceRectangles = value; }
        }

        private TileSheet()
        {
        }

        public TileSheet(Texture2D texture, string name, Rectangle dimensions)
        {
            this.texture = texture;
            this.name = name;
            this.dimensions = dimensions;

            SourceRectangles = new Rectangle[dimensions.X * dimensions.Y];

            for (int y = 0; y < dimensions.Y; y++)
            {
                for (int x = 0; x < dimensions.X; x++)
                {
                    SourceRectangles[y * dimensions.X + x] = new()
                    {
                        X = x * dimensions.Width,
                        Y = y * dimensions.Height,
                        Height = dimensions.Height,
                        Width = dimensions.Width
                    };
                }                
            }
        }
    }

    public class TileSet
    {
        #region Fields and Properties

        private List<TileSheet> tileSheets;

        #endregion

        #region Property Region

        public List<TileSheet> TileSheets
        {
            get { return tileSheets; }
            set { tileSheets = value; }
        }

        #endregion

        #region Constructor Region

        private TileSet()
        {
            tileSheets = new();
        }

        public TileSet(TileSheet sheet)
            : this()
        {
            tileSheets.Add(sheet);
        }

        #endregion

    }
}
