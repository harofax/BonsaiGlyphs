using SadConsole.Input;

namespace AsciiAnimator.Scenes.Code.Screens;

public class AnimationScreen : ScreenObject
{
    private ScreenSurface surf;
    public AnimationScreen(uint width, uint height)
    {

        surf = new ScreenSurface((int)width, (int)height);
        surf.Fill(Color.Transparent, Color.Red);
        UseKeyboard = true;
        IsFocused = true;

    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Escape))
        {
            Game.Instance.MonoGameInstance.Exit();
        }
        return base.ProcessKeyboard(keyboard);
    }
}