using System.Runtime.Serialization;
using SadConsole.UI;
using SadConsole.UI.Controls;

namespace AsciiAnimator.Code.Util;

/// <summary>
/// A 3D theme of the button control using thin lines. Supports the SadConsole extended character set.
/// </summary>
[DataContract]
public class ButtonLine : ButtonBase
{
    public ButtonLine(int width, int height) : base(width, height)
    {
    }

    /// <summary>
    /// When <see langword="true"/>, indicates that the lines of the theme should use the extended SadConsole font characters if available.
    /// </summary>
    [DataMember]
    public bool UseExtended { get; set; }

    /// <summary>
    /// Creates a new instance of the object.
    /// </summary>
    public ButtonLine() =>
        UseExtended = true;

    /// <inheritdoc />
    protected override void RefreshThemeStateColors(Colors colors)
    {
        base.RefreshThemeStateColors(colors);

        ThemeState.Normal.Background = colors.GetOffColor(ThemeState.Normal.Background, colors.ControlHostBackground);
        ThemeState.MouseOver.Background = colors.GetOffColor(ThemeState.MouseOver.Background, colors.ControlHostBackground);
        ThemeState.MouseDown.Background = colors.GetOffColor(ThemeState.MouseDown.Background, colors.ControlHostBackground);
        ThemeState.Focused.Background = colors.GetOffColor(ThemeState.Focused.Background, colors.ControlHostBackground);
    }
        
