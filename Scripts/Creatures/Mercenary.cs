namespace RuinGamePDT.Creatures;

public class Mercenary : Creature
{
    protected override int StatCap => 10;

    public Mercenary(int agility = 1, int focus = 1, int mind = 1, int strength = 1, int toughness = 1)
        : base("Mercenary", agility, focus, mind, strength, toughness) { }
}
