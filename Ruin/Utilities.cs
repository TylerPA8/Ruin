using Ruin.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ruin
{
    internal class Utilities
    {
        public Dictionary<int, int> statArray = new Dictionary<int, int>() { { 1, -5 }, { 2, -4 }, { 3, -4 }, { 4, -3 }, { 5, -3 }, { 6, -2 }, { 7, -2 }, { 8, -1 }, { 9, -1 }, { 10, 0 }, { 11, 0 }, { 12, 1 }, { 13, 1 }, { 14, 2 }, { 15, 2 }, { 16, 3 }, { 17, 3 }, { 18, 4 }, { 19, 4 }, { 20, 5 } };

        public static void NewGame()
        {

        }
        public static void SaveGame(string saveName)
        {
            if (File.Exists(@$"C:\Ruin\{saveName}.txt"))
            {
                string tempFile = @"C:\Ruin\GameStateTemp.txt";
                File.Copy(saveName, tempFile);
                List<Attack> attacks = AttackLibrary.attacksList;
                //List<Creatures> creatures = Creatures.GetCreatures();
                //WriteSave(saveName, attacks, creatures);

             }
            else
            {
                
            }
        }
        public static void LoadGame()

        {
            //TODO write method to be called to write file to save game
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
