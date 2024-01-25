using SadConsole.Components;
using SadConsole.Input;

namespace BonsaiGlyphs.Code.Components.Input;

public class MoveSurfaceViewPortKeyboardHandler : KeyboardConsoleComponent
{
    private int originalWidth;
    private int originalHeight;
    private int speed = 1;

    public override void OnAdded(IScreenObject host)
    {
        if (host is ScreenSurface surf)
        {
            originalWidth = surf.Width;
            originalHeight = surf.Height;
        }
        else
        {
            throw new Exception($"{nameof(MoveSurfaceViewPortKeyboardHandler)} can only be added to {nameof(ScreenSurface)}");
        }
    }

    public override void ProcessKeyboard(IScreenObject host, Keyboard keyboard, out bool handled)
    {
        if (keyboard.KeysPressed.Count == 0) handled = true;
        
        
        var surface = (ScreenSurface) host;
        
        if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
        {
            surface.ViewPosition = surface.ViewPosition.Translate((-1 * speed, 0));
        }

        if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
        {
            surface.ViewPosition = surface.ViewPosition.Translate((1* speed, 0));
        }

        if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
        {
            surface.ViewPosition = surface.ViewPosition.Translate((0, -1* speed));
        }

        if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
        {
            surface.ViewPosition = surface.ViewPosition.Translate((0, 1* speed));
        }

        handled = true;
    }
}