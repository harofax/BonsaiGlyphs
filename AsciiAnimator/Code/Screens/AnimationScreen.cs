using AsciiAnimator.Code.Tools;
using AsciiAnimator.Code.Util;
using SadConsole.Input;
using SadConsole.SplashScreens;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadConsole.UI.Windows;

namespace AsciiAnimator.Scenes.Code.Screens;

public class AnimationScreen : ControlsConsole
{
    private List<ICellSurface> frames = new List<ICellSurface>(3);

    //private ScreenSurface drawArea;
    private TitleBarHandler titleBarHandler;

    private ColorPickerPopup colorPickerPopup = new ColorPickerPopup();
    private int currentFrameIndex = 0;

    private SurfaceViewer viewer;
    private ToolBar toolBar;

    public AnimationScreen(int width, int height) : base(ProgramSettings.PROGRAM_WIDTH, ProgramSettings.PROGRAM_HEIGHT)
    {
        for (int i = 0; i < 3; i++)
        {
            var cellSurface = new CellSurface(width, height);
            cellSurface.FillWithRandomGarbage(Font);
            frames.Add(cellSurface);
        }


        titleBarHandler = new TitleBarHandler(Width, ProgramSettings.TITLEBAR_HEIGHT);
        titleBarHandler.Position = (0, 0);

        Controls.ThemeColors = ProgramSettings.THEME;

        CharacterPicker characterPicker = new CharacterPicker(Color.Yellow, ProgramSettings.THEME.Black, Color.Blue,
            Game.Instance.EmbeddedFont, 16, 16);

        Controls.Add(characterPicker);

        characterPicker.Position = (1, titleBarHandler.Height + 1);

        characterPicker.MouseButtonClicked += CharacterPickerOnMouseButtonClicked;

        DrawAnimationScreen();

        var colorGradient = new ColorPicker(16, 8, colorPickerPopup.SelectedColor);
        colorGradient.PlaceRelativeTo(characterPicker, Direction.Types.Down);
        //colorGradient.Position = (1, colorPickerButton.Position.Y + colorPickerButton.Height);

        colorPickerPopup.Closed += (sender, args) =>
        {
            if (colorPickerPopup.DialogResult)
            {
                colorGradient.MasterColor = colorPickerPopup.SelectedColor;
            }
        };

        //Controls.Add(colorGradient);

        Button colorPickerButton = new Button("COLOUR");
        colorPickerButton.PlaceRelativeTo(colorGradient, Direction.Types.DownLeft, 1);

        colorPickerButton.Position = characterPicker.Surface.Area.Center.WithY(colorPickerButton.Position.Y)
            .Translate(-colorPickerButton.Text.Length, 0);

        colorPickerButton.MouseButtonClicked += (sender, state) => colorPickerPopup.Show(true);

        colorGradient.SelectedColorChanged +=
            (sender, args) => colorPickerPopup.SelectedColor = colorGradient.SelectedColor;

        //Controls.Add(colorPickerButton);

        //DEBUG_DRAW_UI();

        //drawArea = new ScreenSurface(width, height);
        //drawArea.Position = drawArea.Surface.Area.WithCenter((Game.Instance.ScreenCellsX / 2, Game.Instance.ScreenCellsY/2)).Position;

        //frames.Add(drawArea.Surface);
        System.Console.Out.WriteLine("num frames " + frames.Count);

        viewer = new SurfaceViewer(ProgramSettings.CANVAS_WIDTH, ProgramSettings.CANVAS_HEIGHT, frames[0].Surface);
        viewer.Position = viewer.Surface.Area.WithCenter(Surface.Area.Center).Position;
        viewer.MouseMove += CanvasMouseMove;


        Controls.Add(viewer);

        TitleBarHandler.OnWindowChanged += OnWindowChanged;

        toolBar = new ToolBar(ProgramSettings.TOOLBAR_WIDTH, ProgramSettings.TOOLBAR_HEIGHT);
        toolBar.Position = (Width - toolBar.Width, titleBarHandler.Height + 1);

        Children.Add(toolBar);

        //Children.Add(drawArea);
        UseKeyboard = true;
        UseMouse = true;
        IsFocused = true;

        Children.Add(titleBarHandler);
    }

    private void CharacterPickerOnMouseButtonClicked(object? sender, ControlBase.ControlMouseState e)
    {
        var picker = (CharacterPicker) sender;

        if (picker.IsMouseButtonStateClean && picker.MouseArea.Contains(e.MousePosition))
        {
            if (e.OriginalMouseState.Mouse.LeftClicked)
            {
                toolBar.ColorPicker.BaseGlyph = picker
                    .Surface[e.MousePosition.Add(picker.Surface.ViewPosition).ToIndex(picker.Surface.Width)].Glyph;

                toolBar.ColorPicker.IsDirty = true;
            }

            if (e.OriginalMouseState.Mouse.RightClicked)
            {
                var dec = toolBar.ColorPicker.Decorators[0];
                var pickerglyph = picker
                    .Surface[e.MousePosition.Add(picker.Surface.ViewPosition).ToIndex(picker.Surface.Width)].Glyph;

                toolBar.ColorPicker.Decorators[0] = new CellDecorator(dec.Color, pickerglyph, dec.Mirror);
                toolBar.ColorPicker.IsDirty = true;
            }
        }
    }

