using Ruin.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ruin
{
    internal class Utilities
    {
        public Dictionary<int, int> statArray = new Dictionary<int, int>() { { 1, -5 }, { 2, -4 }, { 3, -4 }, { 4, -3 }, { 5, -3 }, { 6, -2 }, { 7, -2 }, { 8, -1 }, { 9, -1 }, { 10, 0 }, { 11, 0 }, { 12, 1 }, { 13, 1 }, { 14, 2 }, { 15, 2 }, { 16, 3 }, { 17, 3 }, { 18, 4 }, { 19, 4 }, { 20, 5 } };
        public static readonly Dictionary<int, int> levelChart = new Dictionary<int, int>() { { 1, 250 }, { 2, 500 }, { 3, 1000 }, { 4, 2000 }, { 5, 4000 }, { 6, 8000 }, { 7, 16000 }, { 8, 32000 }, { 9, 64000 } };

        public static void NewGame()
        {

        }

        public static void SaveGame(string saveName, List<Creatures> enemies, Creatures player)
        {
            if (File.Exists(@$"C:\Ruin\{saveName}.txt"))
            {
                Console.WriteLine("Save file already exists. Overwriting.");
                string tempFile = @"C:\Ruin\GameStateTemp.txt";
                //File.Copy(saveName, tempFile);
                string path = ($@"C:\Ruin\{saveName}.txt");
                CreatureWrite(player);
                foreach (Creatures e in enemies)
                {
                    CreatureWrite(e);
                }
            }
            else
            {
                File.Create(@$"C:\Ruin\{saveName}.txt");
                string path = ($@"C:\Ruin\{saveName}.txt");
                CreatureWrite(player);
                foreach (Creatures e in enemies)
                {
                    CreatureWrite(e);
                }
            }
        }
        public static void LoadGame(string saveName)

        {
            string path = @$"C:\Ruin\{saveName}.txt";
            string[] lines = File.ReadAllLines(path);
            foreach (string l in lines)
            {
                Console.WriteLine(l);
            }
        }

        public static StringBuilder CreatureWrite(Creatures c)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{c.Name}, [");
            foreach (Attack a in c.Attacks)
                if (a != (c.Attacks.Last()))
                {
                    sb.Append($"{a.attackName}, ");
                }
                else
                {
                    sb.Append($"{a.attackName}], ");
                }
            sb.Append($"{c.Ac}, {c.MaxHp}, {c.CurHp}, {c.Status.ToString()}, ");
            return sb; 
        }
        //public static void WriteSave(string saveName, List<Attack> attacks, List<Creatures> creatures)
        //{
        //    string fileName = @$"C:\Ruin\{saveName}.txt";
        //    using (TextWriter tw = new StreamWriter(fileName))
        //    {
        //        foreach (var item in attacks)
        //        {
        //            tw.WriteLine(string.Format($"{0}, {1}, {2}, {3}, {4}\n", item.attackName.ToString(), item.attackDescription, item.minDmg.ToString(), item.maxDmg.ToString(), item.attackType.ToString()));
        //        }
        //        foreach (var item in creatures)
        //        {
        //            tw.WriteLine(string.Format($"{0}, {1}, {2}, {3}, {4}, {5}\n", item.name, item.curhp.ToString(), item.maxhp.ToString(), item.ac.ToString(), item.stats.ToString(), item.attacks.ToString()));
        //        }
        //    }
        //}
    }
}
