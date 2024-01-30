using BonsaiGlyphs.Code.Game;
using BonsaiGlyphs.Code.Util;
using SadConsole.Components;
using SadConsole.Input;

namespace BonsaiGlyphs.Code.Components;

public class WorldViewPortMovementHandler : IComponent
{
    private LayeredWorld world;

    public void Update(IScreenObject host, TimeSpan delta)
    {
    }

    public void Render(IScreenObject host, TimeSpan delta)
    {
    }

    public void ProcessMouse(IScreenObject host, MouseScreenObjectState state, out bool handled)
    {
    
        if (state.Mouse.LeftClicked)
        {
            if (world.CastRayAt(state.CellPosition, true, out RayHitResult hit))
            {
                System.Console.Out.WriteLine("cell: " + hit.Cell.GlyphCharacter);
            }
        }
        
        
        handled = false;
    }

    public void ProcessKeyboard(IScreenObject host, Keyboard keyboard, out bool handled)
    {
        Point newPos = new Point();
        bool changed = false;

        if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
        {
            newPos = world.Surface.View.Position.Translate((-1, 0));
            changed = true;
        }

        if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
        {
            newPos = world.Surface.View.Position.Translate((1, 0));
            changed = true;
        }

        if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
        {
            newPos = world.Surface.View.Position.Translate((0, -1));
            changed = true;
        }

        if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
        {
            newPos = world.Surface.View.Position.Translate((0, 1));
            
            changed = true;
        }


        if (changed)
        {
            if (world.Surface.Area.WithSize(world.Surface.Area.Width - world.Surface.View.Width,
                    world.Surface.Area.Height - world.Surface.View.Height).Contains(newPos))
            {
                world.Surface.ViewPosition = newPos;
                //world.SetLayersViewPositions(newPos);
            }
        }

        handled = changed;
    }

    public void OnAdded(IScreenObject host)
    {
        if (host is not LayeredWorld worldObject)
        {
            throw new ArgumentException("WorldComponent must be added to a LayeredWorld");
        }
        
        world = worldObject;
    }


    public void OnRemoved(IScreenObject host)
    {
        //throw new NotImplementedException();
    }

    public uint SortOrder { get; }
    public bool IsUpdate { get; }
    public bool IsRender { get; }
    public bool IsMouse { get; } = true;
    public bool IsKeyboard { get; } = true;
}