using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.Creatures
{
    internal class Cyclops : Goblinoid
    {
        public Cyclops(string name, List<int> stats, List<Attacks> attacks) : base(name, stats, attacks)
        {
        }
    }
}
