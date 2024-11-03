using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.General
{
    internal class Cyclops : Goblinoid
    {
        public Cyclops(List<int> stats, List<Attack> attacks) : base("Cyclops", stats, attacks)
        {
        }
    }
}
