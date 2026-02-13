using Godot;
using RuinGamePDT.Core;

namespace RuinGamePDT.Party;

public partial class Party : Node2D
{
    [Signal]
    public delegate void PartyMovedEventHandler(Vector2I oldPos, Vector2I newPos);

    private PartyData? _data;
    private Sprite2D? _sprite;
    private Tween? _moveTween;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
    }

    public void Initialize(PartyData partyData, Vector2I startPosition)
    {
        _data = partyData;
        UpdatePosition(startPosition);
    }

    public void MoveToTile(Vector2I gridPos)
    {
        if (_data == null)
            return;

        Vector2I oldPos = _data.CurrentPosition;

        // Calculate hexagonal world position
        Vector2 targetWorldPos = HexGridToWorld(gridPos);

        // Animate movement with Tween
        if (_moveTween != null && _moveTween.IsValid())
        {
            _moveTween.Kill();
        }

        _moveTween = CreateTween();
        _moveTween.SetTrans(Tween.TransitionType.Quad);
        _moveTween.SetEase(Tween.EaseType.InOut);
        _moveTween.TweenProperty(this, "position", targetWorldPos, Constants.PARTY_MOVE_DURATION);

        // Update data
        _data.MoveTo(gridPos);

        EmitSignal(SignalName.PartyMoved, oldPos, gridPos);

        GD.Print($"Party moved from {oldPos} to {gridPos}");
    }

    public void UpdatePosition(Vector2I gridPos)
    {
        Vector2 worldPos = HexGridToWorld(gridPos);

        Position = worldPos;

        if (_data != null)
        {
            _data.CurrentPosition = gridPos;
        }
    }

    // Convert hexagonal grid coordinates to world position (flat-top hexagons)
    private Vector2 HexGridToWorld(Vector2I gridPos)
    {
        int x = gridPos.X;
        int y = gridPos.Y;

        float xPos = x * Constants.HEX_HORIZONTAL_SPACING;
        float yPos = y * Constants.HEX_VERTICAL_SPACING;

        // Offset odd rows to create hexagonal pattern
        if (y % 2 == 1)
        {
            xPos += Constants.HEX_ODD_ROW_OFFSET;
        }

        return new Vector2(xPos, yPos);
    }

    public PartyData? GetPartyData()
    {
        return _data;
    }
}
