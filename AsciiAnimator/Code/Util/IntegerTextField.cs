using SadConsole.UI.Controls;

namespace AsciiAnimator.Code.Util;

public class IntegerTextField : TextBox
{
    private readonly int _numberMin = 1;

    public IntegerTextField(int width, int defaultValue) : base(width)
    {
        Text = defaultValue.ToString();
    }


    public int NumberMax { get; set; } = int.MaxValue;

    protected override void OnUnfocused()
    {
        ValidateNumber();
        base.OnUnfocused();
    }

    private void ValidateNumber()
    {
        if (int.TryParse(_text, out var num))
            num = Math.Clamp(num, _numberMin, NumberMax);
        else
            num = _numberMin;

        Text = num.ToString();

        IsDirty = true;
    }
}