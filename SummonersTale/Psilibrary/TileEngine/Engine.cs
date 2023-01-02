using Microsoft.Xna.Framework;

namespace Psilibrary.TileEngine
{
    public static class Engine
    {
        #region Field Region
        private static Rectangle viewPortRectangle;

        private static int tileWidth = 32;
        private static int tileHeight = 32;

        private static Camera camera;

        #endregion

        #region Property Region

        public static int TileWidth
        {
            get { return tileWidth; }
            set { tileWidth = value; }
        }

        public static int TileHeight
        {
            get { return tileHeight; }
            set { tileHeight = value; }
        }

        public static Rectangle ViewportRectangle
        {
            get { return viewPortRectangle; }
            set { viewPortRectangle = value; }
        }


        public static Camera Camera
        {
            get { return camera; }
        }

        #endregion

        #region Constructors

        static Engine()
        {
            ViewportRectangle = new Rectangle(0, 0, 1280, 720);
            camera = new Camera();

            TileWidth = 64;
            TileHeight = 64;
        }

        #endregion

        #region Methods

        public static Point VectorToCell(Vector2 position)
        {
            return new Point((int)position.X / tileWidth, (int)position.Y / tileHeight);
        }

        public static Vector2 VectorFromOrigin(Vector2 origin)
        {
            return new Vector2((int)origin.X / tileWidth * tileWidth, (int)origin.Y / tileHeight * tileHeight);
        }

        public static void Reset(Rectangle rectangle, int x, int y)
        {
            Engine.viewPortRectangle = rectangle;
            Engine.TileWidth = x;
            Engine.TileHeight = y;
        }

        #endregion
    }
}
