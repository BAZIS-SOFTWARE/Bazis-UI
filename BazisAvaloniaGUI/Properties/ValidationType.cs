using System;

namespace BazisAvaloniaGUI.Properties
{
    [Flags]
    public enum ValidationType
    {
        None = 0,
        Integer = 1,
        Float = 2,
        Text = 4,

        PositiveOnly = 8,
        NegativeAndPositive = 16,

        IntPositive = Integer | PositiveOnly,
        IntAny = Integer | NegativeAndPositive,
        FloatPositive = Float | PositiveOnly,
        FloatAny = Float | NegativeAndPositive
    }
}
