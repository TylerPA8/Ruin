using Ruin.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin
{
    internal class AttackLibrary
    {
    public static readonly List<Attack> attacksList = new List<Attack>(); 

    internal void InitializeAttacks()
        {
            Attack Wack = new("Wack", "A swing of a crude club.", 1, 4, AttackType.Bludgeoning);
            Attack Crush = new("Crush", "A heavy swing of a blunt weapon.", 1, 6, AttackType.Bludgeoning);
            Attack Stab = new("Stab", "A quick stab with a dull blade.", 1, 4, AttackType.Piercing);
            Attack Puncture = new("Puncture", "A precise stab with a sharp blade.", 1, 6, AttackType.Piercing);
            Attack Nick = new("Nick", "A quick swipe with a dull blade.", 1, 4, AttackType.Slashing);
            Attack Slash = new("Slash", "A clean slice from a sharp blade.", 1, 6, AttackType.Slashing);
            Attack Firebolt = new("Firebolt", "A small bolt of fire that burns a target.", 1, 6, AttackType.Fire);
            Attack Incinerate = new("Incinerate", "A growing flame that engulfs the target in fire.", 2, 12, AttackType.Fire);
            Attack PoisonSpray = new("Poison Spray", "A sickening cloud of poison clouding that builds up poison.", 1, 4, AttackType.Poison);
            Attack Infect = new("Infect", "A vile infection that massively raises poison levels.", 2, 8, AttackType.Poison);
            Attack Chill = new("Chill", "A chilling blast of cold that numbs and freezes.", 1, 6, AttackType.Cold);
            Attack Frostbite = new("Frostbite", "A frigid blast of bitter cold that numbs and freezes.", 2, 12, AttackType.Cold);
            Attack Arc = new("Arc", "A crackling arc of lightning that shocks the target.", 1, 12, AttackType.Lightning);
            Attack LightningBolt = new("Lightning Bolt", "A crackling bolt of lightning that electrocutes the target.", 2, 24, AttackType.Lightning);
            Attack ArcaneBolt = new("Arcane Bolt", "A simple blast of magic that damages and regenerates mana for the caster.", 1, 4, AttackType.Arcane);
            Attack MagicMissile = new("Magic Missile", "A focused burst of magic that damages and regenerates mana for the caster.", 2, 8, AttackType.Arcane);
            attacksList.Add(Wack);
            attacksList.Add(Crush);
            attacksList.Add(Stab);
            attacksList.Add(Puncture);
            attacksList.Add(Nick);
            attacksList.Add(Slash);
            attacksList.Add(Firebolt);
            attacksList.Add(Incinerate);
            attacksList.Add(PoisonSpray);
            attacksList.Add(Infect);
            attacksList.Add(Chill);
            attacksList.Add(Frostbite);
            attacksList.Add(Arc);
            attacksList.Add(LightningBolt);
            attacksList.Add(ArcaneBolt);
            attacksList.Add(MagicMissile);
        }
    }
}
