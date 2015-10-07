using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class ActivatedAbility : Ability
    {
        public ActivatedAbility (Cost cost = null, MagicCardAction toDo = null, bool isManaAbility = false, bool hasTapSymbol = false, bool hasUnTapSymbol = false)
        {
            if (cost != null)
                cost = new Cost();
            this.isManaAbility = isManaAbility;
            this.cost = cost;
            this.toDo = toDo;
            this.hasTapSymbol = hasTapSymbol;
            this.hasUnTapSymbol = hasUnTapSymbol;
        }

        public void Activate (BattlefieldCardReference reference)
        {
            if (hasTapSymbol)
                if (reference.isUntapped)
                    reference.isTapped = true;
                else
                    return;
            toDo.Run();
        }
        
        Cost cost;
        MagicCardAction toDo;
        bool hasTapSymbol;
        bool hasUnTapSymbol;
        bool isManaAbility;
    }
}
