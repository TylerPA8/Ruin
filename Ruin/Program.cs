using Ruin.General;

Attack.InitializeAttacks();

Console.WriteLine("Generating Creatures...");

Creatures Creature = new Creatures("Creature", null, null);
Creatures Creature2 = new Creatures("Creature 2", null, null);
Creatures Creature3 = new Creatures("Creature 3", null, null);
Creatures Creature4 = new Creatures("Creature 4", null, null);


Creatures.DisplayCreatures();