using Ruin.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin.Utilities
{
    internal class Utilities
    {
        public string GenerateName()
        {
            return this.GetType().Name;
        }
        public static List<int> GenerateStatArray()
        {
            Random random = new Random();
            //TODO
            return new List<int>();
        }
        public static int GenerateMaxhp()
        {
            //TODO
            return 0;
        }
        public static int GenerateAc()
        {
            //TODO
            return 0;
        }
        public static List<Attacks> GenerateAttacks()
        {
            //TODO
            return new List<Attacks>();
        }
    }
}
