using SadConsole.Effects;
using SadConsole.UI;

namespace AsciiAnimator;

static class ProgramSettings
{
    public const int PROGRAM_WIDTH = 150;
    public const int PROGRAM_HEIGHT = 50;

    public const int MAIN_MENU_BUTTON_WIDTH = 19;
    public const int MAIN_MENU_BUTTON_HEIGHT = 3;

    private const string THEME_JSON_PATH = "Assets/ascii_animator.json";

    public const string TD_FONT_PATH = "Assets/BASIC.TDF";
    
    public const int CANVAS_WIDTH = PROGRAM_WIDTH - ASCII_COLOR_PICKER_WIDTH - TOOLBAR_WIDTH;
    public const int CANVAS_HEIGHT = PROGRAM_HEIGHT - TITLEBAR_HEIGHT - TIMELINE_HEIGHT;
    
    public const int TITLEBAR_HEIGHT = 3;
    
    public const int ASCII_COLOR_PICKER_WIDTH = PROGRAM_WIDTH/5;
    
    public const int TIMELINE_HEIGHT = PROGRAM_HEIGHT / 5;
    public const int TIMELINE_WIDTH = PROGRAM_WIDTH - ASCII_COLOR_PICKER_WIDTH;
    
    public const int TOOLBAR_WIDTH = 10;
    public const int TOOLBAR_HEIGHT = 20;

    public static Colors THEME = Serializer.Load<Colors>(THEME_JSON_PATH, false);
}