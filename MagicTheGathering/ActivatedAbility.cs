using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class ActivatedAbility
    {
        public ActivatedAbility (Cost cost = null, MagicCardAction toDo = null, bool isManaAbility = false)
        {
            this.isManaAbility = isManaAbility;
            this.cost = cost;
            this.toDo = toDo;
        }

        Cost cost;
        MagicCardAction toDo;
        bool isManaAbility;
    }
}
