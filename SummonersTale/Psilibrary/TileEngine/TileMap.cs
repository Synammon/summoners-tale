using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.TileEngine
{
    public class TileMap
    {
        #region Field Region

        string mapName;
        List<ILayer> layers = new();

        int mapWidth;
        int mapHeight;

        List<TileSet> tileSets = new();

        #endregion

        #region Property Region

        [ContentSerializer(Optional = true)]
        public string MapName
        {
            get { return mapName; }
            private set { mapName = value; }
        }

        [ContentSerializer]
        public List<TileSet> TileSets
        {
            get { return tileSets; }
            set { tileSets = value; }
        }

        [ContentSerializer]
        public List<ILayer> Layers
        {
            get { return layers; }
            set { layers = value; }
        }

        [ContentSerializer]
        public int MapWidth
        {
            get { return mapWidth; }
            private set { mapWidth = value; }
        }

        [ContentSerializer]
        public int MapHeight
        {
            get { return mapHeight; }
            private set { mapHeight = value; }
        }

        public int WidthInPixels
        {
            get { return mapWidth * Engine.TileWidth; }
        }

        public int HeightInPixels
        {
            get { return mapHeight * Engine.TileHeight; }
        }

        #endregion

        #region Constructor Region

        private TileMap()
        {
        }

        private TileMap(List<TileSet> tileSets, string mapName)
            : this()
        {
            this.tileSets = tileSets;
            this.mapName = mapName;
        }

        public TileMap(
            List<TileSet> tileSets,
            List<ILayer> layers,
            string mapName)
            : this(tileSets, mapName)
        {
            this.layers = layers;
            this.tileSets = tileSets;

            TileLayer layer = (TileLayer)layers.Where(x => x is TileLayer).FirstOrDefault();

            mapWidth = layer.Width;
            mapHeight = layer.Height;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            foreach (ILayer layer in this.layers) 
            { 
                layer.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera, bool debug = false)
        {
            if (WidthInPixels >= Engine.TargetWidth || debug)
            {
                spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    camera.Transformation);
            }
            else
            {
                Matrix m = Matrix.CreateTranslation(
                    new Vector3((Engine.TargetWidth) / 2, (Engine.TargetHeight - HeightInPixels) / 2, 0));
                spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    m);
            }

            foreach (ILayer layer in this.layers)
            {
                if (layer is TileLayer || layer is CharacterLayer)
                    layer.Draw(spriteBatch, camera, TileSets);
            }

            spriteBatch.End();
        }

        public void AddLayer(ILayer layer)
        {
            this.layers.Add(layer); 
        }

        public bool PlayerCollides(Rectangle nextPotition)
        {
            CharacterLayer layer = layers.Where(x => x is CharacterLayer).FirstOrDefault() as CharacterLayer;

            if (layer != null)
            {
                foreach (var character in layer.Characters)
                {
                    Rectangle rectangle = new(
                        new(
                            character.Tile.X * Engine.TileWidth, 
                            character.Tile.Y * Engine.TileHeight),
                        new(
                            Engine.TileWidth, 
                            Engine.TileHeight));
                    if (rectangle.Intersects(nextPotition))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion
    }
}
