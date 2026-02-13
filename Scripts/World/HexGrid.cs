using Godot;
using RuinGamePDT.Core;

namespace RuinGamePDT.World;

public partial class HexGrid : Node2D
{
    private int _width;
    private int _height;
    private Color _gridColor = new Color(0.3f, 0.3f, 0.3f, 0.5f); // Semi-transparent gray
    private float _lineWidth = 1.0f;

    public void Initialize(int width, int height)
    {
        _width = width;
        _height = height;
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (_width == 0 || _height == 0)
            return;

        // Draw hexagonal grid lines
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                DrawHexOutline(x, y);
            }
        }
    }

    private void DrawHexOutline(int gridX, int gridY)
    {
        // Calculate hexagonal world position
        float xPos = gridX * Constants.HEX_HORIZONTAL_SPACING;
        float yPos = gridY * Constants.HEX_VERTICAL_SPACING;

        // Offset odd rows
        if (gridY % 2 == 1)
        {
            xPos += Constants.HEX_ODD_ROW_OFFSET;
        }

        Vector2 center = new Vector2(xPos, yPos);

        // Hexagon vertices for 150px tall equilateral hexagon (pointy-top orientation)
        Vector2[] vertices = new Vector2[6];
        vertices[0] = center + new Vector2(0, -75);      // Top
        vertices[1] = center + new Vector2(65, -37.5f);  // Top-right
        vertices[2] = center + new Vector2(65, 37.5f);   // Bottom-right
        vertices[3] = center + new Vector2(0, 75);       // Bottom
        vertices[4] = center + new Vector2(-65, 37.5f);  // Bottom-left
        vertices[5] = center + new Vector2(-65, -37.5f); // Top-left

        // Draw hexagon outline
        for (int i = 0; i < 6; i++)
        {
            Vector2 start = vertices[i];
            Vector2 end = vertices[(i + 1) % 6];
            DrawLine(start, end, _gridColor, _lineWidth);
        }
    }
}
