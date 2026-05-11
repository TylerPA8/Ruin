namespace RuinGamePDT.Combat;

public enum ReactionType
{
    OnMissCounter
}

public record Reaction(ReactionType Type, string? LinkedAttackName);