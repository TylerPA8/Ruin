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
        public List<int> usePosition;
        public List<int> targetPosition;
        //public Random roll;

        public Attack (string attackName, int minDmg, int maxDmg, List<int> usePosition, List<int> targetPosition)
        {
            this.attackName = attackName;
            this.minDmg = minDmg;
            this.maxDmg = maxDmg;
            this.usePosition = usePosition;
            this.targetPosition = targetPosition;
        }
    }
}

