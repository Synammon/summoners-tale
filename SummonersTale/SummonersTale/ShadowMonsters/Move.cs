using Microsoft.Xna.Framework;
using Psilibrary.ShadowMonsters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace SummonersTale.ShadowMonsters
{
    public class Move : MoveData
    {
        protected static Random Random { get; set; } = new Random();

        public Move()
        { 
        }

        public virtual void Apply(ShadowMonster target)
        {
            int amount = Random.Next(Range.X, Range.Y);

            if (Hurts)
            {
                amount *= -1;
            }

            string propertyName = !IsTemporary ? TargetAttribute.ToString() : string.Format("{0}Mod",TargetAttribute.ToString());
            var property = target.GetType().GetProperty(propertyName);

            if (property.PropertyType != typeof(Point)) 
            {
                int value = (int)property.GetValue(this);
                property.SetValue(this, value + amount, null);
            }
            else
            {
                Point p = ((Point)(property.GetValue(this)));
                p.X += amount;

                if (p.X > p.Y)
                {
                    p.X = p.Y;
                }

                property.SetValue(this, p, null);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            Type moveType = typeof(Move);
            bool first = true;

            foreach (var property in moveType.GetProperties())
            {
                if (property.Name == "Random")
                {
                    continue;
                }

                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append('\t');
                }
                if (property.PropertyType != typeof(Point))
                {
                    object o = moveType.GetProperty(property.Name).GetValue(this);
                    sb.Append($"{property.Name}+{o.ToString()} ");
                }
                else if (property.PropertyType == typeof(Point))
                {
                    Point p = (Point)moveType.GetProperty(property.Name).GetValue(this);
                    sb.Append($"{property.Name}+{p.X}:{p.Y}");
                }
            }
            return sb.ToString();
        }

        public static Move FromString(string value)
        {
            Move move = new();
            Type moveType = typeof(Move);

            value = value.Replace("[", "");
            value = value.Replace("]", "");

            string[] parts = value.Split('\t');

            foreach (string part in parts)
            {
                string[] attributes = part.Split('+');

                if (moveType.GetProperty(attributes[0]).PropertyType != typeof(Point))
                {
                    if (attributes[0] != "Target" && attributes[0] != "TargetAttribute")
                    {
                        if (moveType.GetProperty(attributes[0]).PropertyType == typeof(string))
                        {
                            moveType.GetProperty(attributes[0]).SetValue(move, attributes[1], null);
                        }
                        else if (moveType.GetProperty(attributes[0]).PropertyType == typeof(Int32))
                        {
                            moveType.GetProperty(attributes[0]).SetValue(move, int.Parse(attributes[1]), null);
                        }
                    }

                    if (attributes[0] == "Target")
                    {
                        if (Enum.TryParse<TargetType>(attributes[1], out TargetType target))
                        {
                            moveType.GetProperty(attributes[0]).SetValue(move, target, null);
                        }
                    }

                    if (attributes[0] == "TargetAttribute")
                    {
                        if (Enum.TryParse<TargetAttribute>(attributes[1], out TargetAttribute target))
                        {
                            moveType.GetProperty(attributes[0]).SetValue(move, target, null);
                        }
                    }
                }

                if (moveType.GetProperty(attributes[0]).PropertyType == typeof(Point))
                {
                    string[] components = attributes[1].Split(':');
                    Point point;

                    _ = int.TryParse(components[0], out point.X);
                    _ = int.TryParse(components[1], out point.Y);

                    moveType.GetProperty(attributes[0]).SetValue(move, point, null);
                }
            }

            return move;
        }
    }
}
