using Microsoft.Xna.Framework;
using SummonersTale.SpriteClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.TileEngine
{
    public class Camera
    {
        #region Field Region

        Vector2 position;
        float speed;

        #endregion

        #region Property Region

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = (float)MathHelper.Clamp(speed, 1f, 16f); }
        }

        public Matrix Transformation
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position, 0f));
            }
        }

        #endregion

        #region Constructor Region

        public Camera()
        {
            speed = 4f;
        }

        public Camera(Vector2 position)
        {
            speed = 4f;
            Position = position;
        }

        #endregion

        public void LockToSprite(AnimatedSprite sprite, TileMap map)
        {
            position.X = (sprite.Position.X + sprite.Width / 2)
                - (Engine.TargetWidth / 2);

            position.Y = (sprite.Position.Y + sprite.Height / 2)
                - (Engine.TargetHeight / 2);

            LockCamera(map);
        }


        public void LockCamera(TileMap map)
        {
            position.X = MathHelper.Clamp(position.X,
                0,
                map.WidthInPixels - Engine.ViewportRectangle.Width);

            position.Y = MathHelper.Clamp(position.Y,
                0,
                map.HeightInPixels - Engine.ViewportRectangle.Height);
        }
    }
}
