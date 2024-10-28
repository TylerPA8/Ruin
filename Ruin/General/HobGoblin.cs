using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.General
{
    internal class HobGoblin : Goblinoid
    {
        public HobGoblin(string name, List<int> stats, List<Attack> attacks) : base(name, stats, attacks)
        {
        }
    }
}
