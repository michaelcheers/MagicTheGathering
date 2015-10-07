using System.Collections.Generic;

namespace MagicTheGathering
{
    internal class Cost
    {
        List<CostComponent> payment;
        

        public Cost (List<CostComponent> components)
        {
            payment = components;
        }

        internal CostComponent[] Payment
        {
            get
            {
                return payment.ToArray();
            }
        }
    }
}