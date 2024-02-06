using AsciiAnimator.Scenes.Code.Screens;
using SadConsole.Configuration;

namespace AsciiAnimator;

internal static class Program
{
    public static void Main(string[] args)
    {
        Settings.WindowTitle = "ASCII ANIMATOR MK.1000";

        Builder gameStartup = new Builder()
                .SetScreenSize(ProgramSettings.GAME_WIDTH, ProgramSettings.GAME_HEIGHT)
                .ConfigureFonts(true)
                .OnStart(Init)
            ;

        Game.Create(gameStartup);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }

    private static void Init(object? sender, GameHost e)
    {
        var startScreen = new AnimationScreen(32, 16);

        Game.Instance.Screen = startScreen;
        Game.Instance.Screen.IsFocused = true;
        Game.Instance.DestroyDefaultStartingConsole();

        Game.Instance.MonoGameInstance.Window.IsBorderless = true;
        Settings.ResizeMode = Settings.WindowResizeOptions.Fit;
    }
}