using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.General
{
    internal class Combat
    {
        //TODO change to list of player characters
        //TODO when creatures die all slide forward and the back most slot becomes locked.
        public static void StartCombat(Creatures player, List<Creatures> enemies)
        {
            bool escape = false;
            int maxCreatureHp = 0;
            Random rnd = new Random();
            foreach (Creatures c in enemies)
            {
                maxCreatureHp += c.MaxHp;
            }
            Console.WriteLine($"There are {enemies.Count()} creatures before you.");
            foreach (Creatures c in enemies)
            { Display(c); }
            Console.WriteLine($"\nWhat will you do?");

            //Combat loop begins here.
            while (!escape)
            {
                Console.WriteLine("1. Fight 2. Run");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case (1):
                        {
                            int x = 1;
                            foreach (Creatures c in enemies)
                            {
                                DisplayWithNum(x, c);
                                x++;
                            }
                            Creatures target = SelectTarget(enemies);
                            Attack atk = player.SelectAttack();
                            player.AttackRoll(atk, target);

                            if (target.CurHp <= 0)
                            {
                                player.Exp += target.ExpValue;
                                enemies.Remove(target);
                            }
                            CreatureAttack(player, enemies);
                            if (player.CurHp <= 0)
                            {
                                Console.WriteLine("You have been slain.");
                                break;
                            }
                            break;
                        }
                    case (2):
                        {
                            escape = Escape(enemies, maxCreatureHp, player);
                            if (escape == true)
                            {
                                foreach (Creatures c in enemies)
                                    player.Exp += Creatures.FleeExpCalc(c);
                                break;
                            }
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Please select 1 or 2.");
                            break;
                        }
                }


                if (enemies.Count() == 0 || player.CurHp == 0)
                {
                    if (enemies.Count() == 0)
                        Console.WriteLine($"{player.Name} wins!");
                    escape = true;
                }
                else
                {
                    Display(player);
                    Console.WriteLine("Enemies:");

                    foreach (Creatures c in enemies)
                    {
                        Display(c);
                    }
                }
            }
        }


        public static bool Escape(List<Creatures> e, int max, Creatures player)
        {
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
            CreatureAttack(player, e);
                return flee;        
        }


        public static Creatures SelectTarget(List<Creatures> targets)
        {
            Console.WriteLine("\n Chose your target: ");
            //TODO try/catch block for out of bounds numbers.
            try
            {
                Creatures target = targets[Convert.ToInt32(Console.ReadLine()) - 1];
                Console.WriteLine("\n");
                return target;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Please select a valid target (1 - {targets.Count()}).");
                return SelectTarget(targets);
            }        
        }


        public static void CreatureAttack(Creatures player, List<Creatures> enemies)
        {
            foreach (Creatures c in enemies)
            {
                Attack catk = c.SelectAttack(c.Attacks);
                {
                    c.AttackRoll(catk, player);
                }
            }
        }


        public static void Recover(Creatures c)
        {
            c.CurHp += 2;
            if (c.CurHp > c.MaxHp) { c.CurHp = c.MaxHp; }
        }


        public static void Display(Creatures c)
        {
            Console.Write($"{c.Name}\tHp: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{c.CurHp}/{c.MaxHp} ");
            Console.ForegroundColor = ConsoleColor.Gray;
        }


        public static void DisplayWithNum (int x, Creatures c)
        {
            Console.Write($"{x}. {c.Name} Hp: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{c.CurHp}/{c.MaxHp} ");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
