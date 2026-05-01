namespace RuinGamePDT.Generation;

public class MoistureMapGenerator
{
    private NoiseMapGenerator _noiseGen;

    public MoistureMapGenerator()
    {
        _noiseGen = new NoiseMapGenerator();
    }

    public float[,] GenerateMoistureMap(int width, int height, int seed)
    {
        // Offset seed so moisture noise is independent of heat and elevation
        return _noiseGen.GenerateNoiseMap(width, height, seed + 2000, 0.04f, 4, 0.5f, 2.0f);
    }
}
