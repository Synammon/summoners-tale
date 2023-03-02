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
        public TargetType Target { get; set; }
        public TargetAttribute TargetAttribute { get; set; }
        public Point Mana { get; set; }
        public Point Range { get; set; }
        public int Status { get; set; }
        public bool Hurts { get; set; }
        public bool IsTemporary { get; set; }
    }
}

// "Animations=System.Collections.Generic.Dictionary`2[System.String,SummonersTale.SpriteClasses.Animation]UnlockedMoves=System.Collections.Generic.List`1[SummonersTale.ShadowMonsters.Move]LockedMoves=System.Collections.Generic.List`1[SummonersTale.ShadowMonsters.Move]Name=GoblinElements=0Level=1Experience=0Health=20:20Attack=5Defence=5SpecialAttack=5SpecialDefence=5Speed=5Accuracy=100Status=0Moves=System.Collections.Generic.List`1[Psilibrary.ShadowMonsters.MoveData])"