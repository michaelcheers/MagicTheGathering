using System;
using System.Collections.Generic;

namespace MagicTheGathering
{
    internal class NormalDeck : Deck
    {
        public override int Length
        {
            get
            {
                return cards.Count;
            }
        }

        public NormalDeck (List<MagicCard> deck)
        {
            this.cards = deck;
        }

        public override MagicCard DrawCard(int index)
        {
            MagicCard result = GetCard(index);
            cards.RemoveAt(index);
            return result;
        }

        List<MagicCard> cards;

        public override MagicCard GetCard(int index)
        {
            return cards[index];
        }
    }
}