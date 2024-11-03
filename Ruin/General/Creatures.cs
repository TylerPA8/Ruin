using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.General
{
    internal class Creatures
    {
        public string name;
        public int maxhp;
        public int curhp;
        public int ac;
        public List<int> stats;
        public List<Attack> attacks = new List<Attack>();
        public Dictionary<int, int> statArray = new Dictionary<int,int>() {{1, -5 },{2,-4},{3,-4},{ 4,-3},{ 5,-3},{ 6,-2},{ 7,-2},{ 8,-1},{ 9,-1},{ 10,0},{ 11,0},{ 12,1},{ 13,1},{ 14, 2 },{ 15, 2 },{ 16, 3 },{ 17, 3 },{ 18, 4 },{ 19, 4 },{ 20, 5 } };
        protected int strength;
        protected int strMod;
        protected int dexterity;
        protected int dexMod;
        protected int constitution;
        protected int conMod;
        protected int mind;
        protected int minMod;

        public Creatures(string? name, List<int>? stats, List<Attack>? attacks)
        {
            this.name = name;
            if (stats is null)
            {
                GenerateStatArray();
            }
            else
            {
                this.stats = GenerateStatArray(stats);
            }
            this.maxhp = GenerateHp(this.stats[5]);
            this.curhp = this.maxhp;
            this.ac = GenerateAc(this.stats[3]);
            this.attacks = GenerateAttacks(new List<int> { this.stats[1], this.stats[3], this.stats[7] });
        }
        public Creatures(string name, List<int> stats, List<Attack> attacks, int ac, int maxhp, int curhp)
        {
        }

        public void displayCreatureStats()
        {
            Console.WriteLine($"Creature name: {name}\nHp: {curhp}/{maxhp}\nAC: {ac}\nStats: Strength: {stats[0]} Dexterity: {stats[2]} Constitution: {stats[4]} Mind: {stats[6]} \nAttacks: {attacks}");
        }
        public virtual void DisplayCreature()
        {
                Console.WriteLine($"{this.name}\nHp: {this.curhp}/{this.maxhp}\n{this.attacks[0].attackName}, {this.attacks[1].attackName}\n");
        }
        protected virtual void GenerateStatArray()
        {
            //generates 4 stats between 6 and 14 and places them in an array. 
            Random rnd = new();
            this.stats = new List<int>();
            this.strength = rnd.Next(6, 15);
            this.dexterity = rnd.Next(6, 15);
            this.constitution = rnd.Next(6, 15);
            this.mind = rnd.Next(4, 13);
            PullMods();
            PopulateArray();

        }

        protected void PopulateArray()
        {
            this.stats.Add(this.strength);
            this.stats.Add(this.strMod);
            this.stats.Add(this.dexterity);
            this.stats.Add(this.dexMod);
            this.stats.Add(this.constitution);
            this.stats.Add(this.conMod);
            this.stats.Add(this.mind);
            this.stats.Add(this.minMod);
        }

        protected void PullMods()
        {
            this.strMod = statArray[this.strength];
            this.dexMod = statArray[this.dexterity];
            this.conMod = statArray[this.constitution];
            this.minMod = statArray[this.mind];
        }
        public List<int> GenerateStatArray(List<int> stats)
        {
            this.strength = stats[0];
            this.strMod = statArray[strength];
            this.dexterity = stats[1];
            this.dexMod = statArray[dexterity];
            this.constitution = stats[2];
            this.conMod = statArray[constitution];
            this.mind = stats[3];
            this.minMod = statArray[mind];

            return new List<int> { strength, strMod, dexterity, dexMod, constitution, conMod, mind, minMod };
        }
            public int GenerateHp(int c)
        
        {
            Random rnd = new();
            int health = rnd.Next(7, 13) + (c*2); 
            return health;
        }
        public virtual int GenerateAc(int d)
        {
            return (10 + d);
        }
        public virtual List<Attack> GenerateAttacks(List<int> combatMods)
        {
            if ((combatMods[0] >= combatMods[1]) && (combatMods[0] >= combatMods[2])) 
            {
                this.attacks.Add(AttackLibrary.attacksList[0]);
                this.attacks.Add(AttackLibrary.attacksList[4]);
                return this.attacks;
            }
            if ((combatMods[1] >= combatMods[0]) && (combatMods[1] >= combatMods[2])) 
            {
                this.attacks.Add(AttackLibrary.attacksList[2]);
                this.attacks.Add(AttackLibrary.attacksList[4]);
                return this.attacks;
            }
            else 
            { 
                this.attacks.Add(AttackLibrary.attacksList[14]);
                this.attacks.Add(AttackLibrary.attacksList[4]); 
                return this.attacks;
            }
        }
    }
}

    
