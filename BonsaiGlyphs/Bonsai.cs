using BonsaiGlyphs.Code.Debug;
using BonsaiGlyphs.Code.Game;
using SadConsole.Configuration;

namespace BonsaiGlyphs
{
    public static class Bonsai
    {

        public static DebugWindow Debug;
        
        static void Main(string[] args)
        {
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
        }

        private static void Init(object? sender, GameHost e)
        {
            Debug = new DebugWindow(60, GameSettings.GAME_HEIGHT);


            var mainScreen = new RootScreen();

            Game.Instance.Screen = mainScreen;
            Game.Instance.Screen.IsFocused = true;
            
            Game.Instance.DestroyDefaultStartingConsole();
            Settings.ResizeMode = Settings.WindowResizeOptions.None;
            Game.Instance.MonoGameInstance.WindowResized += mainScreen.OnResize;

        }
    }
    
}