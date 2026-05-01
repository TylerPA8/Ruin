using RuinGamePDT.Core;
using RuinGamePDT.Resources;

namespace RuinGamePDT.Generation;

public class BiomeMapper
{
    public BiomeType DetermineBiome(float heat, float elevation, float moisture)
    {
        if (elevation > Constants.HIGH_ELEVATION_THRESHOLD)
            return BiomeType.Mountains;

        if (heat > Constants.HIGH_HEAT_THRESHOLD && moisture < Constants.LOW_MOISTURE_THRESHOLD)
            return BiomeType.Desert;

        if (moisture > Constants.HIGH_MOISTURE_THRESHOLD && heat < Constants.LOW_HEAT_THRESHOLD)
            return BiomeType.Swamp;

        if (moisture > Constants.HIGH_MOISTURE_THRESHOLD)
            return BiomeType.Forest;

        if (moisture > Constants.MEDIUM_MOISTURE_THRESHOLD && heat < Constants.MEDIUM_HEAT_THRESHOLD)
            return BiomeType.Forest;

        return BiomeType.Plains;
    }

    public BiomeType[,] MapBiomes(float[,] heatMap, float[,] elevationMap, float[,] moistureMap)
    {
        int width = heatMap.GetLength(0);
        int height = heatMap.GetLength(1);
        BiomeType[,] biomeMap = new BiomeType[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                biomeMap[x, y] = DetermineBiome(heatMap[x, y], elevationMap[x, y], moistureMap[x, y]);

        return biomeMap;
    }
}
