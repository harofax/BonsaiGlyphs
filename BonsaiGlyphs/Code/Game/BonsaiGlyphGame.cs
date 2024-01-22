namespace BonsaiGlyphs.Code.Game;

public class BonsaiGlyphGame : LayeredScreenSurface
{
    private ICellSurface bgLayer;
    public BonsaiGlyphGame(int width, int height, int worldWidth, int worldHeight) : base(width, height, worldWidth, worldHeight)
    {
        bgLayer = new CellSurface(worldWidth, worldHeight);
        Layers.Add(bgLayer);
        bgLayer.Fill(Color.Goldenrod, Color.Transparent, '.');
        System.Console.Out.WriteLine(Layers.Count);
    }
    
    
}