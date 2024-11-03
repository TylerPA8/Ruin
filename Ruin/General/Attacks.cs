using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ruin.General
{
    internal class Attack
    {
        public string attackName = string.Empty;
        public string attackDescription = string.Empty;
        public int minDmg;
        public int maxDmg;
        public AttackType attackType;
        //public Random roll;

        public Attack (string attackName, string attackDescription, int minDmg, int maxDmg, AttackType attackType)
        {
            this.attackName = attackName;
            this.attackDescription = attackDescription;
            this.minDmg = minDmg;
            this.maxDmg = maxDmg;
            this.attackType = attackType;
        }

        //public int RollDamage(Attack attack) => roll.Next(this.minDmg, (this.maxDmg + 1));
    }
}

