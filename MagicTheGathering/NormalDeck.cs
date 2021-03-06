﻿using System;
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
            cards = deck.ConvertAll(DeckCardReference.MakeReference);
        }

        public override CardReference DrawCard(int index)
        {
            CardReference result = GetCard(index);
            if (result != null)
            cards.RemoveAt(index);
            return result;
        }

        List<DeckCardReference> cards;

        public override CardReference GetCard(int index)
        {
            if (cards.Count == 0)
                return null;
            return cards[index];
        }
    }
}