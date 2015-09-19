using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class DeckCardReference : CardReference
    {
        internal DeckCardReference(MagicCard card) : base(card)
        {
        }

        public static DeckCardReference MakeReference (MagicCard card)
        {
            return new DeckCardReference(card);
        }

        public override CardLocation Location
        {
            get
            {
                return CardLocation.Deck;
            }
        }
        
    }
}
