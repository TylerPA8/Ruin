using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.Creatures
{
    internal class Goblinoid : Creature
    {
        public StreamReader namesDoc = new StreamReader("C:\\Users\\tyler.paneitz\\OneDrive - Veterans United Home Loans\\Desktop\\Monster Names.txt");
        protected List<string> BossNames = namesDoc.Split(',').ToList();


        public Goblinoid(string name, int maxhp, int curhp, int ac, List<int> stats, List<Utilities.Attacks> attacks) : base(name, maxhp, curhp, ac, stats, attacks)
        {
        }
    }
}
