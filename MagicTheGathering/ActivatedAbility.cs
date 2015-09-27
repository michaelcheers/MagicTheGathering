using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class ActivatedAbility
    {
        public ActivatedAbility (bool isManaAbility = false)
        {
            this.isManaAbility = isManaAbility;
        }
        bool isManaAbility;
    }
}
