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
- **Tile Reveal Logic**: Randomizes values, selects top 2 treasures + 1 enemy
