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
        protected int ac;
        protected List<Attack> attacks = new List<Attack>();
        protected List<Status> status = new List<Status>();
        protected int expvalue;
        protected int level;
        protected int exp;

        public string Name => name;
        public int MaxHp
        {
            get {return maxhp;}
            set {maxhp = value;}
        }
        public int CurHp
            {
            get { return curhp; }
            set { curhp = value; }
            } 
        public int Ac
        {
            get { return ac; }
            set { ac = value; }
        }
        public List<Attack> Attacks
        { 
            get { return attacks; }
            set { attacks = value; }
        }
        public List<Status> Status
        {
            get { return status; }
            set { status = value; }
        }
        public int ExpValue
        {
            get { return expvalue; }
            set { expvalue = value; }
        }
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        public int Exp
        {
            get { return exp; }
            set { exp = value; }
        }
        


        public Creatures(string name, int ac, int maxhp, int curhp, List<Status>? status)
        {
            this.name = name;
            this.ac = ac;
            this.maxhp = maxhp;
            this.curhp = curhp;
            this.status = status;
        }

        public Creatures(string name, List<Attack>? attacks, int ac, int maxhp, int curhp, List<Status>? status, int level, int exp)
        {
            this.name = name;
            this.attacks = attacks;
            this.ac = ac;
            this.maxhp = maxhp;
            this.curhp = curhp;
            this.status = status;
            this.level = level;
            this.exp = exp;
        }

        public void DisplayCreature()
        {
            string atkString = "";
            foreach (Attack atk in this.attacks)
            {
                if (atk.Equals(this.attacks[(this.attacks.Count) - 1]))
                    atkString += ($"{atk.attackName} ");
                else
                    atkString += ($"{atk.attackName}, ");
            }
            Console.Write($"{this.name}\nHp: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{this.curhp}/{this.maxhp} ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"\nAttacks: {atkString}\n\n");
        }

        public virtual Creatures? SelectAttack(List<Creatures> enemies)
        {
            return null;
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
            int atkRoll = dice.Next(1, 21);
            int dmg = dice.Next(atk.minDmg, (atk.maxDmg+1));

            if (atkRoll == 20)
            {
                Console.WriteLine($"A critical hit on {target.Name} for {atk.maxDmg + dmg}!");
                DealDamage((atk.maxDmg + dmg), target);
                if (target.CurHp <= 0)
                    Console.WriteLine($"{target.Name} has been slain!");
            }
            else
            {
                bool hit = CheckHit(atkRoll, target);
                switch (hit)
                {
                    case true:
                        Console.Write($"{this.Name} uses {atk.attackName}! A {atkRoll} hits {target.name} for {dmg}!\n");
                        DealDamage(dmg, target);
                        if (target.CurHp <= 0)
                            Console.WriteLine($"{target.Name} has been slain!\n");

                        break;
                    case false:
                        Console.Write($"{this.Name} uses {atk.attackName}! A {atkRoll} misses {target.name}!\n");
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

        //public Attack? SelectAttack(List<Attack> atks)
        //{
        //    Random rnd = new Random();
        //    return atks[rnd.Next(0,(atks.Count-1))];

        //}

        //TODO Check where enemies and you are, then select an attack.
        //public Attack? SelectAttack(List<Attack> attacks)
        //{
        //    Random rnd = new Random();
        //    List <Attack> tempAttacks = new (this.attacks.ToList());
        //    foreach (Attack attack in this.attacks)
        //    {
        //        if (attack.stamCost > this.CurStamina)
        //        { tempAttacks.Remove(attack); }
        //    }
        //    foreach (Attack attack in tempAttacks)
        //        if (attack.manaCost > this.CurMana)
        //        { tempAttacks.Remove(attack); }
        //    if (tempAttacks.Count == 0)
        //    {
        //        return null;
        //    }
        //    Attack atkchoice = tempAttacks[rnd.Next(0, (tempAttacks.Count()-1))];
        //    return atkchoice;
        //}


        public static int FleeExpCalc(Creatures c) 
        {
            int exp = ((c.CurHp/c.MaxHp)*c.ExpValue);
            return (exp);
        }

        //public void LevelCheck()
        //{
        //    if (this.exp >= Utilities.levelChart[this.level])
        //    {
        //        LevelUp();
        //    }
        //}
        //TODO allow increasing, hp, ac, or variables of attacks on level.

        //public void LevelUp()
        //{
        //    Random rnd = new Random();
        //    this.Exp -= Utilities.levelChart[this.level];
        //    this.Level++;
        //    StatIncrease();
        //    this.MaxHp += (rnd.Next(1, 9) + this.ConMod);
        //    this.CurHp = this.MaxHp;
        //}

        //public void StatIncrease()
        //{
        //    int si = 2;
        //    while (si > 0)
        //    {
        //        Console.WriteLine("Which stat will you increase? 1. Strength 2. Dexterity 3. Constitution 4. Mind");
        //        int choice = Convert.ToInt32(Console.ReadLine());
        //        switch (choice)
        //        {
        //            case (1):
        //                this.Strength++;
        //                break;
        //            case (2):
        //                this.Dexterity++;
        //                break;
        //            case (3):
        //                this.Constitution++;
        //                break;
        //            case (4):
        //                this.Mind++;
        //                break;
        //        }
        //        si--;
        //    }
    }
}

    
