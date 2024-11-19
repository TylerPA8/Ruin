using Ruin;
using Ruin.General;
using static System.Formats.Asn1.AsnWriter;


var attackLibrary = new AttackLibrary(); 
attackLibrary.InitializeAttacks();
List<Attack> emptyattacks = new List<Attack>();
Goblin Goblin1 = new("Goblin", null, 12, 1, 1, null, 50);
Goblin Goblin2 = new("Goblin", null, 12, 2, 2, null, 50);
Goblin Goblin3 = new("Goblin", null, 12, 3, 3, null, 50);
Goblin Goblin4 = new("Goblin", null, 12, 4, 4, null, 50);

List<Creatures> enemies = new List<Creatures> { Goblin1, Goblin2, Goblin3, Goblin4};

Console.WriteLine("Welcome to Ruin\n");
Console.WriteLine("What is your character's name?");
//string nm = Console.ReadLine();
Phalanx Asher = new("Asher", null, 15, 28, 28, [], 1, 0);
//Character Migan = new("Migan", [10, 14, 15, 15], [AttackLibrary.attacksList[2], AttackLibrary.attacksList[6], AttackLibrary.attacksList[7], AttackLibrary.attacksList[14]], 12, 12, 12, 7, 7, 7, 7, [], 1, 0, 2);
List<Creatures> player = new List<Creatures> { Asher };
Asher.DisplayCreature();
Goblin1.DisplayCreature();
//Utilities.SaveGame("MiganPlaythrough", enemies, Migan);
//Utilities.LoadGame("MiganPlaythrough");

Combat.StartCombat(Asher, enemies);
//Migan.LevelCheck();



//Console.WriteLine("Generating Creatures...");

//Goblin2.DisplayCreature();
//Goblin3.DisplayCreature();
//Goblin4.DisplayCreature();    