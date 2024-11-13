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
        public static void StartCombat(Character player, List<Creatures> enemies)
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
                Console.WriteLine("1. Fight 2. Recover 3. Run");
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
                            if (atk == null)
                            {
                                Recover(player);
                            }

                            else
                            {
                                player.AttackRoll(atk, target);
                            }

                            if (target.CurHp <= 0)
                            {
                                player.Exp += Creatures.ExpCalc(target);
                                enemies.Remove(target);
                            }
                            CreatureAttack(player, enemies);
                            if (player.CurHp <= 0)
                            {
                                Console.WriteLine("You have been slain.");
                                break;
                            }
                            else
                            {
                                EndRoundRegen(player, enemies);
                            }
                            break;
                        }
                    case (2):
                        {
                            CreatureAttack(player, enemies);
                            if (player.CurHp <= 0)
                            {
                                Console.WriteLine("You have been slain.");
                                break;
                            }
                            Recover(player);
                            EndRoundRegen(player, enemies);
                            break;
                        }
                    case (3):
                        {
                            escape = Escape(enemies, maxCreatureHp, player);
                            if (escape == true)
                            {
                                foreach (Creatures c in enemies)
                                    player.Exp += Creatures.FleeExpCalc(c);
                                break;
                            }

                            EndRoundRegen(player, enemies);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Please select 1, 2, or 3.");
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


        public static bool Escape(List<Creatures> e, int max, Character player)
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


        public static void CreatureAttack(Character player, List<Creatures> enemies)
        {
            foreach (Creatures c in enemies)
            {
                Attack catk = c.SelectAttack();
                if (catk == null)
                {
                    Console.Write($"{c.Name} pants heavily, attempting to catch their breath.\n");
                    Recover(c);
                }
                else
                {
                    c.AttackRoll(catk, player);
                }
            }
        }


        public static void EndRoundRegen(Character player, List<Creatures> enemies)
        {        
            if (player.CurStamina < player.MaxStamina)
            {
                if (player.ConMod <= 0)
                {
                    if ((player.CurStamina + 1) <= player.MaxStamina)
                    {
                        player.CurStamina += 1;
                    }
                }
                else
                    {
                        player.CurStamina += player.ConMod;
                        if (player.CurStamina > player.MaxStamina)
                        {
                            player.CurStamina = player.MaxStamina;
                        }
                    }

            }

            if (player.CurMana < player.MaxMana)
            {
                if (player.MindMod <= 0)
                {
                    if (player.CurMana + 1 <= player.MaxMana)
                    {
                        player.CurMana += 1;
                    }
                }
                else
                    {
                        player.CurMana += player.MindMod;
                        if (player.CurMana > player.MaxMana)
                        {
                            player.CurMana = player.MaxMana;
                        }
                    }

            }

            foreach (Creatures c in enemies)
            {
                if (c.CurStamina < c.MaxStamina)
                {
                    if (c.ConMod <= 0)
                    {
                        if (c.CurStamina + 1 <= c.MaxStamina)
                        {
                            c.CurStamina += 1;
                        }
                    }
                    else
                        c.CurStamina += c.ConMod;

                    if (c.CurStamina > c.MaxStamina)
                    {
                        c.CurStamina = c.MaxStamina;
                    }
                }

                if (c.CurMana < c.MaxMana)
                {
                    if (c.MindMod <= 0)
                    {
                        if (c.CurMana + 1 <= c.MaxMana)
                        {
                            c.CurMana += 1;
                        }
                    }
                    else
                        c.CurMana += c.MindMod;

                    if (c.CurMana  > c.MaxMana)
                    {
                        c.CurMana = c.MaxMana;
                    }
                }
            }
        }


        public static void Recover(Creatures c)
        {
            c.CurHp += c.ConMod;
            c.CurStamina += c.ConMod;
            c.CurMana += c.MindMod;
            if (c.CurStamina > c.MaxStamina) { c.CurStamina = c.MaxStamina; }
            if (c.CurMana > c.MaxMana) { c.CurMana = c.MaxMana; }
            if (c.CurHp > c.MaxHp) { c.CurHp = c.MaxHp; }
        }


        public static void Display(Creatures c)
        {
            Console.Write($"{c.Name}\tHp: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{c.CurHp}/{c.MaxHp} ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Stamina: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{c.CurStamina}/{c.MaxStamina} ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"Mana: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{c.CurMana}/{c.MaxMana}\n\n");
            Console.ForegroundColor = ConsoleColor.Gray;
        }


        public static void DisplayWithNum (int x, Creatures c)
        {
            Console.Write($"{x}. {c.Name} Hp: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{c.CurHp}/{c.MaxHp} ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Stamina: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{c.CurStamina}/{c.MaxStamina} ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"Mana: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{c.CurMana}/{c.MaxMana}\n\n");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
