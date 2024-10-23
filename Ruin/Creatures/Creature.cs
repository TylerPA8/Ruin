using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.Creatures
{
    internal class Creature
    {
        private string name;
        private int maxhp;
        private int curhp;
        private int ac;
        private List<int> stats;
        private List<Attacks> attacks;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Maxhp
        {
            get { return maxhp; }
            set { maxhp = value; }
        }
        public int Curhp
        {
            get { return curhp; }
            set { curhp = value; }
        }
        public int Ac
        {
            get { return ac; }
            set { ac = value; }
        }
        public List<int> Stats
        {
            get { return stats; }
            set { stats = value; }
        }
        public List<Attacks> Attacks
        {
            get { return attacks; }
            set { attacks = value; }
        }
        public Creature(string? name, List<int>? stats, List<Attacks>? attacks)
        {
            if (name == null)
            {
                name = this.GetType().Name;
            }
            else Name = name;
            if (stats == null)
                Stats = GenerateStatArray();
            else Stats = stats;
            Maxhp = GenerateHp();
            Curhp = Maxhp;
            Ac = GenerateAc();
            if (attacks == null)
                Attacks = GenerateAttacks();
            else Attacks = attacks;
        }
        public void displayCreatureStats()
        {
            Console.WriteLine($"Creature name: {name}\nHp: {curhp}/{maxhp}\nAC: {ac}\nStats: {stats}");
        }
        public List<int> GenerateStatArray()
        {
            //generates 4 stats between 6 and 14 and places them in an array. 
            Random rnd = new();
            int strength = rnd.Next(6,15);
            int strMod = Utilities.Rounder(strength);
            int dexterity = rnd.Next(6, 15);
            int dexMod = Utilities.Rounder(dexterity);
            int constitution = rnd.Next(6, 15);
            int conMod = Utilities.Rounder(constitution);
            int mind = rnd.Next(4, 13);
            int minMod = Utilities.Rounder(mind);

            return new List<int> { strength, strMod, dexterity, dexMod, constitution, conMod, mind, minMod };
        }
        public int GenerateHp()
        {
            Random rnd = new();
            int health = rnd.Next(2, 13) + (this.stats[5]); 
            return health;
        }
        public int GenerateAc()
        {
            return (10 + this.stats[3]);
        }
        public List<Attacks> GenerateAttacks()
        {
            //TODO
            return new List<Attacks>();
        }
    }
}

    
