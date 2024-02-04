using SadConsole.UI;

namespace AsciiAnimator.Code.Windows;

public class NewAnimationDialogue : Window
{
    public NewAnimationDialogue(int width, int height) : base(width, height)
    {
        Title = "New Animation";
        Surface.FillWithRandomGarbage(Font);
        this.Clear();

        IsModalDefault = true;
        
        Border.CreateForWindow(this);
    }
}