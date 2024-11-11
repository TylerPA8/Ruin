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
            Console.Clear();
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
                        if (enemies.Count() == 0)
                        {
                            Console.WriteLine($"{player.Name} wins!");
                            break;
                        }
                    }
                    CreatureAttack(player, enemies);
                    if (player.CurHp <= 0)
                    {
                        Console.WriteLine("You have been slain.");
                        break;
                    }
                }

                else
                {
                    escape = Escape(enemies, maxCreatureHp, player);
                }
                if (escape == true)
                    break;

                Console.WriteLine($"\n{player.Name}\nHp:{player.CurHp}/{player.MaxHp} Stamina: {player.CurStamina}/{player.MaxStamina} Mana: {player.CurMana}/{player.MaxMana}\n");
                Console.WriteLine("Enemies:");
                
                foreach (Creatures c in enemies)
                {
                    Console.WriteLine($"{c.Name}\nHp:{c.CurHp}/{c.MaxHp} Stamina: {c.CurStamina}/{c.MaxStamina} Mana: {c.CurMana}/{c.MaxMana}\n");
                }
                EndRoundRegen(player, enemies);
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
            Creatures target = targets[Convert.ToInt32(Console.ReadLine())-1];
            return target;
        }

        public static void CreatureAttack(Character player, List<Creatures> enemies)
        {
            foreach (Creatures c in enemies)
            {
                //TODO select attacks for creatures.
                Attack catk = c.SelectAttack();
                c.AttackRoll(catk, player);
            }
        }

        public static void EndRoundRegen(Character player, List<Creatures> enemies)
        {        
            if (player.CurStamina < player.MaxStamina)
            {
                if (player.ConMod <= 0)
                    if (player.CurStamina + 1 <= player.MaxStamina)
                    {
                        player.CurStamina += 1;
                    }
                if ((player.ConMod > 0) && (((player.CurStamina + player.ConMod) > player.MaxStamina)))
                    player.CurStamina = player.MaxStamina;
                else
                    player.CurStamina += player.ConMod;
            }

            if (player.CurMana < player.MaxMana)
            {
                if (player.MindMod <= 0)
                    if (player.CurMana + 1 <= player.MaxMana)
                    {
                        player.CurMana += 1;
                    }
                if ((player.MindMod > 0) && (((player.CurMana + player.MindMod) > player.MaxMana)))
                    player.CurMana = player.MaxMana;
                else
                    player.CurMana += player.MindMod;
            }

            foreach (Creatures c in enemies)
            {
                if (player.CurStamina < player.MaxStamina)
                {
                    if (player.ConMod <= 0)
                        if (player.CurStamina + 1 <= player.MaxStamina)
                        {
                            player.CurStamina += 1;
                        }
                    if ((player.ConMod > 0) && (((player.CurStamina + player.ConMod) > player.MaxStamina)))
                        player.CurStamina = player.MaxStamina;
                    else
                        player.CurStamina += player.ConMod;
                }

                if (player.CurMana < player.MaxMana)
                {
                    if (player.MindMod <= 0)
                        if (player.CurMana + 1 <= player.MaxMana)
                        {
                            player.CurMana += 1;
                        }
                    if ((player.MindMod > 0) && (((player.CurMana + player.MindMod) > player.MaxMana)))
                        player.CurMana = player.MaxMana;
                    else
                        player.CurMana += player.MindMod;
                }
            }
        }
    }
}
