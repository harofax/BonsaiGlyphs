using BonsaiGlyphs.Code.Game;
using SadConsole.Components;
using SadConsole.Input;

namespace BonsaiGlyphs.Code.Components;

public class WorldComponent : IComponent
{
    private LayeredWorld world;

    public void Update(IScreenObject host, TimeSpan delta)
    {
        //throw new NotImplementedException();
    }

    public void Render(IScreenObject host, TimeSpan delta)
    {
        //throw new NotImplementedException();
    }

    public void ProcessMouse(IScreenObject host, MouseScreenObjectState state, out bool handled)
    {
        //throw new NotImplementedException();
        handled = true;
    }

    public void ProcessKeyboard(IScreenObject host, Keyboard keyboard, out bool handled)
    {
        Point newPos = new Point();
        bool changed = false;

        if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
        {
            newPos = world.View.Position.Translate((-1, 0));
            changed = true;
        }

        if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
        {
            newPos = world.View.Position.Translate((1, 0));
            changed = true;
        }

        if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
        {
            newPos = world.View.Position.Translate((0, -1));
            changed = true;
        }

        if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
        {
            newPos = world.View.Position.Translate((0, 1));
            changed = true;
        }


        if (changed)
        {
            if (world.Area.WithSize(world.Area.Width - world.View.Width, world.Area.Height - world.View.Height).Contains(newPos))
            {
                world.SetViewPosition(newPos);
            }
        }

        handled = changed;
    }

    public void OnAdded(IScreenObject host)
    {
        if (host is LayeredWorld worldObject)
        {
            this.world = worldObject;
        }
    }

    public void OnRemoved(IScreenObject host)
    {
        //throw new NotImplementedException();
    }

    public uint SortOrder { get; }
    public bool IsUpdate { get; }
    public bool IsRender { get; }
    public bool IsMouse { get; }
    public bool IsKeyboard { get; }
}