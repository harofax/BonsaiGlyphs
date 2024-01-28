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
    private ColoredGlyph debugLeafFade = new ColoredGlyph(Color.LimeGreen, Color.Transparent, '&');
    
    private ColoredGlyph debugBranch = new ColoredGlyph(branchFG, branchBG, '#');

    private Random rng;

    public ScreenSurface LeafSurface { get; }
    public ScreenSurface BranchSurface { get; }

    public BonsaiTree(ref ScreenSurface leafSurface, ref ScreenSurface branchSurface, Point startPos)
    {
        rng = new Random();
        LeafSurface = leafSurface;
        BranchSurface = branchSurface;

        LeafSurface.Clear();
        BranchSurface.Clear();

        LeafSurface.IsVisible = true;
        BranchSurface.IsVisible = true;

        debugRect = new Rectangle(startPos, 2, 4);

        BranchSurface.DrawBox(debugRect, ShapeParameters.CreateFilled(debugBranch, debugBranch));

        LeafSurface.IsDirty = true;
        BranchSurface.IsDirty = true;

        debugRect = debugRect.WithCenter(debugRect.Center.WithY(debugRect.Y));
    }

    private void LeafSurfaceOnIsDirtyChanged(object? sender, EventArgs e)
    {
        System.Console.Out.WriteLine("DIRTY CHANGED LEAF TREE");
    }

    private int pityX = 0;
    private int pityY = 0;

    public Point RandomizeLeaves()
    {
        var randLeaf = new ColoredGlyph(debugLeaf.Foreground, debugLeaf.Background, rng.Next(1, 255));

        remake:
        if (randLeaf.Glyph is 177 or 178 or >= 219 and <= 223 or 8 or 10)
        {
            randLeaf = new ColoredGlyph(debugLeaf.Foreground, debugLeaf.Background, rng.Next(1, 255));
            goto remake;
        }

        var randSize = new Point(rng.Next(4, 10),
            rng.Next(2, 5));

        var nextX = rng.Next(debugRect.Center.X - 9 + pityX, debugRect.Center.X + 9 + pityX);

        if (nextX > GameSettings.WORLD_WIDTH / 2)
        {
            pityX -= 1;
        }
        else
        {
            pityX += 1;
        }

        pityX = Math.Clamp(pityX, -3, 3);

        var nextY = rng.Next(debugRect.Center.Y - 6 + pityY, debugRect.Center.Y + 5 + pityY);

        var yDiff = debugRect.Center.Y - nextY;

        if (yDiff > 0)
        {
            pityY += 1;
        }
        else
        {
            pityY -= 1;
        }

        pityY = Math.Clamp(pityY, -4, 4);

        var randPos = new Point(nextX, nextY);

        randPos = new Point(
            Math.Clamp(randPos.X, 10, LeafSurface.Width - 10),
            Math.Clamp(randPos.Y, 10 + randSize.Y, LeafSurface.Height - 16));

        var randRect = new Rectangle(randPos, randSize.X, randSize.Y);


        
        BranchSurface.DrawLine(debugRect.Center, randRect.Center, debugBranch.Glyph,
            debugBranch.Foreground, debugBranch.Background);
        
        var randBranchWidth = rng.Next(1, 5);

        for (int i = 1; i < randBranchWidth; i++)
        {
            BranchSurface.DrawLine(debugRect.Center.Translate(i,0), randRect.Center.Translate(i,0), debugBranch.Glyph, debugBranch.Foreground,
                debugBranch.Background);
        }


        

        LeafSurface.DrawCircle(randRect, ShapeParameters.CreateFilled(debugLeaf, randLeaf));

        
        
        if (rng.Next(0, 10) > 7)
        {
            LeafSurface.DrawLine(debugRect.Center, randRect.Center, '=', debugBranch.Foreground,
                debugBranch.Background);
        }

        LeafSurface.IsDirty = true;

        //System.Console.Out.WriteLine("LeafSurface.IsDirty = {0}", LeafSurface.IsDirty);
        //System.Console.Out.WriteLine("BranchSurface.IsDirty = {0}", BranchSurface.IsDirty);

        debugRect = randRect;

        return randPos;
    }
}