using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psilibrary.Characters;
using Psilibrary.ConversationComponents;
using Psilibrary.TileEngine;
using SummonersTale.Characters;
using SummonersTale.SpriteClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SummonersTale.StateManagement
{
    public interface IGamePlayState
    {
        GameState GameState { get; }
        void NewGame();
    }

    public class GamePlayState : BaseGameState, IGamePlayState
    {
        private TileMap _tileMap;
        private Camera _camera;        
        private AnimatedSprite sprite;
        private Button upButton, downButton, leftButton, rightButton;
        private bool inMotion = false;
        private Rectangle collision = new();
        private float speed;
        private Vector2 motion;
        private IConversationManager _conversationManager;
        private IConversationState _conversationState;

        public GameState GameState => this;

        public GamePlayState(Game game) : base(game)
        {
            Game.Services.AddService((IGamePlayState)this);
        }

        public override void Initialize()
        {
            Engine.Reset(new(0, 0, 1280, 720), 32, 32);
            _camera = new();
            motion = new();
            speed = 96;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _conversationState = Game.Services.GetService<IConversationState>();
            _conversationManager = Game.Services.GetService<IConversationManager>();

            Texture2D texture = Game.Content.Load<Texture2D>(@"Tiles/OverWorld");

            TileSheet sheet = new(texture, "base", new(40, 36, 16, 16));
            List<TileSet> tilesets = new();
            tilesets.Add(new(sheet));

            TileLayer layer = new(100, 100, 0, 0);
            List<ILayer> layers= new();
            layers.Add(layer);

            _tileMap = new(tilesets, layers, "test");
            
            Dictionary<string, Animation> animations = new();

            Animation animation = new(3, 32, 32, 0, 0) { CurrentFrame = 0, FramesPerSecond = 8 };
            animations.Add("walkdown", animation);

            animation = new(3, 32, 32, 0, 32) { CurrentFrame = 0, FramesPerSecond = 8 };
            animations.Add("walkleft", animation);

            animation = new(3, 32, 32, 0, 64) { CurrentFrame = 0, FramesPerSecond = 8 };
            animations.Add("walkright", animation);

            animation = new(3, 32, 32, 0, 96) { CurrentFrame = 0, FramesPerSecond = 8 };
            animations.Add("walkup", animation);

            texture = Game.Content.Load<Texture2D>(@"CharacterSprites/femalepriest");

            sprite = new(texture, animations)
            {
                CurrentAnimation = "walkdown",
                IsActive = true,
                IsAnimating = true,
            };

            texture = Game.Content.Load<Texture2D>(@"CharacterSprites/femalefighter");

            AnimatedSprite rio = new(texture, animations)
            {
                CurrentAnimation = "walkdown",
                IsAnimating = true,
            };

            CharacterLayer chars = new();

            chars.Characters.Add(
                new Villager(rio, new(10, 10))
                {
                    Name = "Rio",
                    Position = new(320, 320),
                    Visible = true,
                    Enabled = true,
                    Conversation = "Rio"
                });

            _tileMap.AddLayer(chars);

            rightButton = new(Game.Content.Load<Texture2D>("GUI/g21245"), ButtonRole.Menu)
            {
                Position = new(80, Settings.BaseHeight - 80),
                Size = new(32, 32),
                Text = "",
                Color = Color.White,
            };

            rightButton.Down += RightButton_Down;
            Controls.Add(rightButton);

            upButton = new(Game.Content.Load<Texture2D>("GUI/g21263"), ButtonRole.Menu)
            {
                Position = new(48, Settings.BaseHeight - 48 - 64),
                Size = new(32, 32),
                Text = "",
                Color = Color.White,
            };

            upButton.Down += UpButton_Down;
            Controls.Add(upButton);

            downButton = new(Game.Content.Load<Texture2D>("GUI/g21272"), ButtonRole.Menu)
            {
                Position = new(48, Settings.BaseHeight - 48),
                Size = new(32, 32),
                Text = "",
                Color = Color.White,
            };

            downButton.Down += DownButton_Down;
            Controls.Add(downButton);

            leftButton = new(Game.Content.Load<Texture2D>("GUI/g22987"), ButtonRole.Menu)
            {
                Position = new(16, Settings.BaseHeight - 80),
                Size = new(32, 32),
                Text = "",
                Color = Color.White,
            };

            leftButton.Down += LeftButton_Down;

            Controls.Add(leftButton);
        }

        private void LeftButton_Down(object sender, EventArgs e)
        {
            if (!inMotion)
            {
                MoveLeft();
            }
        }

        private void MoveLeft()
        {
            motion = new(-1, 0);
            inMotion = true;
            sprite.CurrentAnimation = "walkleft";
            collision = new(
                (sprite.Tile.X - 2) * Engine.TileWidth,
                sprite.Tile.Y * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
        }

        private void RightButton_Down(object sender, EventArgs e)
        {
            if (!inMotion)
            {
                MoveRight();
            }
        }

        private void MoveRight()
        {
            motion = new(1, 0);
            inMotion = true;
            sprite.CurrentAnimation = "walkright";
            collision = new(
                (sprite.Tile.X + 2) * Engine.TileWidth,
                sprite.Tile.Y * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
        }

        private void DownButton_Down(object sender, EventArgs e)
        {
            if (!inMotion)
            {
                MoveDown();
            }
        }

        private void MoveDown()
        {
            motion = new(0, 1);
            Point newTile = sprite.Tile + new Point(0, 2);
            inMotion = true;
            sprite.CurrentAnimation = "walkdown";
            collision = new(
                newTile.X * Engine.TileWidth,
                newTile.Y * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
        }

        private void UpButton_Down(object sender, EventArgs e)
        {
            if (!inMotion)
            {
                MoveUp();
            }
        }

        private void MoveUp()
        {
            motion = new(0, -1);
            inMotion = true;
            sprite.CurrentAnimation = "walkup";
            collision = new(
                sprite.Tile.X * Engine.TileWidth,
                (sprite.Tile.Y - 2) * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
        }

        public override void Update(GameTime gameTime)
        {
            Controls.Update(gameTime);

            sprite.Update(gameTime);
            _tileMap.Update(gameTime);

            if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && !inMotion)
            {
                MoveLeft();
            }
            else if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && !inMotion)
            {
                MoveRight();
            }

            if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && !inMotion)
            {
                MoveUp();
            }
            else if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && !inMotion)
            {
                MoveDown();
            }

            if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.F) && !inMotion)
            {
                HandleConversation();
            }

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
            }
            else
            {
                inMotion = false;
                return;
            }

            if (!sprite.LockToMap(new(99 * Engine.TileWidth, 99 * Engine.TileHeight), ref motion))
            {
                inMotion = false;
                return;
            }

            Vector2 newPosition = sprite.Position + motion * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Rectangle nextPotition = new Rectangle(
                (int)newPosition.X,
                (int)newPosition.Y,
                Engine.TileWidth,
                Engine.TileHeight);

            if (nextPotition.Intersects(collision))
            {
                inMotion = false;
                motion = Vector2.Zero;
                sprite.Position = new((int)sprite.Position.X, (int)sprite.Position.Y);
                return;
            }

            if (_tileMap.PlayerCollides(nextPotition))
            {
                inMotion = false;
                motion = Vector2.Zero;
                return;
            }

            sprite.Position = newPosition;
            sprite.Tile = Engine.VectorToCell(newPosition);

            _camera.LockToSprite(sprite, _tileMap);

            base.Update(gameTime);
        }

        private void HandleConversation()
        {
            var layer = _tileMap.Layers.FirstOrDefault(x => x is CharacterLayer);

            if (layer is CharacterLayer characterLayer)
            {
                foreach (ICharacter c in characterLayer.Characters)
                {
                    if (c.Tile.X == sprite.Tile.X && Math.Abs(sprite.Tile.Y - c.Tile.Y) == 1 ||
                       (c.Tile.Y == sprite.Tile.Y && Math.Abs(sprite.Tile.X - c.Tile.X) == 1))
                    {
                        if (c is Villager villager)
                        {
                            _conversationState.SetConversation(Player, villager.Conversation);
                            manager.PushState((ConversationState)_conversationState);
                            //Conversation conversation = (Conversation)_conversationManager.GetConversation(villager.Conversation);
                            //if (conversation != null)
                            //{
                            //}
                        }
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

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

            spriteBatch.Begin();
            Controls.Draw(SpriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp);

            SpriteBatch.Draw(renderTarget, new Rectangle(new(0, 0), Settings.Resolution), Color.White);

            SpriteBatch.End();
        }

        public void NewGame()
        {
            LoadContent();
        }
    }
}
