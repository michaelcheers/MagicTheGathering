﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class BattlefieldCardReference : CardReference
    {
        internal BattlefieldCardReference(CardReference card) : base(card)
        {
        }

        public override CardLocation Location
        {
            get
            {
                return CardLocation.BattleField;
            }
        }
    }
}
