using Ruin;
using Ruin.General;


var attackLibrary = new AttackLibrary(); 
attackLibrary.InitializeAttacks();

Console.WriteLine("Generating Creatures...");
List <Attack> emptyattacks = new List<Attack>();    
Goblinoid Goblin = new ("Goblin", null, emptyattacks);
Goblinoid Goblin2 = new("Goblin", null, emptyattacks);
Goblinoid Goblin3 = new("Goblin", null, emptyattacks);
Goblinoid Goblin4 = new("Goblin", null, emptyattacks);


Goblin.DisplayCreature();
Goblin2.DisplayCreature();
Goblin3.DisplayCreature();
Goblin4.DisplayCreature();