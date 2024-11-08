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
        protected string name;
        protected int maxhp;
        protected int curhp;
        protected int maxstamina;
        protected int curstamina;
        protected int maxmana;
        protected int curmana;
        protected int ac;
        protected List<int> stats;
        protected List<Attack> attacks = new List<Attack>();
        protected Dictionary<int, int> statArray = new Dictionary<int,int>() {{1, -5 },{2,-4},{3,-4},{ 4,-3},{ 5,-3},{ 6,-2},{ 7,-2},{ 8,-1},{ 9,-1},{ 10,0},{ 11,0},{ 12,1},{ 13,1},{ 14, 2 },{ 15, 2 },{ 16, 3 },{ 17, 3 },{ 18, 4 },{ 19, 4 },{ 20, 5 } };
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
            this.maxstamina = this.constitution;
            this.curstamina = this.maxstamina;
            this.maxmana = this.mind;
            this.curmana = maxmana;
            this.ac = GenerateAc(this.stats[3]);
            this.attacks = GenerateAttacks(new List<int> { this.stats[1], this.stats[3], this.stats[7] });
        }

        public Creatures(string name, List<int> stats, List<Attack> attacks, int ac, int maxhp, int curhp, int maxstamina, int curstamina, int maxmana, int curmana, Status status)
        {
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
            this.mind = rnd.Next(6, 15);
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
                this.attacks.Add(AttackLibrary.attacksList[1]);
                this.attacks.Add(AttackLibrary.attacksList[4]);
                return this.attacks;
            }
            if ((combatMods[1] >= combatMods[0]) && (combatMods[1] >= combatMods[2])) 
            {
                this.attacks.Add(AttackLibrary.attacksList[2]);
                this.attacks.Add(AttackLibrary.attacksList[4]);
                this.attacks.Add(AttackLibrary.attacksList[5]);
                return this.attacks;
            }
            else 
            { 
                this.attacks.Add(AttackLibrary.attacksList[14]);
                this.attacks.Add(AttackLibrary.attacksList[4]);
                Console.WriteLine("Choose your magic speciality:\n1. Fire 2.Poison 3. Cold 4. Lightning 5. Healing 6. Occult");
                int magicSelect = Convert.ToInt32(Console.ReadLine());
                switch (magicSelect)
                {
                    case 1:
                        this.attacks.Add(AttackLibrary.attacksList[6]);
                        break;
                    case 2:
                        this.attacks.Add(AttackLibrary.attacksList[8]);
                        break;
                    case 3:
                        this.attacks.Add(AttackLibrary.attacksList[10]);
                        break;
                    case 4:
                        this.attacks.Add(AttackLibrary.attacksList[12]);
                        break;
                    case 5:
                        this.attacks.Add(AttackLibrary.attacksList[16]);
                        break;
                    case 6:
                        this.attacks.Add(AttackLibrary.attacksList[17]);
                        break;
                }
                return this.attacks;
            }
        }

        public void TakeDamage(int d)
        {
            if (this.curhp - d <= 0)
            {
                Console.WriteLine($"{this.name} took {d} damage and died.");
            }
            else
            {
                this.curhp -= d;
            }
        }
        
        public void AttackRoll(Attack atk, int proficiencyBonus, Creatures target)
        {
            Random dice = new ();
            int roll = dice.Next(1, 21);
            int dmgRoll = dice.Next(atk.minDmg, (atk.maxDmg+1));
            int dmgMod = 0;
            switch(atk.attackName)
            {
                case ("Wack" or "Crush"):
                    dmgMod = this.strMod;
                    break;
                case ("Stab" or "Puncture"):
                    dmgMod = this.dexMod;
                    break;
                case ("Nick" or "Slash"):
                    if (this.strMod >= this.dexMod)
                    {
                        dmgMod = this.strMod;
                    }
                    else
                    {
                        dmgMod += this.dexMod;
                    }
                    break;
                default:
                    dmgMod = this.minMod;
                    break;
            }
            dmgRoll += dmgMod;
            if (roll == 20)
            {
                Console.WriteLine($"A critical hit for {atk.maxDmg + dmgRoll}!");
                DealDamage((atk.maxDmg + dmgRoll), target);
            }
            else
            {
                int atkRoll = roll+dmgMod+proficiencyBonus;
                bool hit = CheckHit(atkRoll, target);
                switch (hit)
                {
                    case true:
                        Console.Write($"A {atkRoll} hits {target.name} for {dmgRoll}!\n");
                        DealDamage(dmgRoll, target);
                        break;
                    case false:
                        Console.Write($"A {atkRoll} misses {target.name}!");
                        break;
                }

            }
        }

        public bool CheckHit(int atkRoll, Creatures target)
        {
            if (atkRoll >= target.ac)
                return true;
            else; return false;
        }

        public void DealDamage(int dmg, Creatures target)
        {
            target.curhp -= dmg;
        }

        public Creatures SelectTarget(List<Creatures> targets)
        {
            Console.WriteLine($"Select a target:");
            int x = 1;
            foreach (Creatures c in targets)
            {
                Console.Write($"{x}: {c.name} ");
                x++;
            }
            Console.WriteLine("\n");
            Creatures target = targets[Convert.ToInt32(Console.ReadLine())];
            return target;
        }

    }
}

    
