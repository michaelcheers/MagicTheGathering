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

        public int GetAmount(MTGColor c)
        {
            switch (c)
            {
                case MTGColor.White: return whiteAmount;
                case MTGColor.Blue: return blueAmount;
                case MTGColor.Black: return blackAmount;
                case MTGColor.Red: return redAmount;
                case MTGColor.Green: return greenAmount;
                case MTGColor.Colorless: return colorlessAmount;
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

        public void AddAmount(MTGColor c, int amount)
        {
            switch (c)
            {
                case MTGColor.White: whiteAmount += amount; break;
                case MTGColor.Blue: blueAmount += amount; break;
                case MTGColor.Black: blackAmount += amount; break;
                case MTGColor.Red: redAmount += amount; break;
                case MTGColor.Green: greenAmount += amount; break;
                case MTGColor.Colorless: colorlessAmount += amount; break;
                default: throw new ArgumentException("Invalid color for mana add!");
            }
        }

        public bool TrySubtractAmount(MTGColor c, int amount)
        {
            switch (c)
            {
                case MTGColor.White: return TrySubtractAmount(amount, ref whiteAmount);
                case MTGColor.Blue: return TrySubtractAmount(amount, ref blueAmount);
                case MTGColor.Black: return TrySubtractAmount(amount, ref blackAmount);
                case MTGColor.Red: return TrySubtractAmount(amount, ref redAmount);
                case MTGColor.Green: return TrySubtractAmount(amount, ref greenAmount);
                case MTGColor.Colorless: return TrySubtractAmount(amount, ref colorlessAmount);
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
