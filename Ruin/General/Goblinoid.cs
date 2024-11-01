using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.General
{
    internal class Goblinoid : Creatures
    {
        public List<string> bossNames = new List <string> { "Grimbrog", "Throghul", "Uzgoth", "Morgraith", "Skuldran", "Foulmaw", "Druknar", "Grimfang", "Blightwraith", "Vorthag", "Kruldak", "Gharnûl", "Morthûm", "Vulgrok", "Drelgath", "Kragmog", "Gorthûn", "Zulgarth", "Vrognir", "Ghûldren" };
        public Goblinoid(string name, List<int> stats, List<Attack> attacks) : base(name, stats, attacks)
        {
        }
        public override int GenerateAc(int d)
        {
            return (11 + d);
        }

        public override List<int> GenerateStatArray()
        {
            //generates 4 stats between 6 and 14 and places them in an array. 
            Random rnd = new();
            int strength = rnd.Next(6, 13);
            int strMod = statArray[strength];
            int dexterity = rnd.Next(6, 15);
            int dexMod = statArray[dexterity];
            int constitution = rnd.Next(6, 12);
            int conMod = statArray[constitution];
            int mind = rnd.Next(4, 12);
            int minMod = statArray[mind];

            return new List<int> { strength, strMod, dexterity, dexMod, constitution, conMod, mind, minMod };
        }
    }
}
