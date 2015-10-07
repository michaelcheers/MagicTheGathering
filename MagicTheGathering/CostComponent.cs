using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    abstract class CostComponent
    {
        public abstract bool IsPayable
        {
            get;
        }
        public abstract Choices GetChoices(); 
    }
}
