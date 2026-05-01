namespace RuinGamePDT.Creatures;

public class Bandit : Creature
{
    protected override int StatCap => 6;

    public Bandit(int agility = 1, int focus = 1, int mind = 1, int strength = 1, int stamina = 1)
        : base("Bandit", agility, focus, mind, strength, stamina) { }
}