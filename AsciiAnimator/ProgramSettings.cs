﻿using SadConsole.Effects;
using SadConsole.UI;

namespace AsciiAnimator;

static class ProgramSettings
{
    public const int GAME_WIDTH = 100;
    public const int GAME_HEIGHT = 40;

    public const int MAIN_MENU_BUTTON_WIDTH = 19;
    public const int MAIN_MENU_BUTTON_HEIGHT = 3;

    public const string THEME_JSON_PATH = "Assets/ascii_animator.json";

    public const string TD_FONT_PATH = "Assets/BASIC.TDF";

    public static Colors THEME = Serializer.Load<Colors>(THEME_JSON_PATH, false);
}