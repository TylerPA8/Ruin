using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.Creatures
{
    internal class Ogre : Goblinoid
    {
        public Ogre(string name, List<int> stats, List<Attacks> attacks) : base(name, stats, attacks)
        {
        }
    }
}
