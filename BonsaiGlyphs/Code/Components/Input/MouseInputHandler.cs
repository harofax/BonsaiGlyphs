using SadConsole.Components;
using SadConsole.Input;

namespace BonsaiGlyphs.Code.Components.Input;

public class MouseInputHandler : MouseConsoleComponent
{
    public override void OnAdded(IScreenObject host)
    {
        if (host is not ScreenSurface)
        {
            throw new Exception($"{nameof(MouseInputHandler)} can only be added to {nameof(ScreenSurface)}");
        }
        base.OnAdded(host);
    }

    public override void ProcessMouse(IScreenObject host, MouseScreenObjectState mouse, out bool handled)
    {
        var surface = (ScreenSurface) host;

        if (mouse.Mouse.LeftClicked)
        {
            System.Console.Out.WriteLine("-------------- MOUSE -----------------");
            System.Console.Out.WriteLine("cell pos: " + mouse.CellPosition);
            System.Console.Out.WriteLine("world cell pos: " + mouse.WorldCellPosition);
            System.Console.Out.WriteLine("surface cell pos: " + mouse.SurfaceCellPosition);
            System.Console.Out.WriteLine("--------------------------------------");
        }

        handled = true;
    }
}