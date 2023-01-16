using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psilibrary.TileEngine;

namespace SummonersTale.Forms
{
    public class MapDisplay : Control
    {
        private TileMap _tileMap;
        private readonly int _width;
        private readonly int _height;
        private Tile _selectedTile;
        private GameTime _gameTime;

        public Point Tile { get; set; }
        public bool MouseOver { get; set; }

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        public MapDisplay(TileMap map, int width, int height)
        {
            Position = new(0, 20);
            Size = new(width * Engine.TileWidth, height * Engine.TileHeight);
            _tileMap = map;

            _width = width;
            _height = height;

            _tileMap = map;
        }

        public void SetMap(TileMap map)
        {
            _tileMap = map;
            _selectedTile = new(0, 0);
            Engine.Camera.Position = new(0, 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_tileMap != null)
            {
                _tileMap.Draw(new(), spriteBatch, Engine.Camera, true);

                spriteBatch.Begin();
                spriteBatch.DrawString(ControlManager.SpriteFont, "(" + Tile.X + ", " + Tile.Y + ")", Vector2.Zero, Color.Red);
                spriteBatch.End();
            }
        }

        public override void HandleInput()
        {
            Rectangle r = new(Helper.V2P(Position), Helper.V2P(Size));
            MouseOver = r.Contains(Xin.MouseAsPoint);

            Vector2 position = new()
            {
                X = Xin.MouseAsPoint.X + Engine.Camera.Position.X,
                Y = Xin.MouseAsPoint.Y + Engine.Camera.Position.Y,
            };

            Tile = Engine.VectorToCell(position);

            if (MouseOver)
            {
                Vector2 vector2 = new();

                if (new Rectangle(0, 0, 128, 1080).Contains(Xin.MouseAsPoint))
                {
                    vector2.X = -1;
                }

                if (new Rectangle(0, 0, 1280, 128).Contains(Xin.MouseAsPoint))
                {
                    vector2.Y = -1;
                }

                if (new Rectangle(1280 - 128, 0, 128, 1080).Contains(Xin.MouseAsPoint))
                {
                    vector2.X = 1;
                }

                if (new Rectangle(0, 1080 - 128, 1280, 128).Contains(Xin.MouseAsPoint))
                {
                    vector2.Y = 1;
                }

                if (vector2 != Vector2.Zero)
                    vector2.Normalize();

                Engine.Camera.Position = 
                    Engine.Camera.Position + vector2 * 160f * (float)_gameTime.ElapsedGameTime.TotalSeconds;

                LockCamera();
            }
        }


        private void LockCamera()
        {
            float y = MathHelper.Clamp(Engine.Camera.Position.Y, 0, _tileMap.HeightInPixels - _height);
            float x = MathHelper.Clamp(Engine.Camera.Position.X, 0, _tileMap.WidthInPixels - _width);

            Engine.Camera.Position = new(x, y);
        }

        public override void Update(GameTime gameTime)
        {
            _gameTime = gameTime;

            //if (!HasFocus) return;

            if (_tileMap != null)
            {
                _tileMap.Update(gameTime);
                HandleInput();
                Engine.Camera.LockCamera(_tileMap);
            }
        }
    }
}
