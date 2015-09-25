using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    public class ManaPool
    {
        public int redCost;
        public int blueCost;
        public int greenCost;
        public int blackCost;
        public int whiteCost;
        public int colorlessCost;

        public void Empty ()
        {
            redCost = 0;
            blueCost = 0;
            greenCost = 0;
            blackCost = 0;
            whiteCost = 0;
            colorlessCost = 0;
        }

        public ManaPool(int redCost, int blueCost, int greenCost, int blackCost, int whiteCost, int colorlessCost)
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