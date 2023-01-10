using Microsoft.Xna.Framework;

namespace SummonersTale
{
    public static class Helper
    {
        public static Point V2P(Microsoft.Xna.Framework.Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static Vector2 NearestInt(Vector2 v)
        {
            return new Vector2((int)v.X, (int)v.Y);
        }
    }
}
