using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class DeckCardReference : CardReference
    {
        internal DeckCardReference(CardReference card) : base(card)
        {
        }

        internal DeckCardReference(MagicCard card) : base(card, new CardID()) // this is where new IDs are born
        {
        }

        public static DeckCardReference MakeReference (MagicCard card)
        {
            return new DeckCardReference(card);
        }

        protected override void UpdateAbilites(List<AbilityInstance> abilities)
        {
            throw new NotImplementedException();
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
