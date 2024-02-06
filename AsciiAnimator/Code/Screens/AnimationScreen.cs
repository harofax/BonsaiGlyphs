using AsciiAnimator.Code.Util;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;

namespace AsciiAnimator.Scenes.Code.Screens;

public class AnimationScreen : ControlsConsole
{

    private ScreenSurface drawArea;
    private TitleBarHandler titleBarHandler;
    public AnimationScreen(int width, int height) : base(ProgramSettings.GAME_WIDTH, ProgramSettings.GAME_HEIGHT)
    {
        titleBarHandler = new TitleBarHandler(Width, 2);
        titleBarHandler.Position = (0, 0);
        
        
        Controls.ThemeColors = ProgramSettings.THEME;
        //Surface.Fill(null, Controls.ThemeColors.ControlHostBackground);

        Surface.DrawBox(Surface.Area, ShapeParameters.CreateStyledBoxThin(Controls.ThemeColors.Lines));
        Surface.Print(Width/2, 0, "[ ASCII ANIMATOR ]");
        
        Button button = new Button("TEST")
        {
            Position = (5,5),
        };
        Button button2 = new Button("TEST")
        {
            Position = (16,30),
        };
        
        Controls.Add(button);
        
        Controls.Add(button2);
        
        drawArea = new ScreenSurface(width, height);
        drawArea.Position = drawArea.Surface.Area.WithCenter((Game.Instance.ScreenCellsX / 2, Game.Instance.ScreenCellsY/2)).Position;
        
        
        
        drawArea.FillWithRandomGarbage(drawArea.Font);
        Children.Add(drawArea);
        UseKeyboard = true;
        IsFocused = true;

        Children.Add(titleBarHandler);
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Escape))
        {
            Game.Instance.MonoGameInstance.Exit();
        }

        return base.ProcessKeyboard(keyboard);
    }
}