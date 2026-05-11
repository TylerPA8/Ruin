using RuinGamePDT.Weapons;

namespace RuinGamePDT.Creatures;

public class Mercenary : Creature
{
    protected override int StatCap => 10;

    public Mercenary(int agility = 1, int focus = 1, int mind = 1, int strength = 1, int stamina = 1)
        : base("Mercenary", agility, focus, mind, strength, stamina)
    {
        EquippedWeapon = new Unarmed();
        Attacks.AddRange(EquippedWeapon.Attacks);
    }

    protected Mercenary(string name, int agility, int focus, int mind, int strength, int stamina)
        : base(name, agility, focus, mind, strength, stamina) { }
}
