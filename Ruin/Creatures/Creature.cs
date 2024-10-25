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
        public string name;
        public int maxhp;
        public int curhp;
        public int ac;
        public List<int> stats;
        public List<Attack> attacks;
        public Dictionary<int, int> statArray = new Dictionary<int,int>() {{1, -5 },{2,-4},{3,-4},{ 4,-3},{ 5,-3},{ 6,-2},{ 7,-2},{ 8,-1},{ 9,-1},{ 10,0},{ 11,0},{ 12,1},{ 13,1},{ 14, 2 },{ 15, 2 },{ 16, 3 },{ 17, 3 },{ 18, 4 },{ 19, 4 },{ 20, 5 } };

        public Creature(string? name, List<int>? stats, List<Attack>? attacks)
        {
            if (name == null)
            {
                name = this.GetType().Name;
            }
            if (stats == null)
            {
                this.stats = GenerateStatArray();
            }
            this.maxhp = GenerateHp(this.stats[5]);
            this.curhp = this.maxhp;
            this.ac = GenerateAc(this.stats[3]);
            if (attacks == null)
                attacks = GenerateAttacks(new List<int> { this.stats[1], this.stats[3], this.stats[7] });
        }
        public void displayCreatureStats()
        {
            Console.WriteLine($"Creature name: {name}\nHp: {curhp}/{maxhp}\nAC: {ac}\nStats: \nStrength: {stats[0]} Dexterity: {stats[2]} Constitution: {stats[4]} Mind: {stats[6]} \nAttacks: {attacks}");
        }
        public List<int> GenerateStatArray()
        {
            //generates 4 stats between 6 and 14 and places them in an array. 
            Random rnd = new();
            int strength = rnd.Next(6,15);
            int strMod = statArray[strength];
            int dexterity = rnd.Next(6, 15);
            int dexMod = statArray[dexterity];
            int constitution = rnd.Next(6, 15);
            int conMod = statArray[constitution];
            int mind = rnd.Next(4, 13);
            int minMod = statArray[mind];

            return new List<int> { strength, strMod, dexterity, dexMod, constitution, conMod, mind, minMod };
        }
        public int GenerateHp(int c)
        
        {
            Random rnd = new();
            int health = rnd.Next(7, 13) + (c*2); 
            return health;
        }
        public int GenerateAc(int d)
        {
            return (10 + d);
        }
        public List<Attack> GenerateAttacks(List<int> combatMods)
        {
            if ((combatMods[0] >= combatMods[1]) && (combatMods[0] >= combatMods[2])) { return this.attacks.Add(Punch, Slash); }
            if ((combatMods[1] >= combatMods[0]) && (combatMods[1] >= combatMods[2])) { return this.attacks.Add(Stab, Slash); }
            else { return this.attacks.Add(ArcaneBolt, Slash); }
        }
    }
}

    
