using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psilibrary.ShadowMonsters
{
    public class ShadowMonsterManager
    {
        public readonly Dictionary<string, MoveData> Moves = new();
        public readonly Dictionary<string, ShadowMonsterData> Monsters = new();
    }
}
