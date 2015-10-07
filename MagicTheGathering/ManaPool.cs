using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    public class ManaPool
    {
        internal List<ManaAmount> amounts;

        public ManaAmount[] Amounts
        {
            get
            {
                return amounts.ToArray();
            }
        }

        public void Empty ()
        {
            amounts.Clear();
        }

        public ManaPool()
        {
            amounts = new List<ManaAmount>();
        }

        internal void Add (ManaAmount amount)
        {
            amounts.Add(amount);
        }
    }
}