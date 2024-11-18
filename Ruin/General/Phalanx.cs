using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.General
{
    internal class Phalanx : Creatures
    {
        public Phalanx(string name, List<Attack>? attacks, int ac, int maxhp, int curhp, List<Status>? status, int level, int exp): base(name, attacks, ac, maxhp, curhp, status, level, exp)
        {
            this.name = name;
            this.attacks = AttackLibrary.PhalanxAttacks;
            this.ac = ac;
            this.maxhp = maxhp;
            this.curhp = curhp;
            this.status = status;
            this.level = level;
            this.exp = exp;           
        }
        public static void Puncture(Attack atk, List<Creatures> targetcreatures, int target)
        {
            Random rnd = new Random();
            targetcreatures[target-1].TakeDamage(rnd.Next(atk.minDmg, (atk.maxDmg + 1)));
            if (targetcreatures.IndexOf(targetcreatures[target-1]) != (targetcreatures.Count - 1))
            {
                Creatures temp = targetcreatures[target-1];
                targetcreatures[target-1] = targetcreatures[target];
                targetcreatures[target] = temp;
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
