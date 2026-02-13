using Godot;
using RuinGamePDT.Core;
using RuinGamePDT.World;
using RuinGamePDT.Party;
using System;

namespace RuinGamePDT.Input;

public partial class InputController : Node
{
	private WorldMap? _worldMap;
	private Party.Party? _party;

	private Tile? _hoveredTile;

	public override void _Ready()
	{
		// Get references to other nodes
		_worldMap = GetNode<WorldMap>("../WorldMap");

		// Wait a frame for everything to initialize
		CallDeferred(nameof(DeferredReady));
	}

	private void DeferredReady()
	{
		// Try to find party node
		_party = GetNodeOrNull<Party.Party>("../Party");

		if (_party == null)
		{
			GD.PrintErr("InputController: Party node not found!");
		}

		if (_worldMap == null)
		{
			GD.PrintErr("InputController: WorldMap node not found!");
		}

		// Connect to tile clicked signals
		if (_worldMap != null)
		{
			ConnectTileSignals();
		}
	}

	private void ConnectTileSignals()
	{
		// Tiles are already connected in WorldMap, we'll handle clicks there
	}

	public override void _Input(InputEvent @event)
	{
		// Handle mouse clicks for tile interaction
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
			{
				HandleMouseClick(mouseButton.Position);
			}
		}
	}

	public override void _Process(double delta)
	{
		UpdateTileHighlights();
	}

	private void HandleMouseClick(Vector2 screenPos)
	{
		if (_worldMap == null || _party == null)
			return;

		var worldData = _worldMap.GetWorldData();
		var partyData = _party.GetPartyData();

		if (worldData == null || partyData == null)
			return;

		// Convert screen position to world position using the viewport's camera
		Vector2 worldPos = GetViewport().GetCamera2D()?.GetGlobalMousePosition() ?? screenPos;

		// Convert world position to hexagonal grid position
		Vector2I gridPos = WorldToHexGrid(worldPos);

		// Check if tile exists
		var tileData = worldData.GetTile(gridPos);
		if (tileData == null)
		{
			GD.Print("Clicked outside world bounds");
			return;
		}

		// Check if within scout range
		int distance = worldData.GetDistance(partyData.CurrentPosition, gridPos);

		if (distance > Constants.SCOUT_RANGE)
		{
			GD.Print($"Too far to scout! Distance: {distance}, Max: {Constants.SCOUT_RANGE}");
			return;
		}

		// Valid tile click within range
		HandleTileClick(gridPos, tileData);
	}

	private void HandleTileClick(Vector2I gridPos, WorldTile tileData)
	{
		if (_worldMap == null || _party == null)
			return;

		var tileNode = _worldMap.GetTileNode(gridPos);
		if (tileNode == null)
			return;

		// Reveal tile if not already revealed
		if (!tileData.IsRevealed)
		{
			tileData.Reveal();
			tileNode.UpdateVisual();  // Update to show revealed sprite
			GD.Print($"Revealed tile: {tileData}");
		}

		// Move party to tile (camera will follow automatically since it's attached to party)
		_party.MoveToTile(gridPos);
		tileData.IsExplored = true;
	}

	private void UpdateTileHighlights()
	{
		if (_worldMap == null || _party == null)
			return;

		var worldData = _worldMap.GetWorldData();
		var partyData = _party.GetPartyData();

		if (worldData == null || partyData == null)
			return;

		// Get mouse position in world coordinates using the viewport's camera
		var camera = GetViewport().GetCamera2D();
		if (camera == null)
			return;

		Vector2 mouseWorldPos = camera.GetGlobalMousePosition();
		Vector2I mouseGridPos = WorldToHexGrid(mouseWorldPos);

		// Clear previous highlight
		if (_hoveredTile != null)
		{
			_hoveredTile.ShowHighlight(false);
			_hoveredTile = null;
		}

		// Check if mouse is over a valid tile
		var hoveredWorldTile = worldData.GetTile(mouseGridPos);
		if (hoveredWorldTile == null)
			return;

		// Check if within scout range
		int distance = worldData.GetDistance(partyData.CurrentPosition, mouseGridPos);
		if (distance <= Constants.SCOUT_RANGE)
		{
			var tileNode = _worldMap.GetTileNode(mouseGridPos);
			if (tileNode != null)
			{
				tileNode.ShowHighlight(true);
				_hoveredTile = tileNode;
			}
		}
	}

	// Convert world position to hexagonal grid coordinates (pointy-top hexagons)
	private Vector2I WorldToHexGrid(Vector2 worldPos)
	{
		// First approximation based on row
		int row = (int)Math.Round(worldPos.Y / Constants.HEX_VERTICAL_SPACING);

		// Adjust x based on whether row is odd or even
		float xOffset = (row % 2 == 1) ? Constants.HEX_ODD_ROW_OFFSET : 0;
		int col = (int)Math.Round((worldPos.X - xOffset) / Constants.HEX_HORIZONTAL_SPACING);

		return new Vector2I(col, row);
	}
}
