using AsciiAnimator.Code.Util;
using AsciiAnimator.Scenes.Code.Screens;
using Microsoft.VisualBasic.CompilerServices;
using SadConsole.Input;
using SadConsole.UI;
using SadConsole.UI.Controls;

namespace AsciiAnimator.Code.Windows;

public class NewAnimationDialogue : Window
{
    private IntegerTextField heightInputField;
    private IntegerTextField widthInputField;

    private MainMenu menu;
    
    public NewAnimationDialogue(int width, int height, MainMenu menu) : base(width, height)
    {
        this.menu = menu;
        var theme = ProgramSettings.THEME;
        this.Controls.ThemeColors = theme;
        this.BorderLineStyle = ICellSurface.ConnectedLineThick;


        CloseOnEscKey = true;
        
        Surface.Fill(Color.Transparent, theme.ControlHostBackground);

        Label widthLabel = new Label("Width");
        widthLabel.TabStop = false;
        widthInputField = new IntegerTextField(6, 32);
        
        
        Label heightLabel = new Label("Height");
        heightLabel.TabStop = false;
        heightInputField = new IntegerTextField(6, 16);

        widthLabel.Position = (3, 3);
        Controls.Add(widthLabel);
        widthInputField.PlaceRelativeTo(widthLabel, Direction.Types.Right, 4);
        
        heightLabel.PlaceRelativeTo(widthLabel, Direction.Types.Down);
        heightInputField.PlaceRelativeTo(widthInputField, Direction.Types.Down);

        ButtonBox createButton = new ButtonBox(10, 3);
        createButton.Text = "Create";
        createButton.UseExtended = true;
        createButton.Position = createButton.Surface.Area.WithCenter(Surface.Area.Center).WithY(Height - 5).Position;
        
        createButton.MouseButtonClicked += CreateNewAnimation;
        createButton.TabStop = false;
        
        Controls.Add(createButton);
        
        Controls.Add(widthInputField);
        Controls.Add(heightInputField);
        
        CloseOnEscKey = false;
        
        Title = "[ New Animation ]";
        IsModalDefault = true;
        
        
          
        //Border.CreateForWindow(this);
    }

    private void CreateNewAnimation(object? sender, ControlBase.ControlMouseState e)
    {
        int width = int.Parse(widthInputField.Text);
        int height = int.Parse(heightInputField.Text);

        //menu.IsFocused = false;
        //menu.Dispose();
        //menu = null;
        
        var animationScreen = new AnimationScreen(width, height);
        Game.Instance.Screen = animationScreen;
        Game.Instance.Screen.IsFocused = true;
        
        Hide();
    }

    protected override void OnShown()
    {
        base.OnShown();
    }
    
}