using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.ShadowMonsters
{
    public enum TargetType { Self, Enemy }
    public enum TargetAttribute 
    { 
        Health, 
        Attack, 
        Defence, 
        SpecialAttack, 
        SpecialDefense, 
        Speed,
        Accuracy 
    }

    public class MoveData
    {
        public string Name { get; set; }
        public int Elements { get; set; }
        public int Level { get; set; }
        public TargetType Target { get; set; }
        public TargetAttribute TargetAttribute { get; set; }
        public Point Mana { get; set; }
        public Point Range { get; set; }
        public int Status { get; set; }
        public int Power { get; set; }
        public bool Hurts { get; set; }
        public bool IsTemporary { get; set; }
    }
}
