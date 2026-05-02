using RuinGamePDT.Combat;

namespace RuinGamePDT.Creatures;

public class PricklebackGoblin : Monstrosity
{
    public PricklebackGoblin() : base("Prickleback Goblin", 6, 6, 2, 3, 3)
    {
        CombatStats.MovementPoints  += 11;
        CombatStats.CritDamageBonus *= 1.5f;

        Attacks.Add(new Attack("Scratch", 1, 3, 1, 1));
        Attacks.Add(new Attack("Skewer",  2, 4, 2, 5,
            onCrit: new AttackEffect(AttackEffectType.MovementSpeedReduction, 1f, 3f, 1, 2)));
        Attacks.Add(new Attack("Grub Goo", 0, 1, 1, 3,
            onHit: new AttackEffect(AttackEffectType.AccuracyReduction, 5f, 5f, 1, 4)));
    }
}
