﻿using BonsaiGlyphs.Code.Components.Input;
using BonsaiGlyphs.Code.Tree;
using SadConsole.Entities;
using SadConsole.Input;

namespace BonsaiGlyphs.Code.Game;

internal class Container : ScreenObject
{
    private BonsaiTree bonsaiTree;
    private BonsaiPot bonsaiPot;
    
    private LayeredScreenSurface world;

    public Container()
    {
        world = new LayeredScreenSurface(GameSettings.VIEW_WIDTH, GameSettings.VIEW_HEIGHT, GameSettings.WORLD_WIDTH,
            GameSettings.WORLD_HEIGHT);
        
        InitializeSky();

        bonsaiTree = new BonsaiTree(20, 20);
        
        //world.Children.Add(bonsaiTree);
        
        bonsaiPot = BonsaiPot.ConstructBonsaiPot(world.Surface.Area.Center);

        //world.Layers.Add(bonsaiTree.BranchSurface);
        //world.Layers.Add(bonsaiTree.LeafSurface);

        world.SadComponents.Add(new MoveSurfaceViewPortKeyboardHandler());
        world.SadComponents.Add(new MouseInputHandler());

        world.IsVisible = true;


        bonsaiTree.Position = (5, 5);
        
        Children.Add(world);
        Children.Add(bonsaiPot);
        Children.Add(bonsaiTree);
        
        Children.MoveToTop(bonsaiPot);
        Children.MoveToTop(bonsaiTree);


        System.Console.Out.WriteLine("bonsaiTree.IsVisible: " + bonsaiTree.IsVisible);
    }

    private void InitializeSky()
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
        Algorithms.GradientFill(world.FontSize,
            world.Surface.Area.Center,
            world.Surface.Height,
            0,
            world.Surface.Area,
            new Gradient(colors, colorStops),
            (x, y, color) => world.Surface[x, y].Background = color);
    }


    private Queue<Point> viewPath = new Queue<Point>();

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        world.ProcessKeyboard(keyboard);

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
            System.Console.Out.WriteLine("---- POT ---");
            System.Console.Out.WriteLine("pos: " + bonsaiPot.Position);
            System.Console.Out.WriteLine("visible: " + bonsaiPot.IsVisible);
            System.Console.Out.WriteLine("------------");
        }

        if (newLeaf)
        {
            var desired = randPos - new Point(world.ViewWidth / 2, world.ViewHeight / 2);
            if (Math.Abs(world.ViewPosition.Y - desired.Y) > 11)
            {
                viewPath.Enqueue(desired);
            }
        }


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

            Point cur = world.ViewPosition;
            Point desired = viewPath.Peek();

            Point lerpPoint = new Point(
                (int) lerp(cur.X, desired.X, fraction),
                (int) lerp(cur.Y, desired.Y, fraction)
            );

            var yDiff = (float) world.ViewPosition.Y - ((float) desired.Y);

            //System.Console.Out.WriteLine("view pos: " + world.ViewPosition);
            //System.Console.Out.WriteLine("desir pos: " + desired);
            //
            //System.Console.Out.WriteLine("Y diff: " + yDiff);

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
        world.Surface.View = world.Surface.View.WithSize(
            SadConsole.Game.Instance.MonoGameInstance.WindowWidth / world.FontSize.X,
            SadConsole.Game.Instance.MonoGameInstance.WindowHeight / world.FontSize.Y);

        int windowWidth = SadConsole.Game.Instance.MonoGameInstance.WindowWidth;
        int windowHeight = SadConsole.Game.Instance.MonoGameInstance.WindowHeight;


        //System.Console.Out.WriteLine("------- RESIZE ----------");
        //System.Console.Out.WriteLine("window width: " + windowWidth);
        //System.Console.Out.WriteLine("window height: " + windowHeight);
        //System.Console.Out.WriteLine("-------------------------");
        //System.Console.Out.WriteLine("world pixelwidth: " + world.WidthPixels);
        //System.Console.Out.WriteLine("world pixelheight: " + world.HeightPixels);
        //System.Console.Out.WriteLine("-------------------------");

        world.Position = new Point((windowWidth - world.WidthPixels) / 2, (windowHeight - world.HeightPixels) / 2);

        //System.Console.Out.WriteLine("post-resize pos: " + world.Position);

        IsFocused = true;
    }

    public void OnResize(object? sender, EventArgs e)
    {
        FitToWindow();
        IsFocused = true;
    }
}