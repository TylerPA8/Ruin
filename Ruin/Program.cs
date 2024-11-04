using Ruin;
using Ruin.General;


var attackLibrary = new AttackLibrary(); 
attackLibrary.InitializeAttacks();
List<Attack> emptyattacks = new List<Attack>();
Goblinoid Goblin = new("Goblin", null, emptyattacks);
Goblin.DisplayCreature();
//Goblinoid Goblin2 = new("Goblin", null, emptyattacks);
//Goblinoid Goblin3 = new("Goblin", null, emptyattacks);
//Goblinoid Goblin4 = new("Goblin", null, emptyattacks);

Console.WriteLine("Welcome to Ruin\n");
Console.WriteLine("What is your character's name?");
string nm = Console.ReadLine();
Console.WriteLine("\nGenerating your hero...\n");
Character Character1 = new(nm, null, emptyattacks);
Character1.DisplayCreature();
Character1.AttackRoll(AttackLibrary.attacksList[5], 2, Goblin);
Goblin.DisplayCreature();



//switch(ans)

//Console.WriteLine("Generating Creatures...");

//Goblin2.DisplayCreature();
//Goblin3.DisplayCreature();
//Goblin4.DisplayCreature();