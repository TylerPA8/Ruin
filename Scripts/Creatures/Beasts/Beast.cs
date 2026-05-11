namespace RuinGamePDT.Creatures;

public class Beast : Creature
{
    protected override int StatCap => 12;

    public Beast(int agility = 1, int focus = 1, int mind = 1, int strength = 1, int stamina = 1)
        : base("Beast", agility, focus, mind, strength, stamina) { }

    protected Beast(string name, int agility, int focus, int mind, int strength, int stamina)
        : base(name, agility, focus, mind, strength, stamina) { }
}
