using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class HandCardReference : CardReference
    {
        internal HandCardReference(CardReference card) : base(card)
        {
        }

        public override CardLocation Location
        {
            get
            {
                return CardLocation.Hand;
            }
        }
    }
}
