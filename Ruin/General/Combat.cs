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
                }

                else
                    escape = Escape();
                break;

                Creatures target = SelectTarget(enemies);
                Attack atk = player.SelectAttack();
                player.AttackRoll(atk, target);
                if (target.CurHp <= 0)
                {
                    enemies.Remove(target);
                }
            }
        }
        public static bool Escape()
        {
            //Add % chance to escape based on enemy total health.
            bool flee = true;
            return flee;
        }
        public static Creatures SelectTarget(List<Creatures> targets)
        {
            Console.WriteLine("\n Chose your target: ");
            Creatures target = targets[Convert.ToInt32(Console.ReadLine())-1];
            return target;
        }
    }
}
