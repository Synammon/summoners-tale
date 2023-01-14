using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psilibrary.TileEngine;
using SummonersTale.SpriteClasses;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace SummonersTale.StateManagement
{
    public interface IGamePlayState
    {
        GameState GameState { get; }
    }

    public class GamePlayState : BaseGameState, IGamePlayState
    {
        private TileMap _tileMap;
        private Camera _camera;
        private AnimatedSprite sprite;

        public GameState GameState => this;

        public GamePlayState(Game game) : base(game)
        {
            Game.Services.AddService((IGamePlayState)this);
        }

        public override void Initialize()
        {
            Engine.Reset(new(0, 0, 1280, 720), 32, 32);
            _camera = new();

            base.Initialize();
        }
        protected override void LoadContent()
        {

            TileSheet sheet = new(content.Load<Texture2D>(@"Tiles/Overworld"), "test", new(40, 36, 16, 16));
            TileSet set = new(sheet);

            TileLayer ground = new(100, 100, 0, 0);
            TileLayer edge = new(100, 100, -1, -1);
            TileLayer building = new(100, 100, -1, -1);
            TileLayer decore = new(100, 100, -1, -1);
            
            for (int i = 0; i < 1000; i++)
            {
                edge.SetTile(random.Next(0, 100), random.Next(0, 100), 0, random.Next(0, 64));
            }

            _tileMap = new(set, ground, edge, building, decore, "test");

            Texture2D texture = content.Load<Texture2D>(@"CharacterSprites/femalepriest");

            Dictionary<string, Animation> animations = new();

            Animation animation = new(3, 32, 32, 0, 0) { CurrentFrame = 0, FramesPerSecond = 8 };
            animations.Add("walkdown", animation);

            animation = new(3, 32, 32, 0, 32) { CurrentFrame = 0, FramesPerSecond = 8 };
            animations.Add("walkleft", animation);

            animation = new(3, 32, 32, 0, 64) { CurrentFrame = 0, FramesPerSecond = 8 };
            animations.Add("walkright", animation);

            animation = new(3, 32, 32, 0, 96) { CurrentFrame = 0, FramesPerSecond = 8 };
            animations.Add("walkup", animation);

            sprite = new(texture, animations)
            {
                CurrentAnimation = "walkdown",
                IsActive = true,
                IsAnimating = true,
            };

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 motion = Vector2.Zero;

            if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                motion.X = -1;

                if (sprite.CurrentAnimation != "walkleft")
                {
                    sprite.CurrentAnimation = "walkleft";
                    sprite.ResetAnimation();
                }
            }
            else if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                motion.X = 1;

                if (sprite.CurrentAnimation != "walkright")
                {
                    sprite.CurrentAnimation = "walkright";
                    sprite.ResetAnimation();
                }
            }

            if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                motion.Y = -1;

                if (sprite.CurrentAnimation != "walkup")
                {
                    sprite.CurrentAnimation = "walkup";
                    sprite.ResetAnimation();
                }
            }
            else if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                motion.Y = 1;

                if (sprite.CurrentAnimation != "walkdown")
                {
                    sprite.CurrentAnimation = "walkdown";
                    sprite.ResetAnimation();
                }
            }

            if (motion != Vector2.Zero)
                motion.Normalize();

            if (!sprite.LockToMap(new(99 * Engine.TileWidth, 99 * Engine.TileHeight), ref motion)) return;

            sprite.Position += motion * 160 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _camera.LockToSprite(sprite, _tileMap);

            sprite.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _tileMap.Draw(gameTime, SpriteBatch, _camera, false);

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                _camera.Transformation);

            sprite.Draw(SpriteBatch);
            SpriteBatch.End();
        }
    }
}
