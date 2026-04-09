namespace RuinGamePDT.Creatures;

public class Beast : Creature
{
    protected override int StatCap => 10;

    public Beast(int agility = 1, int focus = 1, int mind = 1, int strength = 1, int toughness = 1)
        : base("Beast", agility, focus, mind, strength, toughness) { }
}
