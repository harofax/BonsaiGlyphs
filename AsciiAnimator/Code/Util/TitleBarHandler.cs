using System.Runtime.InteropServices;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;
using Game = SadConsole.Game;
using Point = Microsoft.Xna.Framework.Point;

namespace AsciiAnimator.Code.Util;

public class TitleBarHandler : ControlsConsole
{
    public delegate void WindowChanged(int width, int height);

    public static WindowChanged OnWindowChanged;
    private bool _maximized;
    private int dragDelay = 1;
    private bool dragging;
    private ButtonBox exitButton;
    private ButtonBox maximizeButton;
    private ButtonBox minimizeButton;
    private Point mouseDownPosition;

    public TitleBarHandler(int width, int height) : base(width, height)
    {
        Controls.ThemeColors = ProgramSettings.THEME;
        UseMouse = true;
        UseKeyboard = false;

        DrawTitleBar();

        exitButton = new ButtonBox(3, 3);
        exitButton.Text = "X";
        exitButton.TextAlignment = HorizontalAlignment.Center;
        exitButton.UseExtended = true;

        Controls.Add(exitButton);

        maximizeButton = new ButtonBox(3, 3);
        maximizeButton.Text = "[ ]";
        maximizeButton.TextAlignment = HorizontalAlignment.Left;
        maximizeButton.UseExtended = true;

        minimizeButton = new ButtonBox(3, 3);
        minimizeButton.Text = "_";
        minimizeButton.TextAlignment = HorizontalAlignment.Center;
        minimizeButton.UseExtended = true;


        PlaceButtons();

        exitButton.MouseButtonClicked += ExitClicked;
        maximizeButton.MouseButtonClicked += MaximizeClicked;
        minimizeButton.MouseButtonClicked += MinimizeClicked;

        //Surface.FillWithRandomGarbage(Font);


        FocusedMode = FocusBehavior.Push;
    }

    private void DrawTitleBar()
    {
        var theme = ProgramSettings.THEME;
        
        Surface.Fill(Color.Transparent, theme.ControlBackgroundNormal);
        Surface.DrawBox(Surface.Area, ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThinExtended,
            new ColoredGlyph(theme.Lines, theme.ControlBackgroundNormal)));
        Surface.Print(2, 1, "ASCII ANIMATOR", theme.Lines);
    }

    [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_MinimizeWindow(IntPtr window);

    private void MinimizeClicked(object? sender, ControlBase.ControlMouseState e)
    {
        SDL_MinimizeWindow(Game.Instance.MonoGameInstance.Window.Handle);
    }

    private void MaximizeClicked(object? sender, ControlBase.ControlMouseState e)
    {
        if (!_maximized)
        {
            var width = Game.Instance.MonoGameInstance.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            var height = Game.Instance.MonoGameInstance.GraphicsDevice.Adapter.CurrentDisplayMode.Height;

            Game.Instance.MonoGameInstance.ResizeGraphicsDeviceManager(FontSize.ToMonoPoint(), width / FontSize.X,
                height / FontSize.Y, 0, 0);

            Game.Instance.MonoGameInstance.Window.Position = new Point(0, 0);
            _maximized = true;
        }
        else
        {
            Game.Instance.MonoGameInstance.ResizeGraphicsDeviceManager(FontSize.ToMonoPoint(),
                ProgramSettings.PROGRAM_WIDTH, ProgramSettings.PROGRAM_HEIGHT, 0, 0);
            _maximized = false;
        }

        Game.Instance.MonoGameInstance.ResetRendering();

        var newWidth = Game.Instance.MonoGameInstance.GraphicsDevice.PresentationParameters.BackBufferWidth;
        var newHeight = Game.Instance.MonoGameInstance.GraphicsDevice.PresentationParameters.BackBufferHeight;

        Resize(newWidth / FontSize.X, Height, true);
        DrawTitleBar();

        PlaceButtons();

        OnWindowChanged?.Invoke(newWidth, newHeight);
    }

    private void PlaceButtons()
    {
        exitButton.Position = (Width - exitButton.Width, 0);
        maximizeButton.PlaceRelativeTo(exitButton, Direction.Types.Left, 0);
        minimizeButton.PlaceRelativeTo(maximizeButton, Direction.Types.Left, 0);
    }

    private void ExitClicked(object? sender, ControlBase.ControlMouseState e)
    {
        // autosave? 
        // prompt for save?
        Game.Instance.MonoGameInstance.Exit();
    }

    public override bool ProcessMouse(MouseScreenObjectState state)
    {
        if (state.Mouse.LeftButtonDown && state.Mouse.LeftButtonDownDuration.Milliseconds < dragDelay
                                       && state.IsOnScreenObject && state.ScreenObject == this)
        {
            mouseDownPosition = state.Mouse.ScreenPosition.ToMonoPoint();
            dragging = true;
        }

        if (!state.Mouse.LeftButtonDown) dragging = false;

        if (!dragging) return base.ProcessMouse(state);

        if (state.Mouse.LeftButtonDown && state.Mouse.LeftButtonDownDuration.Milliseconds > dragDelay)
        {
            var windowPos = Game.Instance.MonoGameInstance.Window.Position;

            Game.Instance.MonoGameInstance.Window.Position =
                state.Mouse.ScreenPosition.ToMonoPoint() + windowPos - mouseDownPosition;
        }

        return base.ProcessMouse(state);
    }
}