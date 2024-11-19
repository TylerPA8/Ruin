using Ruin.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.General
{
    internal class Phalanx : Creatures
    {
        public Phalanx(string name, List<Attack>? attacks, int ac, int maxhp, int curhp, List<Status>? status, int level, int exp): base(name, ac, maxhp, curhp, status)
        {
            this.name = name;
            this.attacks = GetAttacks();
            this.ac = ac;
            this.maxhp = maxhp;
            this.curhp = curhp;
            this.status = status;
            this.level = level;
            this.exp = exp;           
        }
        public List<Attack> GetAttacks()
        {
            Random rnd = new Random();
            List<Attack> atklist = new List<Attack>();
            List <int> temp = new List<int>();
            while (temp.Count != 4)
            {
                int i = Convert.ToInt32(rnd.Next(0, 8));
                if (!temp.Contains(i))
                {
                    temp.Add(i);
                }
            }
            temp.Sort();
            foreach (int i in temp)
            {
                atklist.Add(AttackLibrary.PhalanxAttacks[i]);
            }
            return atklist;
        }


        public override Creatures? SelectAttack(List<Creatures> enemies)
        {
            string choices = "";
            int runner = 1;
            List<int> validAttacks = new List<int>();
            int targetIndex = Convert.ToInt32(Console.ReadLine()) - 1;
            Console.WriteLine("\n Chose your target: ");
            Creatures target = null;
            //TODO try/catch block for out of bounds numbers.
            try
            {
                target = enemies[targetIndex];
                Console.WriteLine("\n");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Please select a valid target (1 - {enemies.Count()}).");
                SelectAttack(enemies);
            }

            foreach (Attack atk in this.attacks)
            {

                //TODO add checker to see if the player is in the right spot and if there is a target available for that attack

                choices += ($"{runner}. {atk.attackName} ");
                runner++;
            }
            Console.WriteLine($"Select attack:\n{choices}");
            int intchoice = Convert.ToInt32(Console.ReadLine());
            string atkname = this.Attacks[intchoice - 1].attackName;
            Attack atkchoice = this.Attacks[intchoice - 1];
            switch (atkname)
            {
                case ("Puncture"):
                    Puncture(atkchoice, enemies, targetIndex);
                    break;
            }
            //if (validAttacks.Count() == 0)
            //{
            //    return null;
            //}

            //else
            //{
            return target;
            //}
        }


        public static List<Creatures> Puncture(Attack atk, List<Creatures> targetcreatures, int target)
        {
            Random rnd = new Random();
            targetcreatures[target-1].TakeDamage(rnd.Next(atk.minDmg, (atk.maxDmg + 1)));
            if (targetcreatures.IndexOf(targetcreatures[target-1]) != (targetcreatures.Count - 1))
            {

                targetcreatures.Move(targetcreatures[target-1], target);
                return targetcreatures; 
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public static void Skewer(Attack atk, List<Creatures> targetcreatures, int target)
        {

        }
        public static void ShieldBash(Attack atk, List<Creatures> targetcreatures, int target)
        {

        }
        public static void Brace(Attack atk, List<Creatures> targetcreatures, int target)
        {

        }
        public static void Hurl(Attack atk, List<Creatures> targetcreatures, int target)
        {

        }
        public static void PointedCurse(Attack atk, List<Creatures> targetcreatures, int target)
        {

        }
        public static void CursedBlast(Attack atk, List<Creatures> targetcreatures, int target)
        {

        }
        public static void Cover(Attack atk, List<Creatures> targetcreatures, int target)
        {

        }
    }
}
