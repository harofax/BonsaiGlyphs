using SadConsole.UI.Controls;

namespace AsciiAnimator.Code.Util;

public class IntegerTextField : TextBox
{
    public IntegerTextField(int width) : base(width)
    {
        _text = "1";
    }

    protected override void OnUnfocused()
    {
        ValidateNumber();
        base.OnUnfocused();
    }
    

    public uint NumberMax { get; set; } = UInt32.MaxValue;
    private uint _numberMin = 1;
    private bool UseMaxValue => NumberMax < uint.MaxValue;

    private void ValidateNumber()
    {
        if (uint.TryParse(_text, out uint num))
        {
            num = Math.Clamp(num, _numberMin, NumberMax);
        }
        else
        {
            num = _numberMin;
        }

        Text = num.ToString();

        IsDirty = true;
    }
}