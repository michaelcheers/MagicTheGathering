using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    public class ManaPool
    {
        internal ManaAmount amount;

        public void Empty ()
        {
            amount.Clear();
        }

        public ManaPool()
        {
        }

        internal void Add (ManaAmount newAmount)
        {
            amount.Add(newAmount);
        }

        public int Get(Color c)
        {
            return amount.GetAmount(c);
        }
    }
}