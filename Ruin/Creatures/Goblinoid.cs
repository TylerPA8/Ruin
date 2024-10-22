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
        public List<string> bossNames = new List <string> { "Grimbrog", "Throghul", "Uzgoth", "Morgraith", "Skuldran", "Foulmaw", "Druknar", "Grimfang", "Blightwraith", "Vorthag", "Kruldak", "Gharnûl", "Morthûm", "Vulgrok", "Drelgath", "Kragmog", "Gorthûn", "Zulgarth", "Vrognir", "Ghûldren" };
        public Goblinoid(string name, int maxhp, int curhp, int ac, List<int> stats, List<Utilities.Attacks> attacks) : base(name, maxhp, curhp, ac, stats, attacks)
        {
        }
        public Goblinoid()
        {
            Name = Utilities.generateName();
            Stats = Utilities.generatestatarray();
            Maxhp = Utilities.generateMaxhp();
            Curhp = Maxhp;
            Ac = Utilities.generateAc();
            Attacks = Utilities.generateAttacks();
        }
    }
}
