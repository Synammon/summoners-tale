using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.ShadowMonsters
{
    public enum Element 
    { 
        Normal = 0, 
        Fire = 1, 
        Water = 2, 
        Earth = 4, 
        Wind = 8, 
        Light = 16, 
        Dark = 32 
    }

    public class ShadowMonsterData
    {
        public const int Normal = 0;
        public const int Asleep = 1;
        public const int Confused = 2;
        public const int Poisoned = 4;
        public const int Paralyzed = 8;
        public const int Burn = 16;
        public const int Frozen = 32;
        
        public string Name { get; set; }
        public int Elements { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public Point Health { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefence { get; set; }
        public int Speed { get; set; }
        public int Accuracy { get; set; }
        public int AttackMod { get; set; }
        public int DefenceMod { get; set; }
        public int SpecialAttackMod { get; set; }
        public int SpecialDefenceMod { get; set; }
        public int SpeedMod { get; set; }
        public int AccuracyMod { get; set; }
        public int Status { get; set; }
        public List<MoveData> Moves { get; set; }
    }
}
