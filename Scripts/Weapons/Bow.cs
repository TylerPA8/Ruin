using RuinGamePDT.Combat;
using RuinGamePDT.Creatures;
using RuinGamePDT.Resources;

namespace RuinGamePDT.Weapons;

public class Bow : Weapon
{
    public Bow() : base(WeaponType.Bow)
    {
        // Loose - reliable single-target shot, bleed on crit
        Attacks.Add(new Attack(
            name: "Loose",
            minDamage: 2,
            maxDamage: 7,
            actionPointCost: 1,
            accuracy: 95,
            attackShape: new AttackShape(new[] { (0, 0) }),
            range: 15,
            reaction: null,
            onHit: null,
            onCrit: new AttackEffect(AttackEffectType.Bleed, CombatStat.HitPoints, 3, 3, 2, 2),
            minRange: 3
        ));

        // Deadeye - accuracy penalty, very long range, every hit becomes a crit
        Attacks.Add(new Attack(
            name: "Deadeye",
            minDamage: 5,
            maxDamage: 10,
            actionPointCost: 2,
            accuracy: 60,
            attackShape: new AttackShape(new[] { (0, 0) }),
            range: 20,
            reaction: null,
            onHit: null,
            onCrit: null,
            minRange: 3,
            autoCrit: true
        ));

        // Suppression - 3x3 burst around target, near-guaranteed hit, slows on crit
        Attacks.Add(new Attack(
            name: "Suppression",
            minDamage: 1,
            maxDamage: 2,
            actionPointCost: 2,
            accuracy: 200,
            attackShape: new AttackShape(BurstShape3x3()),
            range: 20,
            reaction: null,
            onHit: null,
            onCrit: new AttackEffect(AttackEffectType.StatReduction, CombatStat.MovementPoints, 2, 2, 1, 1),
            minRange: 3
        ));

        // Pincushion - multi-hit volley, bleed per hit
        Attacks.Add(new Attack(
            name: "Pincushion",
            minDamage: 1,
            maxDamage: 2,
            actionPointCost: 3,
            accuracy: 70,
            attackShape: new AttackShape(new[] { (0, 0) }),
            range: 15,
            reaction: null,
            onHit: new AttackEffect(AttackEffectType.Bleed, CombatStat.HitPoints, 2, 2, 2, 2),
            onCrit: null,
            minRange: 3,
            minHits: 3,
            maxHits: 5
        ));
    }

    private static IEnumerable<(int, int)> BurstShape3x3()
    {
        var offsets = new List<(int, int)>();
        for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
                offsets.Add((dx, dy));
        return offsets;
    }
}
