using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ruin.Creatures
{
    internal class Attack
    {
        private string attackName = string.Empty;
        private string attackDescription = string.Empty;
        private int minDmg;
        private int maxDmg;
        private AttackType attackType;
        private Random roll;

        internal string AttackName { get; private set; }
        public string AttackDescription { get; private set; }
        public int MinDmg { get; private set; }
        public int MaxDmg { get; private set; }
        public AttackType AttackType { get; private set; }

        public Attack (string attackName, string attackDescription, int minDmg, int maxDmg, AttackType attackType)
        {
        }

        public string GetDescription() => this.attackDescription;
        public int RollDamage(Attack attack) => roll.Next(this.minDmg, (this.maxDmg + 1));
    }
}

