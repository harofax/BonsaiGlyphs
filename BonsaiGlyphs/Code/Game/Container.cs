using BonsaiGlyphs.Assets;
using BonsaiGlyphs.Code.Components;
using BonsaiGlyphs.Code.Components.Input;
using BonsaiGlyphs.Code.Managers;
using BonsaiGlyphs.Code.Tree;
using SadConsole.Entities;
using SadConsole.Input;

namespace BonsaiGlyphs.Code.Game;

internal class Container : ScreenObject
{
    private BonsaiTree bonsaiTree;
    private BonsaiPot bonsaiPot;

    private LayeredWorld world;

    public Container()
    {
        world = new LayeredWorld(GameSettings.VIEW_WIDTH, GameSettings.VIEW_HEIGHT, GameSettings.WORLD_WIDTH,
            GameSettings.WORLD_HEIGHT);

        AddSky(world.GetLayerSurface(LayeredWorld.WorldLayer.Background));
        
        var potLayer = world.GetLayerSurface(LayeredWorld.WorldLayer.Pot);
        var dirtLayer = world.GetLayerSurface(LayeredWorld.WorldLayer.Dirt);
        
        var potFile = AssetManager.LoadRexFile(Paths.REX_POT);

        Point potDimensions = new Point(potFile.Width, potFile.Height);

        Point potStart = world.Area.Center.WithY(world.Area.Height - potDimensions.Y).Translate(-potDimensions.X/2, 0);
        
        bonsaiPot = new BonsaiPot(ref potLayer, ref dirtLayer, potFile, potStart);
        
        
        var treeBranchLayer = world.GetLayerSurface(LayeredWorld.WorldLayer.TreeBackground);
        var treeLeafLayer = world.GetLayerSurface(LayeredWorld.WorldLayer.TreeForeground);

        bonsaiTree = new BonsaiTree(ref treeLeafLayer, ref treeBranchLayer, potStart.Translate(potDimensions.X/2, 0));
        
        var fg = world.GetLayerSurface(LayeredWorld.WorldLayer.Foreground);
        var middleRect = new Rectangle(world.Area.Center, 6, 3);
        fg.DrawCircle(middleRect, ShapeParameters.CreateBorder(new ColoredGlyph(Color.Red, Color.Blue, 'X')));

        Children.Add(world);
    }

    private void AddSky(ScreenSurface screen)
    {
        Color[] colors = new[]
        {
            Color.Black,
            Color.DarkBlue,
            Color.CornflowerBlue,
            Color.Purple,
            Color.OrangeRed,
            Color.Goldenrod,
        };
        float[] colorStops = new[] {0f, 0.3f, 0.5f, 0.7f, 0.85f, 1f};
        Algorithms.GradientFill(screen.FontSize,
            screen.Surface.Area.Center,
            screen.Height,
            0,
            screen.Surface.Area,
            new Gradient(colors, colorStops),
            (x, y, color) => screen.Surface[x, y].Background = color);
    }


    private Queue<Point> viewPath = new Queue<Point>();

    public void StopFollow()
    {
        viewPath.Clear();
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        


        bool newLeaf = false;
        Point randPos = default;

        if (keyboard.IsKeyPressed(Keys.F))
        {
            randPos = bonsaiTree.RandomizeLeaves();
            //world.IsDirty = true;
            newLeaf = true;
        }

        if (keyboard.IsKeyPressed(Keys.Space))
        {
            System.Console.Out.WriteLine("viewpath count " + viewPath.Count);
            //System.Console.Out.WriteLine("---- POT ---");
            //System.Console.Out.WriteLine("pos: " );
            //System.Console.Out.WriteLine("visible: " + bonsaiPot.IsVisible);
            //System.Console.Out.WriteLine("------------");
        }

        if (newLeaf)
        {
            var desired = randPos - new Point(world.Surface.ViewWidth / 2, world.Surface.ViewHeight / 2);
            if (Math.Abs(world.View.Y - desired.Y) > 11)
            {
                if (viewPath.Count > 5)
                {
                    viewPath.Dequeue();
                }
                viewPath.Enqueue(desired);
            }
        }

        world.ProcessKeyboard(keyboard);


        return true;
    }

    private float fraction = 0;

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);

        // TODO: LOOK INTO SadConsole.Instructions.*
        // replace with somethin better this is awful
        if (viewPath.Count < 1) return;

        if (fraction < 1)
        {
            fraction += (float) delta.TotalSeconds * 0.4f;

            Point cur = world.View.Position;
            Point desired = viewPath.Peek();

            Point lerpPoint = new Point(
                (int) lerp(cur.X, desired.X, fraction),
                (int) lerp(cur.Y, desired.Y, fraction)
            );

            var yDiff = (float) world.View.Y - ((float) desired.Y);

            //System.Console.Out.WriteLine("view pos: " + world.ViewPosition);
            //System.Console.Out.WriteLine("desir pos: " + desired);
            //
            //System.Console.Out.WriteLine("Y diff: " + yDiff);

            //if (world.Area.WithSize(world.Area.Width - world.View.Width, world.Area.Height - world.View.Height).Contains(lerpPoint))
            //{
            //    world.SetViewPosition(lerpPoint);
            //}
            
            if (Math.Abs(yDiff) < 2)
            {
                viewPath.Dequeue();
                fraction = 0;
            }
            
            world.SetViewPosition(lerpPoint);
        }
        else
        {
        }
    }

    float lerp(float v0, float v1, float delta)
    {
        var t = delta;
        return (1 - t) * v0 + t * v1;
    }


    public void FitToWindow()
    {
        world.Surface.Surface.View = world.Surface.Surface.View.WithSize(
            SadConsole.Game.Instance.MonoGameInstance.WindowWidth / world.Surface.FontSize.X,
            SadConsole.Game.Instance.MonoGameInstance.WindowHeight / world.Surface.FontSize.Y);

        int windowWidth = SadConsole.Game.Instance.MonoGameInstance.WindowWidth;
        int windowHeight = SadConsole.Game.Instance.MonoGameInstance.WindowHeight;


        //System.Console.Out.WriteLine("------- RESIZE ----------");
        //System.Console.Out.WriteLine("window width: " + windowWidth);
        //System.Console.Out.WriteLine("window height: " + windowHeight);
        //System.Console.Out.WriteLine("-------------------------");
        //System.Console.Out.WriteLine("world pixelwidth: " + world.WidthPixels);
        //System.Console.Out.WriteLine("world pixelheight: " + world.HeightPixels);
        //System.Console.Out.WriteLine("-------------------------");

        world.Position = new Point((windowWidth - world.Surface.WidthPixels) / 2,
            (windowHeight - world.Surface.HeightPixels) / 2);

        //System.Console.Out.WriteLine("post-resize pos: " + world.Position);

        IsFocused = true;
    }

    public void OnResize(object? sender, EventArgs e)
    {
        FitToWindow();
        IsFocused = true;
    }
}