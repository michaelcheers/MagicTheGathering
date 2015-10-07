using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class ActivatedAbility
    {
        public ActivatedAbility (Cost cost = null, MagicCardAction toDo = null, bool isManaAbility = false, bool hasTapSymbol = false, bool hasUnTapSymbol = false)
        {
            this.isManaAbility = isManaAbility;
            this.cost = cost;
            this.toDo = toDo;
            this.hasTapSymbol = hasTapSymbol;
            this.hasUnTapSymbol = hasUnTapSymbol;
        }

        Cost cost;
        MagicCardAction toDo;
        bool hasTapSymbol;
        bool hasUnTapSymbol;
        bool isManaAbility;
    }
}
