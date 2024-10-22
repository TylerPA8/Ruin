using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.Creatures
{
    internal class Cyclops : Goblinoid
    {
        public Cyclops(string name, int maxhp, int curhp, int ac, List<int> stats, List<Utilities.Attacks> attacks) : base(name, maxhp, curhp, ac, stats, attacks)
        {
        }
    }
}
