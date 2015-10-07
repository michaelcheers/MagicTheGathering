using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class ManaPaymentComponent : CostComponent
    {
        private int redCost;
        internal int blueCost;
        internal int greenCost;
        internal int blackCost;
        internal int whiteCost;
        internal int colorlessCost;

        public override bool IsPayable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ManaPaymentComponent(int redCost, int blueCost, int greenCost, int blackCost, int whiteCost, int colorlessCost)
        {
            this.redCost = redCost;
            this.blueCost = blueCost;
            this.greenCost = greenCost;
            this.blackCost = blackCost;
            this.whiteCost = whiteCost;
            this.colorlessCost = colorlessCost;
        }

        public static ManaPaymentComponent Parse(string value)
        {
            int redCost = 0;
            int blueCost = 0;
            int greenCost = 0;
            int blackCost = 0;
            int whiteCost = 0;
            int colorlessCost = 0;

            foreach (char c in value)
            {
                switch (char.ToUpper(c))
                {
                    case 'R':
                        {
                            redCost++;
                            break;
                        }
                    case 'G':
                        {
                            greenCost++;
                            break;
                        }
                    case 'U':
                        {
                            blueCost++;
                            break;
                        }
                    case 'B':
                        {
                            blackCost++;
                            break;
                        }
                    case 'W':
                        {
                            whiteCost++;
                            break;
                        }
                    case '{':
                    case '}':
                        break;
                    default:
                        {
                            colorlessCost *= 10;
                            colorlessCost += int.Parse(c.ToString());

                            break;
                        }
                }
            }

            return new ManaPaymentComponent(redCost, blueCost, greenCost, blackCost, whiteCost, colorlessCost);
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            if (colorlessCost > 0)
                output.Append(colorlessCost);
            for (int i = 0; i < redCost; i++)
            {
                output.Append("R");
            }
            for (int i = 0; i < greenCost; i++)
            {
                output.Append("G");
            }
            for (int i = 0; i < blueCost; i++)
            {
                output.Append("U");
            }
            for (int i = 0; i < blackCost; i++)
            {
                output.Append("B");
            }
            for (int i = 0; i < whiteCost; i++)
            {
                output.Append("W");
            }
            if (output.Length == 0)
                output.Append("0");
            return output.ToString();
        }

        public override Choices GetChoices()
        {
            throw new NotImplementedException();
        }
    }
}
