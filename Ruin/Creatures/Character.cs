using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.Creatures
{
    internal class Character : Creature
    {
        public Character(string name, List<int> stats, int maxhp,int ac, List<Attacks> attacks) : base(name, stats, maxhp, ac, attacks)
        {
        }
    }
}
