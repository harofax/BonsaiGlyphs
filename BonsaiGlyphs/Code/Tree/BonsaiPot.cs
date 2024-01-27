using BonsaiGlyphs.Code.Managers;
using SadConsole.Readers;

namespace BonsaiGlyphs.Code.Tree;

public class BonsaiPot : ScreenSurface
{
    private const int DIRT_LAYER = 0;
    private const int POT_LAYER = 1;

    public ICellSurface DirtSurface { get; }
    public ICellSurface PotSurface { get; }

    private ColoredGlyph debugGlyph = new ColoredGlyph(Color.Red, Color.AnsiYellow);

    private BonsaiPot(int width, int height, REXPaintImage potFile, Point startPos) : base(width, height)
    {
        ParsePot(potFile);
        DirtSurface = new CellSurface(width, height);
        PotSurface = Surface;
        
        PotSurface.DrawCircle(new Rectangle(startPos, 3, 3), ShapeParameters.CreateFilled(debugGlyph, debugGlyph));

        //System.Console.Out.WriteLine("pot created (?) at " + startPos);

        PotSurface.IsDirty = true;
        PotSurface.IsDirty = true;

        IsDirty = true;

        IsVisible = true;

        Position = startPos;
    }
    
    

    public static BonsaiPot ConstructBonsaiPot(Point startPost)
    {
        var potFile = AssetManager.LoadRexFile(Assets.Paths.REX_POT);
        
        return new BonsaiPot(potFile.Width, potFile.Height, potFile, startPost);
    }

    private void ParsePot(REXPaintImage potImage)
    {
        var potLayers = potImage.ToCellSurface();
        
        var potCells = potLayers[POT_LAYER];
        var dirtCells = potLayers[DIRT_LAYER];
        
        for (int x = 0; x < potImage.Width; x++)
        {
            for (int y = 0; y < potImage.Height; y++)
            {
                Surface.SetCellAppearance(x, y, potCells[x,y]);
                dirtCells.SetCellAppearance(x,y, dirtCells[x,y]);
            }
        }
    }

    public override void Render(TimeSpan delta)
    {
        base.Render(delta);
        
    }
}