using SadConsole.UI;
using SadConsole.UI.Controls;

namespace AsciiAnimator.Scenes;

class RootScreen : ScreenObject
{
    private ScreenSurface _mainSurface;

    public RootScreen()
    {
        // Create a surface that's the same size as the screen.
        _mainSurface = new ScreenSurface(ProgramSettings.GAME_WIDTH, ProgramSettings.GAME_HEIGHT);

        _mainSurface.Fill(new Color(0.1f, 0, 0.2f));
        
        
        
        SadConsole.UI.ControlHost host = new ControlHost();

        SadConsole.UI.Controls.TextBox textBox = new TextBox(30);
        
        host.Add(textBox);

        textBox.Position = (5, 5);

        // Print some text at (4, 4) using the foreground and background already there (violet and black)
        _mainSurface.Print(4, 4, "Hello from SadConsole");

        _mainSurface.SadComponents.Add(host);
        
        
        // Add _mainSurface as a child object of this one. This object, RootScreen, is a simple object
        // and doesn't display anything itself. Since _mainSurface is going to be a child of it, _mainSurface
        // will be displayed.
        Children.Add(_mainSurface);
        
    }
}