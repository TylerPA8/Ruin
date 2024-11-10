using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.General
{
    internal class Combat
    {
        public static void StartCombat(Character player, List<Creatures> enemies)
        {
            bool escape = false;
            int maxCreatureHp = 0;
            Random rnd = new Random();
            foreach (Creatures c in enemies)
            {
                maxCreatureHp += c.MaxHp;
            }

            Console.WriteLine($"There are {enemies.Count()} creatures before you.\nWhat will you do?");

            while (!escape)
            {
                Console.WriteLine("1. Fight 2. Run");
                if (Convert.ToInt32(Console.ReadLine()) == 1)
                {
                    int x = 1;
                    foreach (Creatures c in enemies)
                    {
                        Console.WriteLine($"{x}: {c.Name}\n   {c.CurHp}/{c.MaxHp}");
                        x++;
                    }
                    Creatures target = SelectTarget(enemies);
                    Attack atk = player.SelectAttack();
                    player.AttackRoll(atk, target);
                    if (target.CurHp <= 0)
                    {
                        enemies.Remove(target);
                    }
                    foreach(Creatures c in enemies)
                    {
                        //TODO select attacks for creatures.
                        Attack catk = c.SelectAttack();
                        c.AttackRoll(atk, player);
                        if (player.CurHp <= 0)
                        {
                            Console.WriteLine("You have been slain.");
                            break;
                        }
                    }
                }
                
                else
                {
                    escape = Escape(enemies, maxCreatureHp);
                }
                if (escape == true)
                    break;
            }
        }
        public static bool Escape(List<Creatures> e, int max)
        {
            //Add % chance to escape based on enemy total health.
            bool flee = false;
            int totalHp = 0;
            int curHp = 0;
            Random rnd = new Random();
            foreach (Creatures c in e) 
            {
                curHp += c.CurHp;
                totalHp += c.MaxHp;
            }
            if (rnd.Next(0, (max + 1)) > curHp)
            {
                flee = true;
                Console.WriteLine("Got away safely.");
                return flee;
            }
            else
                Console.WriteLine("Can't escape!");
                return flee;        
        }
        public static Creatures SelectTarget(List<Creatures> targets)
        {
            Console.WriteLine("\n Chose your target: ");
            //TODO try/catch block for out of bounds numbers.
            Creatures target = targets[Convert.ToInt32(Console.ReadLine())-1];
            return target;
        }
    }
}
