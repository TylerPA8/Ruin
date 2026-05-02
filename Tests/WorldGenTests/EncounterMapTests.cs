using RuinGamePDT.Generation;
using RuinGamePDT.Resources;
using RuinGamePDT.World;
using Xunit.Abstractions;

namespace RuinGamePDT.Tests;

public class EncounterMapTests(ITestOutputHelper output)
{
    [Fact]
    public void Generate_ProducesCorrectSize()
    {
        var generator = new EncounterMapGenerator();
        EncounterMap map = generator.Generate(BiomeType.Plains, seed: 1);

        Assert.Equal(50, map.Width);
        Assert.Equal(50, map.Height);
    }

    [Theory]
    [InlineData(BiomeType.Plains,    0.10, 0.01)]
    [InlineData(BiomeType.Forest,    0.40, 0.05)]
    [InlineData(BiomeType.Desert,    0.50, 0.05)]
    [InlineData(BiomeType.Swamp,     0.75, 0.15)]
    [InlineData(BiomeType.Mountains, 0.25, 0.25)]
    public void Generate_TileDistributionIsWithinMargin(BiomeType biome, double targetObstacle, double targetHazard)
    {
        const double margin = 0.05;
        var generator = new EncounterMapGenerator();
        EncounterMap map = generator.Generate(biome, seed: 42);

        int obstacle = 0, hazard = 0, total = map.Width * map.Height;
        for (int x = 0; x < map.Width; x++)
            for (int y = 0; y < map.Height; y++)
            {
                if (map.GetTile(x, y) == EncounterTileType.Obstacle) obstacle++;
                else if (map.GetTile(x, y) == EncounterTileType.Hazard) hazard++;
            }

        Assert.InRange(obstacle / (double)total, targetObstacle - margin, targetObstacle + margin);
        Assert.InRange(hazard   / (double)total, targetHazard   - margin, targetHazard   + margin);
    }

    [Fact]
    public void Generate_WritesColoredHtmlMaps()
    {
        var biomes = new[]
        {
            (BiomeType.Plains,    "Plains",    "- Plains",    "#bef264"),
            (BiomeType.Forest,    "Forest",    "T Forest",    "#4ade80"),
            (BiomeType.Desert,    "Desert",    ". Desert",    "#d4a827"),
            (BiomeType.Swamp,     "Swamp",     "~ Swamp",     "#2dd4bf"),
            (BiomeType.Mountains, "Mountains", "^ Mountains", "#9ca3af"),
        };

        var repoRoot   = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../.."));
        var outputPath = Path.Combine(repoRoot, "TestOutput", "encounter_maps.html");

        var html = new System.Text.StringBuilder();
        html.AppendLine("""
            <!DOCTYPE html>
            <html>
            <head>
            <meta charset="utf-8">
            <title>Encounter Maps</title>
            <style>
              body  { background: #111; margin: 0; padding: 24px; font-family: 'Courier New', monospace; }
              h2    { font-size: 15px; margin: 28px 0 6px; }
              pre   { font-size: 11px; line-height: 1.1; letter-spacing: 0.04em; margin: 0; }
              .e    { color: #4ade80; }
              .o    { color: #facc15; }
              .h    { color: #f87171; }
              .legend { font-size: 12px; margin-bottom: 6px; }
              .legend span { margin-right: 16px; }
              .stats  { color: #6b7280; font-size: 11px; margin-top: 6px; }
            </style>
            </head>
            <body>
            <div class="legend">
              <span class="e">E  Empty</span>
              <span class="o">O  Obstacle</span>
              <span class="h">H  Hazard</span>
            </div>
            """);

        var generator = new EncounterMapGenerator();

        foreach (var (biome, name, symbol, titleColor) in biomes)
        {
            EncounterMap map = generator.Generate(biome, seed: 42);

            int obstacle = 0, hazard = 0, empty = 0;
            for (int x = 0; x < map.Width; x++)
                for (int y = 0; y < map.Height; y++)
                    switch (map.GetTile(x, y))
                    {
                        case EncounterTileType.Obstacle: obstacle++; break;
                        case EncounterTileType.Hazard:   hazard++;   break;
                        default:                         empty++;    break;
                    }

            int total = map.Width * map.Height;
            html.AppendLine($"<h2 style=\"color:{titleColor}\">{symbol}</h2>");
            html.AppendLine("<pre>");

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var (cls, ch) = map.GetTile(x, y) switch
                    {
                        EncounterTileType.Obstacle => ("o", "O"),
                        EncounterTileType.Hazard   => ("h", "H"),
                        _                          => ("e", "E")
                    };
                    html.Append($"<span class=\"{cls}\">{ch}</span>");
                }
                html.AppendLine();
            }

            html.AppendLine("</pre>");
            html.AppendLine($"<div class=\"stats\">Empty {empty * 100.0 / total:F1}% &nbsp; Obstacle {obstacle * 100.0 / total:F1}% &nbsp; Hazard {hazard * 100.0 / total:F1}%</div>");
        }

        html.AppendLine("</body></html>");

        File.WriteAllText(outputPath, html.ToString());
        output.WriteLine($"Maps written to: {outputPath}");

        Assert.True(File.Exists(outputPath));
    }
}
