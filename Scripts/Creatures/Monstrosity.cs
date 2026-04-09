namespace RuinGamePDT.Creatures;

public class Monstrosity : Creature
{
    protected override int StatCap => 10;

    public Monstrosity(int agility = 1, int focus = 1, int mind = 1, int strength = 1, int toughness = 1)
        : base("Monstrosity", agility, focus, mind, strength, toughness) { }
}
