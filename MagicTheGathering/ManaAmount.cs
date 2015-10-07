using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    public struct ManaAmount
    {
        public int redAmount;
        public int blueAmount;
        public int greenAmount;
        public int blackAmount;
        public int whiteAmount;
        public int colorlessAmount;

        public ManaAmount(int redAmount, int blueAmount, int greenAmount, int blackAmount, int whiteAmount, int colorlessAmount)
        {
            this.redAmount = redAmount;
            this.blueAmount = blueAmount;
            this.greenAmount = greenAmount;
            this.blackAmount = blackAmount;
            this.whiteAmount = whiteAmount;
            this.colorlessAmount = colorlessAmount;
        }

        public void Clear()
        {
            whiteAmount = 0;
            blueAmount = 0;
            blackAmount = 0;
            redAmount = 0;
            greenAmount = 0;
            colorlessAmount = 0;
        }

        public int GetAmount(Color c)
        {
            switch (c)
            {
                case Color.White: return whiteAmount;
                case Color.Blue: return blueAmount;
                case Color.Black: return blackAmount;
                case Color.Red: return redAmount;
                case Color.Green: return greenAmount;
                case Color.Colorless: return colorlessAmount;
                default: throw new ArgumentException("Invalid color for mana get!");
            }
        }

        public void Add(ManaAmount amount)
        {
            whiteAmount += amount.whiteAmount;
            blueAmount += amount.blueAmount;
            blackAmount += amount.blackAmount;
            redAmount += amount.redAmount;
            greenAmount += amount.greenAmount;
            colorlessAmount += amount.colorlessAmount;
        }

        public void AddAmount(Color c, int amount)
        {
            switch (c)
            {
                case Color.White: whiteAmount += amount; break;
                case Color.Blue: blueAmount += amount; break;
                case Color.Black: blackAmount += amount; break;
                case Color.Red: redAmount += amount; break;
                case Color.Green: greenAmount += amount; break;
                case Color.Colorless: colorlessAmount += amount; break;
                default: throw new ArgumentException("Invalid color for mana add!");
            }
        }

        public bool TrySubtractAmount(Color c, int amount)
        {
            switch (c)
            {
                case Color.White: return TrySubtractAmount(amount, ref whiteAmount);
                case Color.Blue: return TrySubtractAmount(amount, ref blueAmount);
                case Color.Black: return TrySubtractAmount(amount, ref blackAmount);
                case Color.Red: return TrySubtractAmount(amount, ref redAmount);
                case Color.Green: return TrySubtractAmount(amount, ref greenAmount);
                case Color.Colorless: return TrySubtractAmount(amount, ref colorlessAmount);
                default: throw new ArgumentException("Invalid color for mana subtract!");
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
