using BonsaiGlyphs.Code.Game;
using BonsaiGlyphs.Code.Util;
using Microsoft.Xna.Framework;
using SadConsole.Components;
using SadConsole.Input;
using Point = SadRogue.Primitives.Point;

namespace BonsaiGlyphs.Code.Components;

public class WorldViewPortMovementHandler : IComponent
{
    private LayeredWorld world;
  
    public void Update(IScreenObject host, TimeSpan delta)
    {
        //if (cachedViewPosition != world.ViewPosition)
        //{
        //    world.SetLayersViewPositions(world.ViewPosition);
        //    cachedViewPosition = world.ViewPosition;
        //}
    }

    public void Render(IScreenObject host, TimeSpan delta)
    {
    }

    private Vector2 panOrigin;
    private int panDelay = 10;
    private bool panning;
    private float slowdown = 0.9f;
    private Vector2 panVelocity = Vector2.Zero;

    private Vector2 cameraPos;
    private Vector2 cameraMax;


    public void ProcessMouse(IScreenObject host, MouseScreenObjectState state, out bool handled)
    {
        
        if (state.Mouse.LeftClicked)
        {
           
            if (world.CastRayAt(state.CellPosition, true, out RayHitResult hit))
            {
                System.Console.Out.WriteLine("cell: " + hit.Cell.GlyphCharacter);


                
                
            }
        }
        
        if (state.Mouse.RightButtonDown && state.Mouse.RightButtonDownDuration.Milliseconds < panDelay)
        {
            panOrigin = state.CellPosition.ToMonoPoint().ToVector2();
        }

        if (state.Mouse.RightButtonDownDuration.Milliseconds > panDelay)
        {
            panning = true;
            Vector2 current = state.CellPosition.ToMonoPoint().ToVector2();
            Vector2 diff = panOrigin - current;
            panVelocity = diff;
        }

        
        if (panning && !state.Mouse.RightButtonDown)
        {
            panVelocity *= slowdown;

            if (panVelocity.Length() < 0.1f)
            {
                panVelocity = Vector2.Zero;
                panning = false;
            }

            //if (timer > 1f)
            //{
            //    panVelocity = Point.Zero;
            //}
        }

        CameraPosition += panVelocity;
        
        CameraPosition = Vector2.Clamp(CameraPosition, Vector2.Zero, cameraMax);

        world.ViewPosition = new Point((int)CameraPosition.X, (int)CameraPosition.Y);
        
        
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
                CameraPosition = new Vector2(newPos.X, newPos.Y);

                var cameraMin = new Vector2(0, 0);
                var cameraMax = new Vector2(world.Surface.Area.Width - world.Surface.View.Width,
                    world.Surface.Area.Height - world.Surface.View.Height);
                CameraPosition = Vector2.Clamp(CameraPosition, cameraMin, cameraMax);
                //world.Surface.ViewPosition = newPos;
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

        CameraPosition = new Vector2(world.ViewPosition.X, world.ViewPosition.Y);
        cameraMax = new Vector2(world.Surface.Area.Width - world.Surface.View.Width,
            world.Surface.Area.Height - world.Surface.View.Height);

        //cachedViewPosition = world.ViewPosition;
    }


    public void OnRemoved(IScreenObject host)
    {
        //throw new NotImplementedException();
    }

    public uint SortOrder { get; }
    public bool IsUpdate { get; } = true;
    public bool IsRender { get; }
    public bool IsMouse { get; } = true;
    public bool IsKeyboard { get; } = true;

    public Vector2 CameraPosition
    {
        get => cameraPos;
        set => cameraPos = value;
    }
}