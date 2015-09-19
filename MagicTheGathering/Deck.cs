using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    abstract class Deck
    {
        public abstract MagicCard GetCard(int index);
        public abstract MagicCard DrawCard(int index);
        public abstract int Length { get; }
        public virtual MagicCard[] DrawCard(int index, int length)
        {
            MagicCard[] result = new MagicCard[length];
            for (int n = 0; n < length; n++)
            {
                result[n] = DrawCard(n + index);
            }
            return result;
        }
        public virtual MagicCard[] DrawTopCard(int length)
        {
            MagicCard[] result = new MagicCard[length];
            for (int n = 0; n < length; n++)
            {
                result[n] = DrawCard(n);
            }
            return result;
        }
        public virtual MagicCard GetTopCard() { return GetCard(0); }
        public virtual MagicCard DrawTopCard() { return DrawCard(0); }
        
    }
}
