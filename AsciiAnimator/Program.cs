using AsciiAnimator.Code.Util;
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
        MainMenu menu = new MainMenu(ProgramSettings.GAME_WIDTH, ProgramSettings.GAME_HEIGHT);

        Game.Instance.Screen = menu;
        Game.Instance.Screen.IsFocused = true;
        
        SadConsole.Serializer.Settings.Converters.Add(new PackedColorConverter());
        
        Game.Instance.DestroyDefaultStartingConsole();

        Settings.ResizeMode = Settings.WindowResizeOptions.Fit;
    }
}