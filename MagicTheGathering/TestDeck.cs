using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class TestDeck : Deck
    {
        MagicCard card;
        public TestDeck (MagicCard card)
        {
            this.card = card;
        }
        public override int Length
        {
            get
            {
                return 60;
            }
        }

        public override CardReference DrawCard(int index)
        {
            return new DeckCardReference(card);
        }

        public override CardReference GetCard(int index)
        {
            return new DeckCardReference(card);
        }
    }
}
