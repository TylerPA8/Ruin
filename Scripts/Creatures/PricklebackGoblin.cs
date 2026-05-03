using RuinGamePDT.Combat;

namespace RuinGamePDT.Creatures;

public class PricklebackGoblin : Monstrosity
{
    public PricklebackGoblin() : base("Prickleback Goblin", 6, 4, 1, 3, 3)
    {
        CombatStats.MovementPoints += 11;
        CombatStats.CritChance += 10;

        Attacks.Add(new Attack("Scratch", 1, 3, 1, 1));
        Attacks.Add(new Attack("Skewer",  2, 4, 2, 5,
            onHit: new AttackEffect(AttackEffectType.Bleed, 0.05f, 0.05f, 1, 1)));
        Attacks.Add(new Attack("Quill Spray", 0, 1, 1, 3,
            onHit: new AttackEffect(AttackEffectType.Bleed, 0.25f, 0.25f, 1, 1)));
    }
}