    private void DEBUG_DRAW_UI()
    {
        ScreenSurface toolbar = new ScreenSurface(ProgramSettings.TOOLBAR_WIDTH, ProgramSettings.TOOLBAR_HEIGHT);
        toolbar.Fill(null, Color.Red);
        toolbar.Position = (Width - toolbar.Width, titleBarHandler.Height);

        ScreenSurface ascii_color_picker =
            new ScreenSurface(ProgramSettings.ASCII_COLOR_PICKER_WIDTH, Height - titleBarHandler.Height);
        ascii_color_picker.Fill(null, Color.Yellow);
        ascii_color_picker.Position = (0, titleBarHandler.Height);

        ScreenSurface timeline = new ScreenSurface(ProgramSettings.TIMELINE_WIDTH, ProgramSettings.TIMELINE_HEIGHT);
        timeline.Fill(null, Color.Green);
        timeline.Position = (ascii_color_picker.Width, Height - timeline.Height);

        Children.Add(toolbar);
        Children.Add(ascii_color_picker);
        Children.Add(timeline);
    }

    private void CanvasMouseMove(object? sender, ControlBase.ControlMouseState e)
    {
        if (e.OriginalMouseState.Mouse.LeftClicked)
        {
            var viewer = (SurfaceViewer) sender;
            if (viewer.IsMouseButtonStateClean && viewer.MouseArea.Contains(e.MousePosition))
            {
                System.Console.Out.WriteLine(viewer.Surface.GetBackground(
                    e.MousePosition.X + viewer.Surface.ViewPosition.X,
                    e.MousePosition.Y + viewer.Surface.ViewPosition.Y));
            }
        }
    }

    private void NextFrame()
    {
        currentFrameIndex++;

        if (currentFrameIndex >= frames.Count)
        {
            currentFrameIndex = 0;
        }

        System.Console.Out.WriteLine("current frame index: " + currentFrameIndex);

        viewer.Surface = frames[currentFrameIndex];
        viewer.IsDirty = true;
    }

    private void PreviousFrame()
    {
        currentFrameIndex--;

        if (currentFrameIndex < 0)
        {
            currentFrameIndex = frames.Count - 1;
        }

        System.Console.Out.WriteLine("current frame index: " + currentFrameIndex);

        viewer.Surface = frames[currentFrameIndex];
        viewer.IsDirty = true;
    }


    private void OnWindowChanged(int width, int height)
    {
        Resize(width / FontSize.X, height / FontSize.Y, true);
        DrawAnimationScreen();
        Surface.Print(Width / 2, 0, "[ ASCII ANIMATOR ]");
        viewer.Position = viewer.Surface.Area
            .WithCenter((Game.Instance.ScreenCellsX / 2, Game.Instance.ScreenCellsY / 2)).Position;
    }

    private void DrawAnimationScreen()
    {
        Surface.Fill(Controls.ThemeColors.ControlHostForeground, Controls.ThemeColors.ControlHostBackground);

        Surface.DrawBox(
            Surface.Area.WithHeight(Height - titleBarHandler.Height + 1).WithY(titleBarHandler.Height - 1),
            ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThinExtended,
                new ColoredGlyph(Controls.ThemeColors.Lines, Controls.ThemeColors.ControlHostBackground)));
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Escape))
        {
            Game.Instance.MonoGameInstance.Exit();
        }

        if (keyboard.IsKeyPressed(Keys.Left))
        {
            PreviousFrame();
        }

        if (keyboard.IsKeyPressed(Keys.Right))
        {
            NextFrame();
        }

        if (keyboard.IsKeyPressed(Keys.C))
        {
            var dec = toolBar.ColorPicker.Decorators[0];

            var newMirr = (int)dec.Mirror;
            newMirr++;

            if (newMirr > 2)
            {
                newMirr = 0;
            }
            
            toolBar.ColorPicker.Decorators[0] = new CellDecorator(dec.Color, dec.Glyph, (Mirror)newMirr);

            toolBar.ColorPicker.IsDirty = true;
        }
        
        if (keyboard.IsKeyPressed(Keys.V))
        {
            var newMirr = (int)toolBar.brushButton.MirrorMode;
            newMirr++;

            if (newMirr > 2)
            {
                newMirr = 0;
            }
            
            toolBar.brushButton.MirrorMode = (Mirror)newMirr;

            toolBar.brushButton.IsDirty = true;
        }

        return base.ProcessKeyboard(keyboard);
    }
}