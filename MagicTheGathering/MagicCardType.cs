using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    [Flags]
    enum MagicCardType
    {
        Creature = 1,
        Artifact = 2,
        Basic = 4,
        Enchantment = 8,
        Instant = 0x10,
        Land = 0x20,
        Legendary = 0x40,
        Planeswalker = 0x80,
        Snow = 0x100,

    }
}
