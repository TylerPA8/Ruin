using System.Collections.Generic;
using Godot;
using RuinGamePDT.Resources;

namespace RuinGamePDT.Party;

public class PartyData
{
    public Vector2I CurrentPosition { get; set; }
    public Dictionary<ResourceType, int> Inventory { get; set; }
    public int MembersCount { get; set; }
    public float Health { get; set; }

    public PartyData(Vector2I startPosition)
    {
        CurrentPosition = startPosition;
        Inventory = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wood, 0 },
            { ResourceType.Stone, 0 },
            { ResourceType.Metal, 0 },
            { ResourceType.Gold, 0 },
            { ResourceType.Gems, 0 }
        };
        MembersCount = 4;
        Health = 100.0f;
    }

    public void MoveTo(Vector2I newPosition)
    {
        CurrentPosition = newPosition;
    }

    public void AddResources(Dictionary<ResourceType, int> resources)
    {
        foreach (var kvp in resources)
        {
            if (Inventory.ContainsKey(kvp.Key))
            {
                Inventory[kvp.Key] += kvp.Value;
            }
            else
            {
                Inventory[kvp.Key] = kvp.Value;
            }
        }
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (Inventory.ContainsKey(type))
        {
            Inventory[type] += amount;
        }
        else
        {
            Inventory[type] = amount;
        }
    }
}
