using Microsoft.Xna.Framework;
using SadConsole.Input;
using Color = SadRogue.Primitives.Color;
using Game = SadConsole.Game;
using Point = Microsoft.Xna.Framework.Point;

namespace AsciiAnimator.Code.Util;

public class TitleBarHandler : ScreenSurface
{
    private int dragDelay = 10;
    private Point mouseDownPosition;
    private bool dragging = false;

    public TitleBarHandler(int width, int height) : base(width, height)
    {
        UseMouse = true;
        UseKeyboard = false;

        //Surface.Fill(Color.Transparent, ProgramSettings.THEME.ControlHostBackground);
        //Surface.Print(Width-2, 1, "X", Color.Red);
        //Surface.Print(Width-3, 1, "-", Color.AnsiBlue);
        
        Surface.FillWithRandomGarbage(Font);
        
        
        FocusedMode = FocusBehavior.Set;
    }


    public override bool ProcessMouse(MouseScreenObjectState state)
    {
        if (state.Mouse.LeftButtonDown && state.Mouse.LeftButtonDownDuration.Milliseconds < dragDelay)
        {
            mouseDownPosition = state.Mouse.ScreenPosition.ToMonoPoint();
        }

        if (state.Mouse.LeftButtonDown && state.Mouse.LeftButtonDownDuration.Milliseconds > dragDelay)
        {
            var windowPos = Game.Instance.MonoGameInstance.Window.Position;

            Game.Instance.MonoGameInstance.Window.Position = state.Mouse.ScreenPosition.ToMonoPoint() + windowPos - mouseDownPosition;
        }
        
        
        return base.ProcessMouse(state);
    }
}