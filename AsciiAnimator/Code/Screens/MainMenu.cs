using AsciiAnimator.Code.Windows;
using SadConsole.Input;
using SadConsole.Readers;
using SadConsole.UI;
using SadConsole.UI.Controls;

namespace AsciiAnimator.Scenes.Code.Screens;

public class MainMenu : ControlsConsole
{
    private ButtonBox createNewButton;
    private ButtonBox loadButton;
    private ButtonBox exitButton;
    private int buttonPadding = 4;
    private NewAnimationDialogue newAnimationPopup;

    public MainMenu(int width, int height) : base(width, height)
    {
        newAnimationPopup  = new NewAnimationDialogue(50, 10, this);
        //var colorSchemeFile = File.ReadAllText(ProgramSettings.THEME_JSON_PATH);


        //var colorScheme = Serializer.Load<Colors>(ProgramSettings.THEME_JSON_PATH, false);
        Controls.ThemeColors = ProgramSettings.THEME;

        createNewButton =
            new ButtonBox(ProgramSettings.MAIN_MENU_BUTTON_WIDTH, ProgramSettings.MAIN_MENU_BUTTON_HEIGHT)
            {
                TextAlignment = HorizontalAlignment.Stretch,
                Text = "New",
                CanFocus = false,
                UseExtended = true
            };
        
        createNewButton.MouseButtonClicked += CreateNewAnimationButtonClicked;

        createNewButton.Position = createNewButton.Surface.Area.WithCenter(Surface.Area.Center).Position
            .WithY((Height / 7) * 4);
        Controls.Add(createNewButton);

        loadButton = new ButtonBox(ProgramSettings.MAIN_MENU_BUTTON_WIDTH, ProgramSettings.MAIN_MENU_BUTTON_HEIGHT)
        {
            TextAlignment = HorizontalAlignment.Stretch,
            Text = "Load",
            CanFocus = false,
            UseExtended = true
        };
        //loadButton.AutoSize = true;
        loadButton.PlaceRelativeTo(createNewButton, Direction.Down, buttonPadding);


        exitButton = new ButtonBox(ProgramSettings.MAIN_MENU_BUTTON_WIDTH, ProgramSettings.MAIN_MENU_BUTTON_HEIGHT)
        {
            TextAlignment = HorizontalAlignment.Stretch,
            Text = "Exit",
            UseExtended = true,
            CanFocus = false
        };
        //exitButton.AutoSize = true;
        exitButton.PlaceRelativeTo(loadButton, Direction.Types.Down, buttonPadding);
        exitButton.MouseButtonClicked += ExitButtonClicked;

        newAnimationPopup.Center();
        
        PrintTitle();
    }

    private void CreateNewAnimationButtonClicked(object? sender, ControlBase.ControlMouseState e)
    {
        newAnimationPopup.Show();
    }

    private void PrintTitle()
    {
        IsDirty = true;

        var colors = Controls.GetThemeColors();

        var tdf = TheDrawFont.ReadFonts(ProgramSettings.TD_FONT_PATH).ToArray()[0];

        tdf.Type = TheDrawFont.FontType.Outline;

        int titleWidth = 0;
        int titleHeight = 5;
        
        foreach (char ch in "ASCII")
        {
            titleWidth += tdf.GetCharacter(ch).Width;
        }
        
        Surface.PrintTheDraw((Surface.Width - titleWidth) / 2,titleHeight, "ASCII", tdf);
        
        titleWidth = 0;
        titleHeight += tdf.GetCharacter('A').Rows.Length; // shift down a row
        
        foreach (char ch in "Animator")
        {
            titleWidth += tdf.GetCharacter(ch).Width;
        }
        
        Surface.PrintTheDraw((Surface.Width - titleWidth) / 2,titleHeight, "Animator", tdf);
        
        
        // fill background to eliminate the black bg for the title
        Surface.Fill(null, colors.ControlHostBackground, null);
        
    }

    private void ExitButtonClicked(object? sender, ControlBase.ControlMouseState e)
    {
        Game.Instance.MonoGameInstance.Exit();
    }
}