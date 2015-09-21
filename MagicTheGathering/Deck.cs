using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    abstract class Deck
    {
        public abstract CardReference GetCard(int index);
        public abstract CardReference DrawCard(int index);
        public abstract int Length { get; }
        public virtual CardReference[] DrawCard(int index, int length)
        {
            CardReference[] result = new CardReference[length];
            for (int n = 0; n < length; n++)
            {
                result[n] = DrawCard(index);
            }
            return result;
        }
        public virtual CardReference[] DrawTopCard(int length)
        {
            CardReference[] result = new CardReference[length];
            for (int n = 0; n < length; n++)
            {
                result[n] = DrawCard(0);
            }
            return result;
        }
        public virtual CardReference GetTopCard() { return GetCard(0); }
        public virtual CardReference DrawTopCard() { return DrawCard(0); }
        
    }
}
