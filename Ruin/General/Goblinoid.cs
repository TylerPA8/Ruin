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
        public List<string> bossNames = new List<string> { "Grimbrog", "Throghul", "Uzgoth", "Morgraith", "Skuldran", "Foulmaw", "Druknar", "Grimfang", "Blightwraith", "Vorthag", "Kruldak", "Gharnûl", "Morthûm", "Vulgrok", "Drelgath", "Kragmog", "Gorthûn", "Zulgarth", "Vrognir", "Ghûldren" };
        public Goblinoid(string name, List<int> stats, List<Attack> attacks) : base(name, stats, attacks)
        {
        }
        protected override void GenerateStatArray()
        {
            //generates 4 stats between 6 and 14 and places them in an array. 
            Random rnd = new();
            this.stats = new List<int>();
            this.strength = rnd.Next(6, 13);
            this.dexterity = rnd.Next(6, 15);
            this.constitution = rnd.Next(6, 13);
            this.mind = rnd.Next(4, 11);
            PullMods();
            PopulateArray();

        }
        public override int GenerateAc(int d)
        {
            return (11 + d);
        }
        public override List<Attack> GenerateAttacks(List<int> combatMods)
        {
            this.attacks.Add(AttackLibrary.attacksList[2]);
            this.attacks.Add(AttackLibrary.attacksList[4]);
            this.attacks.Add(AttackLibrary.attacksList[18]);
            return this.attacks;
        }
        public override void DisplayCreature()
        {
            Console.WriteLine($"{this.name}\nHp: {this.curhp}/{this.maxhp}\n{this.attacks[0].attackName}, {this.attacks[1].attackName}, {this.attacks[2].attackName}\n");
        }
    }
}
