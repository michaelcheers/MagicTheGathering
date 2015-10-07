using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    [Flags]
    public enum MTGColor
    {
        NoColor = 0,
        ColorlessMana = 1,
        White = 2,
        Blue = 4,
        Black = 8,
        Red = 16,
        Green = 32,
        AllColors = White + Blue + Black + Red + Green,
        GenericMana = White + Blue + Black + Red + Green + ColorlessMana,
    };
}
