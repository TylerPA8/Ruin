using Ruin;
using Ruin.General;
using static System.Formats.Asn1.AsnWriter;


var attackLibrary = new AttackLibrary(); 
attackLibrary.InitializeAttacks();
List<Attack> emptyattacks = new List<Attack>();
Goblinoid Goblin = new("Goblin", null, emptyattacks);
Goblinoid Goblin2 = new("Goblin", null, emptyattacks);
List<Creatures> enemies = new List<Creatures> { Goblin, Goblin2};

Console.WriteLine("Welcome to Ruin\n");
Console.WriteLine("What is your character's name?");
//string nm = Console.ReadLine();

Console.WriteLine("\nGenerating your hero...\n");
//Character Character1 = new(nm, null, emptyattacks);
//Character Asher = new("Asher", [12, 15, 15, 12], [AttackLibrary.attacksList[2], AttackLibrary.attacksList[4], AttackLibrary.attacksList[5], AttackLibrary.attacksList[12]], 12, 12, 12, 7, 7, 6, 6,[],2);
Character Migan = new("Migan", [10, 14, 15, 15], [AttackLibrary.attacksList[2], AttackLibrary.attacksList[6], AttackLibrary.attacksList[7], AttackLibrary.attacksList[14]], 12, 12, 12, 7, 7, 7, 7, [], 2);
Migan.DisplayCreature();
Combat.StartCombat(Migan, enemies);


//Console.WriteLine("Generating Creatures...");

//Goblin2.DisplayCreature();
//Goblin3.DisplayCreature();
//Goblin4.DisplayCreature();    