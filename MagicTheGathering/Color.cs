using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    [Flags]
    public enum Color
    {
        Colorless = 0,
        White = 1,
        Blue = 2,
        Black = 4,
        Red = 8,
        Green = 16,
        AllColors = White+Blue+Black+Red+Green,
    };
}
