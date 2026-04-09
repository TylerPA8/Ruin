using RuinGamePDT.Core;
using RuinGamePDT.Resources;

namespace RuinGamePDT.Generation;

public class BiomeMapper
{
    public BiomeType DetermineBiome(float heat, float elevation)
    {
        if (elevation > Constants.HIGH_ELEVATION_THRESHOLD)
            return BiomeType.Mountains;

        if (elevation < Constants.LOW_ELEVATION_THRESHOLD && heat > Constants.HIGH_HEAT_THRESHOLD)
            return BiomeType.Desert;

        if (elevation < Constants.LOW_ELEVATION_THRESHOLD && heat < Constants.LOW_HEAT_THRESHOLD)
            return BiomeType.Swamp;

        if (heat > Constants.MEDIUM_HEAT_THRESHOLD)
            return BiomeType.Plains;

        return BiomeType.Forest;
    }

    public BiomeType[,] MapBiomes(float[,] heatMap, float[,] elevationMap)
    {
        int width = heatMap.GetLength(0);
        int height = heatMap.GetLength(1);
        BiomeType[,] biomeMap = new BiomeType[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                biomeMap[x, y] = DetermineBiome(heatMap[x, y], elevationMap[x, y]);

        return biomeMap;
    }
}
