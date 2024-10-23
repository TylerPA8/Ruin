using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.Creatures
{
    internal class HobGoblin : Goblinoid
    {
        public HobGoblin(string name, List<int> stats, List<Attacks> attacks) : base(name, stats, attacks)
        {
        }
    }
}
