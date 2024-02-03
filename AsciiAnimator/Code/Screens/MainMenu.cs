using System.Text.Json;
using AsciiAnimator.Code.Util;
using Newtonsoft.Json;
using SadConsole.UI;
using SadConsole.UI.Controls;

namespace AsciiAnimator.Scenes.Code.Screens;

public class MainMenu : ControlsConsole
{
    private ButtonLine createNewButton;

    public MainMenu(int width, int height) : base(width, height)
    {
        //var colorSchemeFile = File.ReadAllText(ProgramSettings.THEME_JSON_PATH);
        
        
        var colorScheme = SadConsole.Serializer.Load<Colors>(ProgramSettings.THEME_JSON_PATH, false);

        Controls.ThemeColors = colorScheme;

        
        createNewButton = new ButtonLine(ProgramSettings.MAIN_MENU_BUTTON_WIDTH, ProgramSettings.MAIN_MENU_BUTTON_HEIGHT)
            {
                TextAlignment = HorizontalAlignment.Stretch,
                Text = "New",
                UseExtended = true
            };
        //createNewButton.AutoSize = true;

        createNewButton.Position = createNewButton.Surface.Area.WithCenter(Surface.Area.Center).Position.WithY((Height/4) * 2);
        Controls.Add(createNewButton);

        Button loadButton = new Button(ProgramSettings.MAIN_MENU_BUTTON_WIDTH, ProgramSettings.MAIN_MENU_BUTTON_HEIGHT);
        loadButton.Text = "Load";
        loadButton.TextAlignment = HorizontalAlignment.Stretch;
        //loadButton.AutoSize = true;
        loadButton.PlaceRelativeTo(createNewButton, Direction.Down);
        
        
        Button exitButton = new Button(ProgramSettings.MAIN_MENU_BUTTON_WIDTH, ProgramSettings.MAIN_MENU_BUTTON_HEIGHT);
        exitButton.Text = "EXIT";
        exitButton.TextAlignment = HorizontalAlignment.Center;
        //exitButton.AutoSize = true;
        exitButton.PlaceRelativeTo(loadButton, Direction.Types.Down);
        Surface.Fill(null ,new Color(0.1f, 0, 0.1f));
       
    }

    public MainMenu(int viewWidth, int viewHeight, int totalWidth, int totalHeight) : base(viewWidth, viewHeight,
        totalWidth, totalHeight)
    {
    }

    public override void Render(TimeSpan delta)
    {
        base.Render(delta);
        //createNewButton.Surface.DrawBox(createNewButton.Surface.Area, ShapeParameters.CreateStyledBoxThin(Color.Gold));

    }
}