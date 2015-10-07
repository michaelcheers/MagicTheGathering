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
                case MTGColor.ColorlessMana: return colorlessAmount;
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

        public void SetAmount(MTGColor c, int amount)
        {
            switch (c)
            {
                case MTGColor.White: whiteAmount = amount; break;
                case MTGColor.Blue: blueAmount = amount; break;
                case MTGColor.Black: blackAmount = amount; break;
                case MTGColor.Red: redAmount = amount; break;
                case MTGColor.Green: greenAmount = amount; break;
                case MTGColor.ColorlessMana: colorlessAmount = amount; break;
                default: throw new ArgumentException("Invalid color for mana set amount!");
            }
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
                case MTGColor.ColorlessMana: colorlessAmount += amount; break;
                default: throw new ArgumentException("Invalid color for mana add!");
            }
        }

        public bool ContainsAtLeast(ManaAmount amount)
        {
            return (whiteAmount >= amount.whiteAmount &&
                blueAmount >= amount.blueAmount &&
                blackAmount >= amount.blackAmount &&
                redAmount >= amount.redAmount &&
                greenAmount >= amount.greenAmount &&
                colorlessAmount >= amount.colorlessAmount);
        }

        public bool TrySubtract(ManaAmount amount)
        {
            if (!ContainsAtLeast(amount))
                return false;

            whiteAmount -= amount.whiteAmount;
            blueAmount -= amount.blueAmount;
            blackAmount -= amount.blackAmount;
            redAmount -= amount.redAmount;
            greenAmount -= amount.greenAmount;
            colorlessAmount -= amount.colorlessAmount;
            return true;
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
                case MTGColor.ColorlessMana: return TrySubtractAmount(amount, ref colorlessAmount);
                default: throw new ArgumentException("Invalid color for mana subtract!");
            }
        }

        public static ManaAmount Parse(string value)
        {
            int redAmount = 0;
            int blueAmount = 0;
            int greenAmount = 0;
            int blackAmount = 0;
            int whiteAmount = 0;
            int colorlessAmount = 0;

            foreach (char c in value)
            {
                switch (char.ToUpper(c))
                {
                    case 'R':
                        {
                            redAmount++;
                            break;
                        }
                    case 'G':
                        {
                            greenAmount++;
                            break;
                        }
                    case 'U':
                        {
                            blueAmount++;
                            break;
                        }
                    case 'B':
                        {
                            blackAmount++;
                            break;
                        }
                    case 'W':
                        {
                            whiteAmount++;
                            break;
                        }
                    case '{':
                    case '}':
                        break;
                    default:
                        {
                            colorlessAmount *= 10;
                            colorlessAmount += int.Parse(c.ToString());

                            break;
                        }
                }
            }
            return new ManaAmount(redAmount, blueAmount, greenAmount, blackAmount, whiteAmount, colorlessAmount);
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
        public static ManaAmount operator + (ManaAmount i1, ManaAmount i2)
        {
            i1 = (ManaAmount)(i1.MemberwiseClone());
            i1.Add(i2);
            ManaAmount result = i1;
            return i1;
        }
    }
}
