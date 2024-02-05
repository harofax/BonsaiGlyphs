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

    public NewAnimationDialogue(int width, int height) : base(width, height)
    {
        var theme = ProgramSettings.THEME;
        this.Controls.ThemeColors = theme;
        this.BorderLineStyle = ICellSurface.ConnectedLineThick;


        CloseOnEscKey = true;
        
        Surface.Fill(Color.Transparent, theme.ControlHostBackground);

        Label widthLabel = new Label("Width");
        widthInputField = new IntegerTextField(6);
        
        Label heightLabel = new Label("Height");
        heightInputField = new IntegerTextField(6);

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
        uint width = uint.Parse(widthInputField.Text);
        uint height = uint.Parse(heightInputField.Text);
        Game.Instance.Screen = new AnimationScreen(width, height);
    }

    protected override void OnShown()
    {
        base.OnShown();
    }
    
}