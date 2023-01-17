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
        TileLayer groundLayer;
        TileLayer edgeLayer;
        TileLayer buildingLayer;
        TileLayer decorationLayer;

        int mapWidth;
        int mapHeight;

        TileSet tileSet;

        #endregion

        #region Property Region

        [ContentSerializer(Optional = true)]
        public string MapName
        {
            get { return mapName; }
            private set { mapName = value; }
        }

        [ContentSerializer]
        public TileSet TileSet
        {
            get { return tileSet; }
            set { tileSet = value; }
        }

        [ContentSerializer]
        public TileLayer GroundLayer
        {
            get { return groundLayer; }
            set { groundLayer = value; }
        }

        [ContentSerializer]
        public TileLayer DecorationLayer
        {
            get { return decorationLayer; }
            set { decorationLayer = value; }
        }

        [ContentSerializer]
        public TileLayer EdgeLayer
        {
            get { return edgeLayer; }
            set { edgeLayer = value; }
        }

        [ContentSerializer]
        public TileLayer BuildingLayer
        {
            get { return buildingLayer; }
            set { buildingLayer = value; }
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

        private TileMap(TileSet tileSet, string mapName)
            : this()
        {
            this.tileSet = tileSet;
            this.mapName = mapName;
        }

        public TileMap(
            TileSet tileSet,
            TileLayer groundLayer,
            TileLayer edgeLayer,
            TileLayer buildingLayer,
            TileLayer decorationLayer,
            string mapName)
            : this(tileSet, mapName)
        {
            this.groundLayer = groundLayer;
            this.edgeLayer = edgeLayer;
            this.buildingLayer = buildingLayer;
            this.decorationLayer = decorationLayer;

            mapWidth = groundLayer.Width;
            mapHeight = groundLayer.Height;
        }

        #endregion

        #region Method Region

        public void SetGroundTile(int x, int y, int set, int index)
        {
            groundLayer.SetTile(x, y, set, index);
        }

        public Tile GetGroundTile(int x, int y)
        {
            return groundLayer.GetTile(x, y);
        }

        public void SetEdgeTile(int x, int y, int set, int index)
        {
            edgeLayer.SetTile(x, y, set, index);
        }

        public Tile GetEdgeTile(int x, int y)
        {
            return edgeLayer.GetTile(x, y);
        }

        public void SetBuildingTile(int x, int y, int set, int index)
        {
            buildingLayer.SetTile(x, y, set, index);
        }

        public Tile GetBuildingTile(int x, int y)
        {
            return buildingLayer.GetTile(x, y);
        }

        public void SetDecorationTile(int x, int y, int set, int index)
        {
            decorationLayer.SetTile(x, y, set, index);
        }

        public Tile GetDecorationTile(int x, int y)
        {
            return decorationLayer.GetTile(x, y);
        }

        public void FillEdges()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    edgeLayer.SetTile(x, y, -1, -1);
                }
            }
        }

        public void FillBuilding()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    buildingLayer.SetTile(x, y, -1, -1);
                }
            }
        }

        public void FillDecoration()
        {
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    decorationLayer.SetTile(x, y, -1, -1);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (groundLayer != null)
                groundLayer.Update(gameTime);

            if (edgeLayer != null)
                edgeLayer.Update(gameTime);

            if (buildingLayer != null)
                buildingLayer.Update(gameTime);

            if (decorationLayer != null)
                decorationLayer.Update(gameTime);
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

            if (groundLayer != null)
                groundLayer.Draw(gameTime, spriteBatch, tileSet, camera);

            if (edgeLayer != null)
                edgeLayer.Draw(gameTime, spriteBatch, tileSet, camera);

            if (decorationLayer != null)
                decorationLayer.Draw(gameTime, spriteBatch, tileSet, camera);

            if (buildingLayer != null)
                buildingLayer.Draw(gameTime, spriteBatch, tileSet, camera);

            spriteBatch.End();
        }

        #endregion
    }
}
