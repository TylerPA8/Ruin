using Ruin.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruin
{
    internal static class Utilities
    {
        public static int Rounder(int a)
        {
            int statMod = 0;
            if (a < 10)
            {
                double amod = a / 2;
                statMod = Convert.ToInt32(Math.Floor(amod));
            }
            else
            {
                double amod = a / 2;
                statMod = Convert.ToInt32(Math.Ceiling(amod));
            }

            return statMod;
        }
    }
}
