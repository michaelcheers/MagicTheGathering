using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    internal abstract class CardReference
    {
        internal enum CardLocation
        {
            BattleField,
            Hand,
            Deck
        }
        public readonly MagicCard card;

        internal MagicCard Card
        {
            get
            {
                return card;
            }
        }

        public abstract CardLocation Location
        {
            get;
        }

        internal CardReference (MagicCard card)
        {
            this.card = card;
        }
    }
}
