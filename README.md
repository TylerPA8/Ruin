# Ruin - Roguelike Game

A procedurally generated roguelike game built with Godot 4.x and C#.

## Features Implemented

### Phase 1: Core World System

- **Procedural World Generation**: Heat/elevation map-based biome generation
- **5 Biome Types**: Forest, Plains, Desert, Swamp, Mountains
- **Fog of War**: Tiles start face-down and reveal when scouted
- **Party Movement**: Click adjacent tiles to scout and move
- **Resource System**: Each biome has unique resource multipliers (Wood, Stone, Metal, Gold, Gems)
- **Monster System**: Weighted random monster spawning (Bandits, Beasts, Monstrosities)
- **Tile Reveal Logic**: Matches Python reference - randomizes values, selects top 2 treasures + 1 enemy

## How to Run

1. Open the project in Godot 4.x (with .NET support enabled)
2. Build the C# solution (Build > Build Solution)
3. Press F5 to run the game

## Controls

- **Left Click**: Scout and move to adjacent tiles (within 1 tile range)
- **Mouse Hover**: Highlights tiles within scout range

## Project Structure

```
RuinGamePDT/
├── Scripts/
│   ├── Core/           # GameManager, Constants, MainScene
│   ├── World/          # Tile system, WorldData, BiomeDatabase
│   ├── Generation/     # Procedural generation (heat/elevation maps)
│   ├── Party/          # Party data and movement
│   ├── Resources/      # Enums for resources and monsters
│   └── Input/          # InputController for player interaction
├── Scenes/
│   ├── Main.tscn       # Main game scene
│   ├── World/          # WorldMap and Tile scenes
│   └── Party/          # Party scene
└── Assets/
    └── Sprites/        # Placeholder tile and icon sprites
```

## Biome Multipliers

Based on the Python reference logic:

- **Forest**: High wood (2x), decent stone/gold (1.5x), more bandits/beasts
- **Plains**: Very low resources except stone, high beast spawns (2x)
- **Desert**: Very low resources except gold (2x), high monstrosities (2x)
- **Swamp**: High wood/gold (1.5x), very low bandits, high monstrosities (1.5x)
- **Mountains**: High stone/metal/gold (2x), balanced monster spawns

## Tile Reveal System

When a tile is revealed:
1. Generates random resource values (0-10) multiplied by biome modifiers
2. Selects top 2 highest-value resources as treasures
3. Weighted random selection of 1 enemy type based on biome
4. Displays resource and enemy icons on the tile

## Next Steps (Future Development)

- The Ruin (home base system)
- Settlements (NPC outposts with quests and trade)
- Nests (monster spawners with dungeon events)
- Roaming Monsters (dynamic encounters)
- Combat system
- Inventory management UI
- Quest system
- Resource gathering and crafting

## Technical Details

- **Engine**: Godot 4.3
- **Language**: C# (.NET 8.0)
- **Architecture**: 3-layer design (Data, Generation, Visual)
- **World Generation**: Multi-stage noise-based procedural generation
- **Scout Range**: Chebyshev distance (max of X/Y differences)
