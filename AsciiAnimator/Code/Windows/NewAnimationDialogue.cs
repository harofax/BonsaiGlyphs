using SadConsole.Input;
using SadConsole.UI;

namespace AsciiAnimator.Code.Windows;

public class NewAnimationDialogue : Window
{
    public NewAnimationDialogue(int width, int height) : base(width, height)
    {
        this.Controls.ThemeColors = ProgramSettings.THEME;
        this.BorderLineStyle = ICellSurface.ConnectedLineThick;
        //Surface.FillWithRandomGarbage(Font);

        //Surface.DrawCircle(Surface.Area, ShapeParameters.CreateFilled(new ColoredGlyph(Color.Red,Color.Green, '#'), new ColoredGlyph(Color.Black, Color.Blue, '.')));

        Surface.Fill(Color.Transparent, ProgramSettings.THEME.ControlHostBackground);
        
        CloseOnEscKey = true;
        
        Title = "[ New Animation ]";
        IsModalDefault = true;
        
        
          
        //Border.CreateForWindow(this);
    }

    protected override void OnShown()
    {
        base.OnShown();
    }

    public override bool ProcessMouse(MouseScreenObjectState mouse)
    {
        bool basehandled = base.ProcessMouse(mouse);

        

        return basehandled;
    }
    
}