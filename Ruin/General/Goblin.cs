using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.General
{
    internal class Goblin : Creatures
    {
        public List<string> bossNames = new List<string> { "Grimbrog", "Throghul", "Uzgoth", "Morgraith", "Skuldran", "Foulmaw", "Druknar", "Grimfang", "Blightwraith", "Vorthag", "Kruldak", "Gharnûl", "Morthûm", "Vulgrok", "Drelgath", "Kragmog", "Gorthûn", "Zulgarth", "Vrognir", "Ghûldren" };
        public Goblin(string name, List<Attack>? attacks, int ac, int maxhp, int curhp, List<Status>? status, int expvalue): base(name, ac, maxhp, curhp, status)
        {
            Random rnd = new Random();
            this.name = name;
            this.attacks = AttackLibrary.GoblinAttacks;
            this.ac = ac;
            this.maxhp = maxhp;
            this.curhp = curhp;
            this.status = status;
            this.expvalue = expvalue;
        }

    }
}
