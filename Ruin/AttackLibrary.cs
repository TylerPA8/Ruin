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
    public static readonly List<Attack> PhalanxAttacks = new List<Attack>();
    public static readonly List<Attack> WeaverAttacks = new List<Attack>();
    public static readonly List<Attack> EclipseAttacks = new List<Attack>();
    public static readonly List<Attack> GoblinAttacks = new List<Attack>();
        public static readonly List<Attack> GoblinMageAttacks = new List<Attack>();

        internal void InitializeAttacks()
        {
            //For targetting, 1 indicates a valid target, 0 is not available.
            //So far the statuses I need are: Bleed, Stun, Burn, Freeze, a way to mark a target (to increase or reduce damage), Poison, Concentration(?), Barrier, Fear (disadvantage, can't use forward attacks), Blind, and Movement.


            //Phalanx - Sturdy and average damage, bleeds chance, back line blaster build option
            Attack Puncture = new("Puncture", 0, 0, [0, 1, 1, 1], [1, 1, 1, 1]); //Test Puncture
            //Attack Puncture = new("Puncture", 3, 8, [0, 1, 1, 1], [1, 1, 1, 1]); //Basic damage, pushes target back
            Attack Skewer = new("Skewer", 2, 4, [0, 0, 1, 1], [1, 1, 0, 0]); //Bleed chance, hits target and 1 behind it.
            Attack ShieldBash = new("Shield Bash", 1, 4, [0, 1, 1, 1], [1, 1, 1, 1]); //Stun chance
            Attack Brace = new("Brace", 0, 0, [0, 1, 1, 1], [1, 1, 1, 1]); //Reduces all attacks by 1-4 damage.
            Attack Hurl = new("Hurl", 1, 6, [1, 1, 1, 0], [0, 0, 1, 1]); //Ranged backline attack, knocks target back, chance to bleed.
            Attack PointedCurse = new("Pointed Curse", 1, 4, [1, 1, 1, 0], [1, 1, 1, 1]); //Marks a target for 3 rounds, dealing an additional 1-4 damage per attacks
            Attack CursedBlast = new("Cursed Blast", 1, 10, [1, 1, 1, 0], [1, 1, 1, 1]); //Basic damage
            Attack Cover = new("Cover", 0, 0, [0, 0, 0, 1], [1, 1, 1, 0]); //Blocks next 2 attacks on ally.
            PhalanxAttacks.Add(Puncture);
            PhalanxAttacks.Add(Skewer);
            PhalanxAttacks.Add(ShieldBash);
            PhalanxAttacks.Add(Brace);
            PhalanxAttacks.Add(Hurl);
            PhalanxAttacks.Add(PointedCurse);
            PhalanxAttacks.Add(CursedBlast);
            PhalanxAttacks.Add(Cover);

            //Weaver - High damage but low hp character, status applier build option
            Attack PoisonSpray = new("Poison Spray", 1, 4, [0, 0, 1, 1], [1, 1, 0, 0]); //Small damage, builds up poison, back 1
            Attack Incinerate = new("Incinerate", 2, 12, [0, 1, 1, 1], [1, 1, 1, 0]); //High damage, chance to burn
            Attack Arc = new("Arc", 1, 4, [1, 1, 1, 1], [1, 1, 1, 1]); //On hit, chains and hits additional targets until a miss, stun chance
            Attack LightningBolt = new("Lightning Bolt", 4, 16, [1, 1, 0, 0], [1, 1, 1, 1]); //Very high damage, but unable to pick target, can hit empty space
            Attack ArcaneBolt = new("ArcaneBolt", 1, 4, [1, 1, 1, 0], [1, 1, 1, 1]); //Always hits, damage not reduced
            Attack Conduit = new("Conduit", 0, 0, [1, 1, 1, 0], [1, 1, 1, 1]); //Marks a target for 3 rounds, magic damage to target fires an Arcane Bolt at a nearby enemy
            Attack Chill = new("Chill", 1, 8, [0, 1, 1, 1], [1, 1, 1, 0]); //Applies freeze stacks
            Attack Overcharge = new("Overcharge", 0, 0, [1, 1, 1, 0], [0, 1, 1, 1]); //Attacks are rolled at advantage until hit, increases crit by +1
            WeaverAttacks.Add(PoisonSpray);
            WeaverAttacks.Add(Incinerate);
            WeaverAttacks.Add(Arc);
            WeaverAttacks.Add(LightningBolt);
            WeaverAttacks.Add(ArcaneBolt);
            WeaverAttacks.Add(Conduit);
            WeaverAttacks.Add(Chill);
            WeaverAttacks.Add(Overcharge);

            //Eclipse - Healing, buffing, and debuffing class. Low damage but high utility
            Attack Shroud = new("Shroud", 1, 4, [1, 1, 1, 0], [1, 1, 1, 1]); //Small heal and dodge bonus on ally
            Attack HealingLight = new("Healing Light", 1, 4, [1, 1, 1, 0], [1, 1, 1, 1]); //Places a beam of moonlight that heals the target in the selected slot
            Attack SilverBarrier = new("Silver Barrier", 2, 12, [1, 1, 1, 0], [0, 1, 1, 1]); //Places a barrier on the target, absorbing X damage
            Attack BlindingRing = new("Blinding Ring", 0, 0, [1, 1, 1, 1], [1, 1, 1, 1]); //Reduces the targets accuracy for 2 rounds
            Attack EmptySky = new("Empty Sky", 1, 4, [1, 1, 1, 1], [1, 1, 1, 1]); //Deals small damage and fears a target
            Attack CresentShield = new("Cresent Shield", 0, 0, [1, 1, 0, 0], [0, 1, 1, 1]); //Provides a shield equal to the targets missing hp, lasts for 1 round
            Attack SweepingBlade = new("Sweeping Blade", 1, 6, [0, 0, 1, 1], [1, 1, 1, 0]); //Damage and a chance to bleed and blind
            Attack SolarFlare = new("Solar Flare", 1, 4, [1, 1, 0, 0], [0, 1, 1, 0]); //Hits 1 target on either side, dealing damage and blinding
            EclipseAttacks.Add(Shroud);
            EclipseAttacks.Add(HealingLight);
            EclipseAttacks.Add(SilverBarrier);
            EclipseAttacks.Add(BlindingRing);
            EclipseAttacks.Add(EmptySky);
            EclipseAttacks.Add(CresentShield);
            EclipseAttacks.Add(SweepingBlade);
            EclipseAttacks.Add(SolarFlare);

            //Goblin Attacks
            Attack TaintedShank = new("Tainted Shank", 1, 4, [1,1,0,0], [0,0,1,1]); //Weak, also deals low poison
            Attack ChokeCloud = new("Choke Cloud", 0, 1, [1, 0, 0, 0], [0, 0, 0, 1]); //Low damage, but lowers accuracy of target
            Attack Volley = new("Volley", 1, 6, [0, 0, 1, 1], [1, 1, 1, 0]); //Basic damage backline hit.
            GoblinAttacks.Add(TaintedShank);
            GoblinAttacks.Add(ChokeCloud);
            GoblinAttacks.Add(Volley);

            //Goblin Mage Attacks
            Attack Infect = new("Infect", 2, 8, [0, 0, 1, 1], [0, 1, 1, 1]); //Applies a heavy poison
            GoblinMageAttacks.Add(TaintedShank);
            GoblinMageAttacks.Add(ChokeCloud);
            GoblinMageAttacks.Add(PoisonSpray);
            GoblinMageAttacks.Add(Infect);

        }
    }
}
