﻿using System;
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
        public Goblinoid(string name, List<int> stats, List<Attacks> attacks) : base(name, stats, attacks)
        {
        }
        public static int GenerateAc()
        {
            //TODO: Get Dex bonus then add +1 for armor
            return 0;
        }

    }
}
