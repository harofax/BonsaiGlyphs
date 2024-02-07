using SadConsole.UI.Controls;

namespace AsciiAnimator.Code.Util;

public class ButtonGlyphBox : ButtonBox
{
    public int BaseGlyph { get; set; }

    public Mirror MirrorMode { get; set; }
    public List<CellDecorator> Decorators { get; } = new List<CellDecorator>();

    public ButtonGlyphBox(int width, int height, int glyph, Mirror mirrorMode = Mirror.None) : base(width, height)
    {
        BaseGlyph = glyph;
        this.MirrorMode = mirrorMode;
    }

    public void AddDecorator(CellDecorator decorator)
    {
        Decorators.Add(decorator);
    }

    public void SetMirrorMode(Mirror mirror)
    {
        MirrorMode = mirror;
    }

    public override void UpdateAndRedraw(TimeSpan time)
    {
        base.UpdateAndRedraw(time);
        
        Surface.SetGlyph(Width/2, Height/2, BaseGlyph, Surface.DefaultForeground, Surface.DefaultBackground, MirrorMode, Decorators);
    }
}