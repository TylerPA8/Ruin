using Ruin.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin
{
    internal class Status
    {
        public string statusName = string.Empty;
        public int effectNumber;
        public string effectTarget;
        public int effectDuration;
        public Status(string statusName, int effectNumber, string effectTarget, int effectDuration)
        {
            
        }
        internal void applyPoison(int dmgAmount, Creatures target, int effectDuration, int acReduction = 2)
        {
            //Poison does a mild dmg over time effect and lowers the target's AC.
        }
        internal void applyBurn(int dmgAmount, Creatures target, int effectDuration)
        {
            //Burn does strong dmg over time effect and lower the targets chance to hit.
        }

        internal void applyChilled(int effectAmount, Creatures target, int effectDuration)
        {
            //Chill lowers the targets chance to hit and AC and applies a large dmg effect on the next bludgeoning attack.
        }

        internal void applyShocked(int effectAmount, Creatures target, int effectDuration)
        {
            //Shocked strongly reduces AC and chance to hit.
        }

        internal void applyStunned(Creatures target, int effectDuration)
        {
            //Stunned causes the next attack to deal a critical attack.
        }

        internal void applyRegenerating(int effectAmount, Creatures target, int effectDuration)
        {
            //Regen gives the target a lingering heal effect.
        }

        internal void applyCursed(int effectAmount, Creatures target, int effectDuration)
        {
            //Curse reduces chance to hit by a varying amount each turn for an extended duration.
        }

        internal void applyBlesses(int effectAmount, Creatures target, int effectDuration)
        {
            //Curse increases chance to hit by a varying amount each turn for an extended duration.
        }

        internal void applyWounded(int effectAmount, Creatures target, int effectDuration)
        {
            //Wound nullifies the next instance of healing.
        }

        internal void removeStatus(Creatures target, Status status) 
        {
        
        }
    }
}
