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

        public override MagicCard DrawCard(int index)
        {
            return card;
        }

        public override MagicCard GetCard(int index)
        {
            return card;
        }
    }
}
