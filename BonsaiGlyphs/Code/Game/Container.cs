﻿using BonsaiGlyphs.Assets;
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
    private CritterManager critterManager;

    public Container()
    {
        world = new LayeredWorld(GameSettings.VIEW_WIDTH, GameSettings.VIEW_HEIGHT, GameSettings.WORLD_WIDTH,
            GameSettings.WORLD_HEIGHT);
        
        var potLayer = world.GetLayerSurface(LayeredWorld.WorldLayer.Pot);
        var dirtLayer = world.GetLayerSurface(LayeredWorld.WorldLayer.Dirt);

        world.UsePixelPositioning = true;

        critterManager = new CritterManager(world);
        
        var potFile = AssetManager.LoadRexFile(Paths.POT_PATH);

        Point potDimensions = new Point(potFile.Width, potFile.Height);

        Point potStart = world.Surface.Area.Center.WithY(world.Surface.Area.Height - potDimensions.Y).Translate(-potDimensions.X/2, 0);
        
        bonsaiPot = new BonsaiPot(ref potLayer, ref dirtLayer, potFile, potStart);
        
        
        var treeBranchLayer = world.GetLayerSurface(LayeredWorld.WorldLayer.TreeBackground);
        var treeLeafLayer = world.GetLayerSurface(LayeredWorld.WorldLayer.TreeForeground);
        
        bonsaiTree = new BonsaiTree(ref treeLeafLayer, ref treeBranchLayer, potStart.Translate(potDimensions.X/2, 0));
        
        var fg = world.GetLayerSurface(LayeredWorld.WorldLayer.Foreground);
        var middleRect = new Rectangle(world.Surface.Area.Center, 6, 3);
        // ------------------ DRAW SOMETHING HERE -----------------------------------------------------------
        // ------------------------------------- LIKE A CLOUD OR SOMETHING ----------------------------------

        Children.Add(world);
        world.IsFocused = true;
        UseMouse = false;

        //world.Viewport.CameraPosition = world.Surface.View.WithCenter(potStart).Position.Translate(potDimensions.X/2, 0).ToMonoPoint().ToVector2();
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
            randPos = bonsaiTree.DebugGrow();
            //world.IsDirty = true;
            newLeaf = true;
        }

        if (keyboard.IsKeyPressed(Keys.Space))
        {
            System.Console.Out.WriteLine("world.viewpos " + world.ViewPosition);
            System.Console.Out.WriteLine("world.surface.viewpos" + world.Surface.ViewPosition);
            System.Console.Out.WriteLine("world.surface.view.pos" + world.Surface.View.Position);
            //System.Console.Out.WriteLine("viewpath count " + viewPath.Count);
            //System.Console.Out.WriteLine("---- POT ---");
            //System.Console.Out.WriteLine("pos: " );
            //System.Console.Out.WriteLine("visible: " + bonsaiPot.IsVisible);
            //System.Console.Out.WriteLine("------------");
        }

        if (newLeaf)
        {
            var desired = randPos - new Point(world.Surface.ViewWidth / 2, world.Surface.ViewHeight / 2);
            if (Math.Abs(world.Surface.View.Y - desired.Y) > 11)
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

            Point cur = world.Surface.View.Position;
            Point desired = viewPath.Peek();

            Point lerpPoint = new Point(
                (int) lerp(cur.X, desired.X, fraction),
                (int) lerp(cur.Y, desired.Y, fraction)
            );

            var yDiff = (float) world.Surface.View.Y - ((float) desired.Y);

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

            world.ViewPosition = lerpPoint;
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
        var fittingView =world.Surface.View.WithSize(
            SadConsole.Game.Instance.MonoGameInstance.WindowWidth / world.FontSize.X,
            SadConsole.Game.Instance.MonoGameInstance.WindowHeight / world.FontSize.Y);

        world.Surface.View = fittingView;
        world.SetLayersView(fittingView);

        int windowWidth = SadConsole.Game.Instance.MonoGameInstance.WindowWidth;
        int windowHeight = SadConsole.Game.Instance.MonoGameInstance.WindowHeight;


        //System.Console.Out.WriteLine("------- RESIZE ----------");
        //System.Console.Out.WriteLine("window width: " + windowWidth);
        //System.Console.Out.WriteLine("window height: " + windowHeight);
        //System.Console.Out.WriteLine("-------------------------");
        //System.Console.Out.WriteLine("world pixelwidth: " + world.WidthPixels);
        //System.Console.Out.WriteLine("world pixelheight: " + world.HeightPixels);
        //System.Console.Out.WriteLine("-------------------------");

        var centeredPosition = new Point((windowWidth - world.WidthPixels) / 2,
            (windowHeight - world.HeightPixels) / 2);
        //world.Position = newPosition;

        world.Position = centeredPosition;
        
        //world.SetLayerPositions(newPosition);

        //System.Console.Out.WriteLine("post-resize pos: " + world.Position);

        IsFocused = true;
    }

    public void OnResize(object? sender, EventArgs e)
    {
        FitToWindow();
        IsFocused = true;
    }
}