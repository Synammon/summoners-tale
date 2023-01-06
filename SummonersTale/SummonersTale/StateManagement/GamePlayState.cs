using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psilibrary.TileEngine;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace SummonersTale.StateManagement
{
    public class GamePlayState : BaseGameState
    {
        private TileMap _tileMap;
        private Camera _camera;

        public GamePlayState(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            Engine.Reset(new(0, 0, 1280, 720), 32, 32);
            _camera = new();

            base.Initialize();
        }
        protected override void LoadContent()
        {

            TileSheet sheet = new(content.Load<Texture2D>(@"Tiles/TX Tileset Grass"), "test", new(8, 8, 32, 32));
            TileSet set = new(sheet);

            TileLayer ground = new(100, 100, 0, 0);
            TileLayer edge = new(100, 100, -1, -1);
            TileLayer building = new(100, 100, -1, -1);
            TileLayer decore = new(100, 100, -1, -1);
            
            for (int i = 0; i < 1000; i++)
            {
                ground.SetTile(random.Next(0, 100), random.Next(0, 100), 0, random.Next(0, 64));
            }

            _tileMap = new(set, ground, edge, building, decore, "test");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 motion = Vector2.Zero;

            if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                motion.X = -1;
            }
            else if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                motion.X = 1;
            }

            if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                motion.Y = -1;
            }
            else if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                motion.Y = 1;
            }

            if (motion != Vector2.Zero)
                motion.Normalize();

            _camera.Position += motion * 240 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_camera.Position.X < 0)
            {
                _camera.Position = new(0, _camera.Position.Y);
            }

            if (_camera.Position.X > _tileMap.WidthInPixels - 1280)
            {
                _camera.Position = new(_tileMap.WidthInPixels - 1280, _camera.Position.Y);
            }

            if (_camera.Position.Y < 0)
            {
                _camera.Position = new(_camera.Position.X, 0);
            }

            if (_camera.Position.Y > _tileMap.HeightInPixels - 720)
            {
                _camera.Position = new(_camera.Position.X, _tileMap.HeightInPixels - 720);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _tileMap.Draw(gameTime, GameRef.SpriteBatch, _camera, false);
        }
    }
}
