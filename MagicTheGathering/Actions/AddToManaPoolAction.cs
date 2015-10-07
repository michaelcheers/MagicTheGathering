using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering.Abilities
{
    class AddToManaPoolAction : MagicCardAction
    {
        Player you;
        ManaAmount toAdd;

        public AddToManaPoolAction ()
        {

        }
        
        public AddToManaPoolAction (Player you, ManaAmount toAdd)
        {
            this.toAdd = toAdd;
            this.you = you;
        }

        public override void Run()
        {
            you.manaPool.Add(toAdd);
        }
    }
}
