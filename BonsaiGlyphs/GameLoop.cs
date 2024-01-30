using BonsaiGlyphs.Code.Debug;
using BonsaiGlyphs.Code.Game;
using SadConsole.Configuration;

namespace BonsaiGlyphs;

internal static class GameLoop
{
    public static void Main(string[] args)
    {
        Settings.WindowTitle = "Bonsai Glyphs";

        Builder gameStartup = new Builder()
            .SetScreenSize(GameSettings.VIEW_WIDTH, GameSettings.VIEW_HEIGHT)
            .ConfigureFonts(true)
            .OnStart(Init);

        Game.Create(gameStartup);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }

    static void Init(object? sender, GameHost e)
    {
        DebugWindow debug = new DebugWindow(60, GameSettings.WORLD_HEIGHT);

        var mainScreen = new Container();

        mainScreen.Children.Add(debug);

        Game.Instance.Screen = mainScreen;
        Game.Instance.Screen.IsFocused = true;

        Game.Instance.DestroyDefaultStartingConsole();
        Settings.ResizeMode = Settings.WindowResizeOptions.Fit;
        //Game.Instance.MonoGameInstance.WindowResized += mainScreen.OnResize;
    }
}