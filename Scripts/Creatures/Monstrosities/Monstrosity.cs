namespace RuinGamePDT.Creatures;

public class Monstrosity : Creature
{
    protected override int StatCap => 14;

    public Monstrosity(int agility = 1, int focus = 1, int mind = 1, int strength = 1, int stamina = 1)
        : base("Monstrosity", agility, focus, mind, strength, stamina) { }

    protected Monstrosity(string name, int agility, int focus, int mind, int strength, int stamina)
        : base(name, agility, focus, mind, strength, stamina) { }
}
