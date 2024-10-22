using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<Utilities.Attacks> attacks;

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
        public List<Utilities.Attacks> Attacks
        {
            get { return attacks; }
            set { attacks = value; }
        }
        public Creature (string name, int maxhp, int curhp,  int ac, List<int> stats, List<Utilities.Attacks> attacks)
        {
            Name = name;
            Maxhp = maxhp;
            Curhp = curhp;
            Ac = ac;
            Stats = stats;
            Attacks = attacks;
        }
        public void displayCreatureStats()
        {
            Console.WriteLine($"Creature name: {name}\nHp: {curhp}/{maxhp}\nAC: {ac}\nStats: {stats}");
        }
    }
}

    
