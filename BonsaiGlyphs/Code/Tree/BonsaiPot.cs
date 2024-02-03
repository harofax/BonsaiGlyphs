using BonsaiGlyphs.Assets;
using BonsaiGlyphs.Code.Managers;
using SadConsole.Readers;

namespace BonsaiGlyphs.Code.Tree;

public class BonsaiPot
{
    private const int DIRT_LAYER = 0;
    private const int POT_LAYER = 1;

    public ScreenSurface DirtSurface { get; }
    public ScreenSurface PotSurface { get; }

    //public ICellSurface CombinedSurface { get; }

    private ColoredGlyph debugGlyph = new ColoredGlyph(Color.Red, Color.AnsiYellow);

    public BonsaiPot(ref ScreenSurface potSurface, ref ScreenSurface dirtSurface, REXPaintImage potFile, Point startPos)
    {
        DirtSurface = dirtSurface;
        PotSurface = potSurface;

        ParsePot(potFile, startPos);
    }

    public static BonsaiPot ConstructBonsaiPot(ref ScreenSurface potLayer, ref ScreenSurface dirtLayer, Point startPos)
    {
        var potFile = AssetManager.LoadRexFile(Paths.POT_PATH);

        return new BonsaiPot(
            ref potLayer,
            ref dirtLayer,
            potFile,
            startPos
                .WithX(startPos.X - potFile.Width / 2)
                .WithY(startPos.Y - potFile.Height / 2)
        );
    }

    private void ParsePot(REXPaintImage potImage, Point startPos)
    {
        var potLayers = potImage.ToCellSurface();

        var potCells = potLayers[POT_LAYER];
        var dirtCells = potLayers[DIRT_LAYER];

        for (int x = 0; x < potImage.Width; x++)
        {
            for (int y = 0; y < potImage.Height; y++)
            {
                var potCell = potCells[x, y];
                var dirtCell = dirtCells[x, y];

                PotSurface.SetCellAppearance(startPos.X + x, startPos.Y + y, potCell);
                DirtSurface.SetCellAppearance(startPos.X + x, startPos.Y + y, dirtCell);

                //CombinedSurface.SetCellAppearance(x, y, potCell.Glyph == 0 ? dirtCell : potCell);
            }
        }
    }
}