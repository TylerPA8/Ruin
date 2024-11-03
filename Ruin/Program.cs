using Ruin;
using Ruin.General;


var attackLibrary = new AttackLibrary(); 
attackLibrary.InitializeAttacks();

Console.WriteLine("Generating Creatures...");
List <Attack> emptyattacks = new List<Attack>();    
Creatures Creature = new Creatures("Creature", null, emptyattacks);
Creatures Creature2 = new Creatures("Creature 2", null, emptyattacks);
Creatures Creature3 = new Creatures("Creature 3", null, emptyattacks);
Creatures Creature4 = new Creatures("Creature 4", null, emptyattacks);


Creatures.DisplayCreatures();