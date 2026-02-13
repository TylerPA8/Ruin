using RuinGamePDT.Core;
using RuinGamePDT.World;

namespace RuinGamePDT.Generation;

public class BiomeMapper
{
    public BiomeType DetermineBiome(float heat, float elevation)
    {
        // High elevation -> Mountains (regardless of heat)
        if (elevation > Constants.HIGH_ELEVATION_THRESHOLD)
            return BiomeType.Mountains;

        // Low elevation + high heat -> Desert
        if (elevation < Constants.LOW_ELEVATION_THRESHOLD && heat > Constants.HIGH_HEAT_THRESHOLD)
            return BiomeType.Desert;

        // Low elevation + low heat -> Swamp
        if (elevation < Constants.LOW_ELEVATION_THRESHOLD && heat < Constants.LOW_HEAT_THRESHOLD)
            return BiomeType.Swamp;

        // Medium elevation + high heat -> Plains
        if (heat > Constants.MEDIUM_HEAT_THRESHOLD)
            return BiomeType.Plains;

        // Medium elevation + low heat -> Forest
        return BiomeType.Forest;
    }

    public BiomeType[,] MapBiomes(float[,] heatMap, float[,] elevationMap)
    {
        int width = heatMap.GetLength(0);
        int height = heatMap.GetLength(1);

        BiomeType[,] biomeMap = new BiomeType[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                biomeMap[x, y] = DetermineBiome(heatMap[x, y], elevationMap[x, y]);
            }
        }

        return biomeMap;
    }
}
