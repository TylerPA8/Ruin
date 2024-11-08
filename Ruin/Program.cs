using Ruin;
using Ruin.General;


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
Character Character1 = new(nm, [12, 1, 15, 2, 15, 2, 12, 1], [AttackLibrary.attacksList[2], AttackLibrary.attacksList[4], AttackLibrary.attacksList[5], AttackLibrary.attacksList[16]], 12, 11, 11, 15, 15, 12, 12, [],2);
Character1.DisplayCreature();

Creatures target = Character1.SelectTarget(enemies);
Character1.AttackRoll(AttackLibrary.attacksList[5], target);

//Console.WriteLine("Generating Creatures...");

//Goblin2.DisplayCreature();
//Goblin3.DisplayCreature();
//Goblin4.DisplayCreature();