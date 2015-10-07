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

        public int GetAmount(Color c)
        {
            switch (c)
            {
                case Color.White: return whiteCost;
                case Color.Blue: return blueCost;
                case Color.Black: return blackCost;
                case Color.Red: return redCost;
                case Color.Green: return greenCost;
                default: throw new ArgumentException("Invalid color for mana!");
            }
        }

        public void AddAmount(Color c, int amount)
        {
            switch (c)
            {
                case Color.White: whiteCost += amount; break;
                case Color.Blue: blueCost += amount; break;
                case Color.Black: blackCost += amount; break;
                case Color.Red: redCost += amount; break;
                case Color.Green: greenCost += amount; break;
                default: throw new ArgumentException("Invalid color for mana!");
            }
        }

        public bool TrySubtractAmount(Color c, int amount)
        {
            switch (c)
            {
                case Color.White: return TrySubtractAmount(amount, ref whiteCost);
                case Color.Blue: return TrySubtractAmount(amount, ref blueCost);
                case Color.Black: return TrySubtractAmount(amount, ref blackCost);
                case Color.Red: return TrySubtractAmount(amount, ref redCost);
                case Color.Green: return TrySubtractAmount(amount, ref greenCost);
                default: throw new ArgumentException("Invalid color for mana!");
            }
        }

        bool TrySubtractAmount(int amount, ref int internalAmount)
        {
            if(internalAmount >= amount)
            {
                internalAmount -= amount;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
