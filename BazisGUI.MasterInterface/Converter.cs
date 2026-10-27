using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.MasterInterface
{
    public static class Converter
    {
        public static GroupType GetGroupTypeFromString(string input)
        {
            if (Enum.TryParse(input, out GroupType res))
                return res;
            throw new ArgumentException($"Не удалось определить тип \"GroupType\" по строке \"{input}\"");
        }
    }
}
