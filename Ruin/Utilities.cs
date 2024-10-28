using Ruin.Creatures;
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
            //TODO write method to be called to write file to save game
            string fileName = @$"C:\Ruin\{saveName}.txt";
            try
            {
                if (File.Exists(fileName))
                {
                    string tempFile = @"C:\Ruin\GameStateTemp.txt";
                    //TODO : Write method for copying game state so user can reload.
                }
                else
                {
                    using (TextWriter tw = new StreamWriter(fileName))
                    {
                        foreach (var item in objects.List)
                        {
                            tw.WriteLine(string.Format($"Name: {0}\n{1}: {1}", item.Name, item.curhp.ToString(), item.maxhp.ToString(), item.ac.ToString(), item.stats.ToString(), item.attacks.ToString()));
                        }
                    }
                }
            }
        }
        public static void LoadGame()
        {
            //TODO write method to be called to write file to save game
        }
    }
}
