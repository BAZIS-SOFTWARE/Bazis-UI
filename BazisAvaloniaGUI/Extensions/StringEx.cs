using BazisAvaloniaGUI.Localization;
using System;

namespace BazisAvaloniaGUI.Extensions
{
    public static class StringEx
    {
        /// <summary>
        /// Метод для преобразование из строки в enum с проверкой на ошибки
        /// </summary>
        public static T ToEnum<T>(this string value) where T : struct, Enum
        {
            if (Enum.TryParse(value, out T result))
                return result;
            else throw new ArgumentException(
                Resources.StringEx_ToEnum_ArgumentException_Part1 +
                $" '{value}' " +
                Resources.StringEx_ToEnum_ArgumentException_Part2);
        }

        public static bool TryToEnum<T>(this string value, out T result) where T : struct, Enum
        {
            return Enum.TryParse(value, out result);
        }
    }
}
