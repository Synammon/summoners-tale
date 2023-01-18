using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;

namespace SummonersTale
{
    public static class ExtentionMethods
    {
        public static Rectangle Scale(this Rectangle r, Vector2 scale)
        {
            Rectangle scaled = new(
                (int)(r.X * scale.X),
                (int)(r.Y * scale.Y),
                (int)(r.Width * scale.X),
                (int)(r.Height * scale.Y));

            return scaled;
        }
    }
}
