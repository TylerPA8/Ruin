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
        protected int proficiency;
        protected List<int> stats;
        protected List<Attack> attacks = new List<Attack>();
        protected List<Status> status = new List<Status>();
        protected Dictionary<int, int> statArray = new Dictionary<int,int>() {{1, -5 },{2,-4},{3,-4},{ 4,-3},{ 5,-3},{ 6,-2},{ 7,-2},{ 8,-1},{ 9,-1},{ 10,0},{ 11,0},{ 12,1},{ 13,1},{ 14, 2 },{ 15, 2 },{ 16, 3 },{ 17, 3 },{ 18, 4 },{ 19, 4 },{ 20, 5 } };
        protected int strength;
        protected int strMod;
        protected int dexterity;
        protected int dexMod;
        protected int constitution;
        protected int conMod;
        protected int mind;
        protected int minMod;
        public string Name => name;
        public int MaxHp => maxhp;
        public int CurHp
            {
            get { return curhp; }
            set { curhp = value; }
            } 
        public int MaxStamina => maxstamina;
        public int CurStamina
        {
            get { return curstamina; }
            set { curstamina = value; }
        }
        public int MaxMana => maxmana;
        public int CurMana
        {
            get { return curmana; }
            set { curmana = value; }
        }
        public int Ac => ac;
        public int Proficiency => proficiency;
        public List<Attack> Attacks => attacks;
        public List<Status> Status => status;
        public int Strength => strength;
        public int StrMod => strMod;
        public int Dexterity => dexterity;
        public int DexterityMod => dexMod;
        public int Constitution => constitution;
        public int ConMod => conMod;
        public int Mind => mind;
        public int MindMod => minMod;

        public Creatures(string? name, List<int>? stats, List<Attack>? attacks, int proficiency = 2)
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
            this.maxstamina = this.constitution/2;
            this.curstamina = this.maxstamina;
            this.maxmana = this.mind/2;
            this.curmana = maxmana;
            this.ac = GenerateAc(this.stats[3]);
            this.attacks = GenerateAttacks(new List<int> { this.stats[1], this.stats[3], this.stats[7] });
        }

        public Creatures(string name, List<int> stats, List<Attack> attacks, int ac, int maxhp, int curhp, int maxstamina, int curstamina, int maxmana, int curmana, List<Status> status, int proficiency = 2)
        {
            this.name = name;
            this.stats = stats;
            this.attacks = attacks;
            this.ac = ac;
            this.maxhp = maxhp;
            this.curhp = curhp;
            this.maxstamina = maxstamina;
            this.curstamina = curstamina;
            this.maxmana = maxmana;
            this.curmana = curmana;
            this.status = status;
            this.proficiency = proficiency;
        }

        public virtual void DisplayCreature()
        {
            Console.Write($"{this.name}\nHp: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{this.curhp}/{this.maxhp}\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{this.attacks[0].attackName}, {this.attacks[1].attackName}\n");
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
                Console.WriteLine("Choose your magic speciality:\n1. Fire 2.Poison 3. Cold 4. Lightning");
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
        
        public void AttackRoll(Attack atk, Creatures target)
        {
            Random dice = new ();
            int roll = dice.Next(1, 21);
            int dmgRoll = dice.Next(atk.minDmg, (atk.maxDmg+1));
            int dmgMod = 0;
            int dmg = 0;
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
            dmg = dmgMod+dmgRoll;

            if (dmg <= 0)
                dmg = 0;

            if (roll == 20)
            {
                Console.WriteLine($"A critical hit on {target.Name} for {atk.maxDmg + dmgRoll}!");
                DealDamage((atk.maxDmg + dmg), target);
                ResourceCost(this, atk);
                if (target.CurHp <= 0)
                    Console.WriteLine($"{target.Name} has been slain!");
            }
            else
            {
                int atkRoll = roll+dmgMod+this.proficiency;
                bool hit = CheckHit(atkRoll, target);
                switch (hit)
                {
                    case true:
                        Console.Write($"{this.Name} uses {atk.attackName}! A {atkRoll} hits {target.name} for {dmg} ({dmgRoll} + {dmgMod})!\n");
                        DealDamage(dmg, target);
                        if (target.CurHp <= 0)
                            Console.WriteLine($"{target.Name} has been slain!\n");
                        ResourceCost(this, atk);

                        break;
                    case false:
                        Console.Write($"{this.Name} uses {atk.attackName}! A {atkRoll} misses {target.name}!\n");
                        ResourceCost(this, atk);
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
            target.CurHp -= dmg;
        }

        public Attack? SelectAttack()
        {
            Random rnd = new Random();
            List <Attack> tempAttacks = new (this.attacks.ToList());
            foreach (Attack attack in this.attacks)
            {
                if (attack.stamCost > this.CurStamina)
                { tempAttacks.Remove(attack); }
            }
            foreach (Attack attack in tempAttacks)
                if (attack.manaCost > this.CurMana)
                { tempAttacks.Remove(attack); }
            if (tempAttacks.Count == 0)
            {
                return null;
            }
            Attack atkchoice = tempAttacks[rnd.Next(0, (tempAttacks.Count()-1))];
            return atkchoice;
        }

        public static void ResourceCost(Creatures c, Attack a)
        {
            c.CurStamina -= a.stamCost;
            c.CurMana -= a.manaCost;
        }

    }
}

    
