using AsciiAnimator.Code.Util;
using SadConsole.UI;
using SadConsole.UI.Controls;

namespace AsciiAnimator.Code.Tools;

public class ToolBar : ControlsConsole
{
    public ButtonGlyphBox brushButton;
    public ButtonGlyphBox ColorPicker { get; }

    public ToolBar(int width, int height) : base(width, height)
    {
        Controls.ThemeColors = ProgramSettings.THEME;
        
        Surface.Fill(null, ProgramSettings.THEME.ControlBackgroundNormal);
        
        Surface.DrawBox(Surface.Area,
            ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThinExtended,
                new ColoredGlyph(Controls.ThemeColors.ControlForegroundNormal,
                    Controls.ThemeColors.ControlBackgroundNormal)));

        brushButton = new ButtonGlyphBox(5, 3, 223);
        brushButton.Surface.DefaultForeground = Color.Orange;
        brushButton.AddDecorator(new CellDecorator(Color.Orange, 'V', Mirror.None));
        //brushButton.AddDecorator(new CellDecorator(Color.Orange, 208, Mirror.Vertical));
        //brush.AddDecorator(new CellDecorator(Color.White, 174, Mirror.None));
        
        ButtonGlyphBox eraser = new ButtonGlyphBox(5, 3, 220);
        eraser.AddDecorator(new CellDecorator(Color.Gray, 205, Mirror.None));
        eraser.Surface.DefaultBackground = Color.Pink;
        
        
        ColorPicker = new ButtonGlyphBox(5, 3, 216);
        ColorPicker.Surface.DefaultForeground = Color.White;
        ColorPicker.AddDecorator(new CellDecorator(Color.SlateGray, 6, Mirror.None));

        brushButton.Text = "B";
        brushButton.TextAlignment = HorizontalAlignment.Center;
        
        eraser.Text = "E";
        eraser.TextAlignment = HorizontalAlignment.Center;
        
        ColorPicker.Text = "P";
        ColorPicker.TextAlignment = HorizontalAlignment.Center;
        
        Controls.Add(brushButton);
        brushButton.Position = ((Width / 2) - brushButton.Width / 2, 1);
        
        eraser.PlaceRelativeTo(brushButton, Direction.Types.Down);
        ColorPicker.PlaceRelativeTo(eraser, Direction.Types.Down);
        
        

    }
    
}