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
string nm = Console.ReadLine();

Console.WriteLine("\nGenerating your hero...\n");
//Character Character1 = new(nm, null, emptyattacks);
Character Character1 = new(nm, [12, 15, 15, 12], [AttackLibrary.attacksList[2], AttackLibrary.attacksList[4], AttackLibrary.attacksList[5], AttackLibrary.attacksList[16]], 12, 11, 11, 7, 7, 6, 6,[],2);
Character1.DisplayCreature();
Combat.StartCombat(Character1, enemies);


//Console.WriteLine("Generating Creatures...");

//Goblin2.DisplayCreature();
//Goblin3.DisplayCreature();
//Goblin4.DisplayCreature();    