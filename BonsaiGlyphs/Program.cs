using BonsaiGlyphs.Code.Debug;
using BonsaiGlyphs.Code.Game;
using SadConsole.Configuration;


Settings.WindowTitle = "Bonsai Glyphs";
            
Builder gameStartup = new Builder()
        .SetScreenSize(GameSettings.VIEW_WIDTH, GameSettings.VIEW_HEIGHT)
        .SetStartingScreen<RootScreen>()
        .IsStartingScreenFocused(true)
        .ConfigureFonts(true)
        .OnStart(Init)
    ;
            
Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();


static void Init(object? sender, GameHost e)
{
    DebugWindow debug = new DebugWindow(60, GameSettings.WORLD_HEIGHT);
    
    var mainScreen = new RootScreen();

    mainScreen.Children.Add(debug);
    
    Game.Instance.Screen = mainScreen;
    Game.Instance.Screen.IsFocused = true;
            
    Game.Instance.DestroyDefaultStartingConsole();
    Settings.ResizeMode = Settings.WindowResizeOptions.None;
    Game.Instance.MonoGameInstance.WindowResized += mainScreen.OnResize;

}
