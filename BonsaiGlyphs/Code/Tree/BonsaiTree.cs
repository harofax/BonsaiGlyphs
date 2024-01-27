using BonsaiGlyphs.Code.Managers;

namespace BonsaiGlyphs.Code.Tree;

using SadConsole.Readers;

public class BonsaiTree : ScreenObject
{
    private Rectangle debugRect;

    private static Color branchBG = new Color(51, 24, 15);
    private static Color branchFG = new Color(117, 65, 47);

    private static Color leafBG = new Color(41, 110, 57, 60);
    
    private ColoredGlyph debugLeaf = new ColoredGlyph(Color.LimeGreen, leafBG, '&');
    private ColoredGlyph debugBranch = new ColoredGlyph(branchFG, branchBG, '#');

    private Random rng;
    
    public ScreenSurface LeafSurface { get; }
    public ScreenSurface BranchSurface { get; }

    public BonsaiTree(int width, int height) : base()
    {
        rng = new Random();
        LeafSurface = new ScreenSurface(width, height);
        BranchSurface = new ScreenSurface(width, height);
        
        LeafSurface.Clear();
        BranchSurface.Clear();

        LeafSurface.IsVisible = true;
        BranchSurface.IsVisible = true;

        debugRect = new Rectangle(LeafSurface.Surface.Area.Center.WithY(height - 6), 2, 4);
        
        BranchSurface.DrawBox(debugRect, ShapeParameters.CreateFilled(debugBranch, debugBranch));

        LeafSurface.IsDirty = true;
        BranchSurface.IsDirty = true;
        
        Children.Add(LeafSurface);
        Children.Add(BranchSurface);
    }

    private void LeafSurfaceOnIsDirtyChanged(object? sender, EventArgs e)
    {
        System.Console.Out.WriteLine("DIRTY CHANGED LEAF TREE");
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

        System.Console.Out.WriteLine("LeafSurface.IsDirty = {0}", LeafSurface.IsDirty);
        System.Console.Out.WriteLine("BranchSurface.IsDirty = {0}", BranchSurface.IsDirty);
        
        debugRect = randRect;

        return randPos;
    }
}