    /// <inheritdoc />
    public override void UpdateAndRedraw(TimeSpan time)
    {
        if (!IsDirty) return;

        RefreshThemeStateColors(FindThemeColors());
        ColoredGlyphBase appearance;
        bool mouseDown = false;
        bool focused = false;

        appearance = ThemeState.GetStateAppearance(State);


        if (Helpers.HasFlag((int)State, (int)ControlStates.MouseLeftButtonDown) ||
            Helpers.HasFlag((int)State, (int)ControlStates.MouseRightButtonDown))
            mouseDown = true;
            
        // Middle part of the button for text.
        int middle = Surface.Height != 1 ? Surface.Height / 2 : 0;
        Color topleftcolor = !mouseDown ? FindThemeColors().Lines.ComputedColor.GetBright() : FindThemeColors().Lines.ComputedColor.GetDarker();
        Color bottomrightcolor = !mouseDown ? FindThemeColors().Lines.ComputedColor.GetDarker() : FindThemeColors().Lines.ComputedColor.GetBright();

        if (Surface.Height > 1 && Surface.Height % 2 == 0)
            middle -= 1;

        // Extended font draw
        if (Parent.Host.ParentConsole.Font.IsSadExtended && UseExtended)
        {
            // Redraw the control
            Surface.Fill(appearance.Foreground, appearance.Background,
                appearance.Glyph, Mirror.None);

            Surface.Print(0, middle, Text.Align(TextAlignment, Width), appearance);

            if (Height == 1)
            {
                Surface.SetDecorator(0, Surface.Width,
                    new GlyphDefinition(ICellSurface.ConnectedLineThinExtended[1], Mirror.None).CreateCellDecorator(topleftcolor),
                    new GlyphDefinition(ICellSurface.ConnectedLineThinExtended[7], Mirror.None).CreateCellDecorator(bottomrightcolor));
                Surface.AddDecorator(0, 1, Parent.Host.ParentConsole.Font.GetDecorator("box-edge-left", topleftcolor));
                Surface.AddDecorator(Surface.Width - 1, 1, Parent.Host.ParentConsole.Font.GetDecorator("box-edge-right", bottomrightcolor));
            }
            else if (Height == 2)
            {
                Surface.SetDecorator(0, Surface.Width,
                    new GlyphDefinition(ICellSurface.ConnectedLineThinExtended[1], Mirror.None).CreateCellDecorator(topleftcolor));

                    


                Surface.SetDecorator(Point.ToIndex(0,  Surface.Height - 1, Surface.Width), Surface.Width,
                    new GlyphDefinition(ICellSurface.ConnectedLineThinExtended[7], Mirror.None).CreateCellDecorator(bottomrightcolor));

                Surface.AddDecorator(0, 1, Parent.Host.ParentConsole.Font.GetDecorator("box-edge-left", topleftcolor));
                Surface.AddDecorator(Point.ToIndex(0,  1, Surface.Width), 1, Parent.Host.ParentConsole.Font.GetDecorator("box-edge-left", topleftcolor));
                Surface.AddDecorator(Surface.Width - 1, 1, Parent.Host.ParentConsole.Font.GetDecorator("box-edge-right", bottomrightcolor));
                Surface.AddDecorator(Point.ToIndex(Surface.Width - 1,  1, Surface.Width), 1, Parent.Host.ParentConsole.Font.GetDecorator("box-edge-right", bottomrightcolor));
            }
            else
            {
                Surface.SetDecorator(0, Surface.Width,
                    new GlyphDefinition(ICellSurface.ConnectedLineThinExtended[1], Mirror.None).CreateCellDecorator(topleftcolor));

                Surface.SetDecorator(Point.ToIndex(0,  Surface.Height - 1, Surface.Width), Surface.Width,
                    new GlyphDefinition(ICellSurface.ConnectedLineThinExtended[7], Mirror.None).CreateCellDecorator(bottomrightcolor));

                Surface.AddDecorator(0, 1, Parent.Host.ParentConsole.Font.GetDecorator("box-edge-left", topleftcolor));
                Surface.AddDecorator(Point.ToIndex(0,  Surface.Height - 1, Surface.Width), 1, Parent.Host.ParentConsole.Font.GetDecorator("box-edge-left", topleftcolor));
                Surface.AddDecorator(Surface.Width - 1, 1, Parent.Host.ParentConsole.Font.GetDecorator("box-edge-right", bottomrightcolor));
                Surface.AddDecorator(Point.ToIndex(Surface.Width - 1,  Surface.Height - 1, Surface.Width), 1, Parent.Host.ParentConsole.Font.GetDecorator("box-edge-right", bottomrightcolor));

                for (int y = 0; y < Surface.Height - 2; y++)
                {
                    Surface.AddDecorator(Point.ToIndex(0,  y + 1, Surface.Width), 1, Parent.Host.ParentConsole.Font.GetDecorator("box-edge-left", topleftcolor));
                    Surface.AddDecorator(Point.ToIndex(Surface.Width - 1,  y + 1, Surface.Width), 1, Parent.Host.ParentConsole.Font.GetDecorator("box-edge-right", bottomrightcolor));
                }
            }
        }
        else // Non extended normal draw
        {
            Surface.Fill(appearance.Foreground, appearance.Background,
                appearance.Glyph, Mirror.None);

            Surface.Print(1, middle, Text.Align(TextAlignment, Width - 2), appearance);
                
            Surface.DrawBox(new Rectangle(0,0,Width,Surface.Height),
                focused ? ShapeParameters.CreateStyledBoxThick(topleftcolor) : ShapeParameters.CreateStyledBoxThin(topleftcolor));

            //SadConsole.Algorithms.Line(0, 0, Width - 1, 0, (x, y) => { return true; });

            Surface.DrawLine(new Point(0, 0), new Point(Width - 1, 0), null, topleftcolor, appearance.Background);
            Surface.DrawLine(new Point(0, 0), new Point(0, Surface.Height - 1), null, topleftcolor, appearance.Background);
            Surface.DrawLine(new Point(Width - 1, 0), new Point(Width - 1, Surface.Height - 1), null, bottomrightcolor, appearance.Background);
            Surface.DrawLine(new Point(1, Surface.Height - 1), new Point(Width - 1, Surface.Height - 1), null, bottomrightcolor, appearance.Background);
        }

        IsDirty = false;
    }
}