using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    public struct ManaAmount
    {
        public int redCost;
        public int blueCost;
        public int greenCost;
        public int blackCost;
        public int whiteCost;
        public int colorlessCost;

        public ManaAmount (int redCost, int blueCost, int greenCost, int blackCost, int whiteCost, int colorlessCost)
        {
            this.redCost = redCost;
            this.blueCost = blueCost;
            this.greenCost = greenCost;
            this.blackCost = blackCost;
            this.whiteCost = whiteCost;
            this.colorlessCost = colorlessCost;
        }
    }
}
