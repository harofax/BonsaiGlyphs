namespace BonsaiGlyphs.Code.Tree;

public class BonsaiTree
{
    private Rectangle potRect;
    private ColoredGlyph potFillGlyph = new ColoredGlyph(Color.OrangeRed, Color.Brown, '#');
    private ColoredGlyph potBorderGlyph = new ColoredGlyph(Color.Brown, Color.OrangeRed, 'o');

    public ICellSurface Cells { get; }

    public BonsaiTree(int width, int height)
    {
        Cells = new CellSurface(width, height);
        potRect = new Rectangle(Cells.Area.Center, 10, 2);

      
        Cells.DrawLine(Cells.Area.MinExtent, Cells.Area.MaxExtent, 'O', potFillGlyph.Foreground,
            potFillGlyph.Background);
        
        Cells.DrawCircle(potRect, ShapeParameters.CreateFilled(potBorderGlyph, potFillGlyph));
    }

  
}