using BonsaiGlyphs.Code.Managers;

namespace BonsaiGlyphs.Code.Tree;

using SadConsole.Readers;

public class BonsaiTree
{
    private Rectangle debugRect;

    private static Color branchBG = new Color(51, 24, 15);
    private static Color branchFG = new Color(117, 65, 47);

    private static Color leafBG = new Color(41, 110, 57, 60);
    
    private ColoredGlyph debugLeaf = new ColoredGlyph(Color.LimeGreen, leafBG, '&');
    private ColoredGlyph debugBranch = new ColoredGlyph(branchFG, branchBG, '#');

    private Random rng;
    
    public ICellSurface LeafSurface { get; }
    public ICellSurface BranchSurface { get; }
    

    public BonsaiTree(int width, int height)
    {
        rng = new Random();
        LeafSurface = new CellSurface(width, height);
        BranchSurface = new CellSurface(width, height);

        debugRect = new Rectangle(LeafSurface.Area.Center.WithY(height - 6), 2, 4);
        
        BranchSurface.DrawBox(debugRect, ShapeParameters.CreateFilled(debugBranch, debugBranch));

        LeafSurface.IsDirty = true;
    }

    public Point RandomizeLeaves()
    {
        var randLeaf = new ColoredGlyph(debugLeaf.Foreground, debugLeaf.Background, rng.Next(1, 255));

        remake:
        if (randLeaf.Glyph is 177 or 178 or >= 219 and <= 223 or 8 or 10)
        {
            randLeaf = new ColoredGlyph(debugLeaf.Foreground, debugLeaf.Background, rng.Next(1, 255));
            goto remake;
        }
        
        var randSize = new Point(rng.Next(2, 8),
            rng.Next(2, 4));

        
        var randPos = new Point(rng.Next(debugRect.Center.X - 9, debugRect.Center.X + 9),
            rng.Next(debugRect.Center.Y - 5, debugRect.Center.Y + 3));

        randPos = new Point(
            Math.Clamp(randPos.X, 10, LeafSurface.Width - 10),
            Math.Clamp(randPos.Y, 10 + randSize.Y, LeafSurface.Height - 16));

        var randRect = new Rectangle(randPos, randSize.X, randSize.Y);

        BranchSurface.DrawLine(debugRect.Center, randRect.Center, debugBranch.Glyph, debugBranch.Foreground,
            debugBranch.Background);

        BranchSurface.DrawLine(debugRect.Center.Translate((1, 0)), randRect.Center.Translate((1, 0)), debugBranch.Glyph,
            debugBranch.Foreground, debugBranch.Background);
        
        LeafSurface.DrawCircle(randRect, ShapeParameters.CreateFilled(debugLeaf, randLeaf));

        if (rng.Next(0, 10) > 7)
        {
            LeafSurface.DrawLine(debugRect.Center, randRect.Center, '=', debugBranch.Foreground,
                debugBranch.Background);
        }
        
        debugRect = randRect;

        return randPos;
    }
}