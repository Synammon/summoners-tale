using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Psilibrary.ShadowMonsters;
using SummonersTale.SpriteClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SummonersTale.ShadowMonsters
{
    public class ShadowMonster : ShadowMonsterData
    {
        public Texture2D Sprite;
        public List<MoveData> UnlockedMoves { get; protected set; } = new();
        public List<MoveData> LockedMoves { get; protected set; } = new();

        private string[] assignedMoves = new string[4];

        public ShadowMonster()
        {
        }

        public void LoadContent(ContentManager Content)
        {
            Sprite = Content.Load<Texture2D>(string.Format("ShadowMonsterSprites/{0}", Name));
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            
            Type type = typeof(ShadowMonster);

            bool first = true;

            foreach (var property in type.GetProperties())
            {
                if (!first)
                {
                    sb.Append(',');
                }
                else
                {
                    first = false;
                }

                if (property.Name != "Health" && !property.Name.Contains("Moves"))
                {
                    sb.Append(
                        string.Format("{0}={1}",
                        property.Name,
                        type.GetProperty(property.Name).GetValue(this).ToString()));
                }
                else if (property.Name == "Health")
                {
                    sb.Append($"Health={Health.X}:{Health.Y}");
                }
                else if (property.Name.Contains("Moves"))
                {
                    // Serialize moves to string here
                    sb.Append($"{property.Name}=");

                    if (type.GetProperty(property.Name).GetValue(this) is List<MoveData> moveData)
                    {
                        if (moveData != null)
                        { 
                            bool firstMove = true;

                            foreach (var move in moveData)
                            {
                                if (!firstMove)
                                {
                                    sb.Append('&');
                                }
                                else
                                { 
                                    firstMove = false; 
                                }

                                sb.Append($"[{move.ToString()}]");
                            }
                        }
                    }
                }
            }
            
            return sb.ToString();
        }

        public static ShadowMonster FromString(string value)
        {
            ShadowMonster monster = new();
            Type type = typeof(ShadowMonster);

            string[] parts = value.Split(',');

            foreach (string part in parts)
            {
                string[] attributes = part.Split('=');
                
                if (attributes.Length < 2 || string.IsNullOrWhiteSpace(attributes[1])) 
                { 
                    continue; 
                }

                PropertyInfo property = type.GetProperty(attributes[0]);

                if (property != null && !attributes[0].Contains("Moves") && attributes[0] != "Health")

                {
                    if (!int.TryParse(attributes[1], out int converted))
                    {
                        property.SetValue(monster, attributes[1], null);
                    }
                    else
                    {
                        property.SetValue(monster, converted, null);
                    }
                }
                else if (property != null && (attributes[0] == "Health"))
                {
                    string[] health = attributes[1].Split(":");

                    if (health.Length == 2) 
                    { 
                        if (int.TryParse(health[0], out int currentHealth) && int.TryParse(health[1], out int maxHealth))
                        {
                            monster.Health = new(currentHealth, maxHealth);
                        }
                    }
                }
                else if (property != null && (attributes[0].Contains("Moves")))
                {
                    property.SetValue(monster, new List<MoveData>(), null);
                    string[] moveString = attributes[1].Split('&');
                    foreach (string move in moveString)
                    {
                        ((List<MoveData>)property.GetValue(monster)).Add(Move.FromString(move));
                    }
                }
            }

            return monster;
        }
    }
}
