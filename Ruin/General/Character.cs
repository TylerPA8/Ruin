using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.General
{
    internal class Character : Creatures
    {
        public Character(string name, List<int> stats, List<Attack> attacks): base(name, stats, attacks) 
        {
        }

        public Character(string name, List<int> stats, List<Attack> attacks, int ac, int maxhp, int curhp, int maxstamina, int curstamina, int maxmana, int curmana, List<Status> status, int proficiency = 2):base(name,stats, attacks, ac, maxhp, curhp, maxstamina,curstamina, maxmana, curmana, status, proficiency)
        {
            this.name = name;
            this.stats = GenerateStatArray(stats);
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

        protected override void GenerateStatArray()
        {
            //generates 4 stats between 6 and 14 and places them in an array. 
            Random rnd = new();
            int totalStats = rnd.Next(16,36);
            this.stats = new List<int>();
            this.strength = 6;
            this.dexterity = 6;
            this.constitution = 6;
            this.mind = 6;
            
                while (totalStats > 0)
                {
                    Console.WriteLine($"You have {totalStats} points to spend.\nEach stat can be increased up to 15. Which stat would you like to increase?\n1. Strength: {this.strength} 2. Dexterity: {this.dexterity} 3. Constitution: {this.constitution} 4. Mind: {this.mind}\n");
                    int statChoice = Convert.ToInt32(Console.ReadLine());
                    string statChoiceString = " ";
                    int curStat = 0; 
                    switch (statChoice)
                    {
                        case 1:
                            statChoiceString = "Strength";
                            curStat = this.strength;
                            break;
                        case 2:
                            statChoiceString = "Dexterity";
                            curStat = this.dexterity;
                        break;
                        case 3:
                            statChoiceString = "Constitution";
                            curStat = this.constitution;
                        break;
                        case 4:
                            statChoiceString = "Mind";
                            curStat = this.mind;
                        break;
                    }
                    Console.WriteLine($"How much would you like to increase {statChoiceString}?");
                    int statRaise = Convert.ToInt32(Console.ReadLine());
                    if (((totalStats - statRaise) >= 0) && ((statRaise + statChoice) <= 15) )
                    {
                        switch(statChoice)
                        {
                            case 1:
                                this.strength += statRaise;
                                break;
                            case 2:
                                this.dexterity += statRaise;
                                break;
                            case 3:
                                this.constitution += statRaise;
                                break;
                            case 4:
                                this.mind += statRaise;
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("That is not a valid increase, please try again.");
                    continue;
                    }

                    totalStats -= statRaise;
                }
            PullMods();
            PopulateArray();

        }

        public override List<Attack> GenerateAttacks(List<int> combatMods)
        {
            if ((combatMods[2] >= combatMods[0]) && (combatMods[2] >= combatMods[1]))
            {
                this.attacks.Add(AttackLibrary.attacksList[14]);
                this.attacks.Add(AttackLibrary.attacksList[4]);
                int spellPages = combatMods[2];
                while (spellPages > 0)
                {
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
                    spellPages--;
                }
                return this.attacks;
            }
            if (combatMods[0] >= combatMods[1])
            {
                this.attacks.Add(AttackLibrary.attacksList[0]);
                this.attacks.Add(AttackLibrary.attacksList[1]);
                this.attacks.Add(AttackLibrary.attacksList[4]);
                if (combatMods[2] > 0)
                    this.attacks.Add(AttackLibrary.attacksList[14]);
                return this.attacks;
            }
            else
            {
                this.attacks.Add(AttackLibrary.attacksList[2]);
                this.attacks.Add(AttackLibrary.attacksList[4]);
                this.attacks.Add(AttackLibrary.attacksList[5]);
                if (combatMods[2] > 0)
                    this.attacks.Add(AttackLibrary.attacksList[14]);
                return this.attacks;
            }
        }

        public override void DisplayCreature()
        {
            string atkString = "";
            foreach (Attack atk in this.attacks)
            {
                if (atk.Equals(this.attacks[(this.attacks.Count)-1]))
                    atkString += ($"{atk.attackName} ");
                else
                    atkString += ($"{atk.attackName}, ");
            }
                string statString = ($"Strength: {this.stats[0]} Dexterity: {this.stats[2]} Constitution: {this.stats[4]} Mind: {this.stats[6]}");
            Console.Write($"{this.name}\nHp: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{this.curhp}/{this.maxhp} ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"AC: {this.ac}\nStamina: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{this.curstamina}/{this.maxstamina} ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Mana: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{this.curmana}/{this.maxmana}\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{ statString}\nAttacks: { atkString}\n\n");
        }


        public Attack SelectAttack()
        {
            string choices = "";
            int runner = 1;
            foreach (Attack atk in this.attacks)
            {
                choices += ($"{runner}. {atk.attackName} Stamina Cost: {atk.stamCost} Mana Cost: {atk.manaCost}\n");
                runner++;
            }
            Console.WriteLine($"Select attack:\n{choices}");
            Attack atkchoice = this.Attacks[Convert.ToInt32(Console.ReadLine()) - 1];
            if ((atkchoice.stamCost > this.CurStamina) || ((this.CurStamina - atkchoice.stamCost) < 0))
            {
                Console.WriteLine($"{this.Name} pants heavily, their stamina too low to do that attack.");
                return AttackLibrary.attacksList[19];
            }

            if ((atkchoice.manaCost > this.CurMana) || ((this.CurMana - atkchoice.manaCost) < 0))
            {
                Console.WriteLine($"{this.Name}'s head pounds, unable to conjour enough mana for that attack.");
                return AttackLibrary.attacksList[19];
            }

            else
            {
                return atkchoice;
            }
        }
    }   
}
