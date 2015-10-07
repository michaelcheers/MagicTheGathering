using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class ManaPaymentComponent : CostComponent
    {
        struct ManaPaymentPart
        {
            internal int amount;
            internal MTGColor color;

            internal ManaPaymentPart(MTGColor color, int amount)
            {
                this.color = color;
                this.amount = amount;
            }

            public override string ToString()
            {
                if (color == MTGColor.GenericMana)
                    return amount.ToString();

                string result = "";
                string symbol = "";
                switch(color)
                {
                    case MTGColor.White: symbol = "W"; break;
                    case MTGColor.Blue: symbol = "U"; break;
                    case MTGColor.Black: symbol = "B"; break;
                    case MTGColor.Red: symbol = "R"; break;
                    case MTGColor.Green: symbol = "G"; break;
                }
                for (int i = 0; i < amount; i++)
                {
                    result += symbol;
                }
                return result;
            }

            static internal int NumBits(MTGColor color)
            {
                int result = 0;
                foreach(MTGColor testColor in ManaPool.ManaTypesList)
                {
                    if ((color & testColor) != 0)
                        result++;
                }
                return result;
            }

            static internal int SortByComplexity(ManaPaymentPart a, ManaPaymentPart b)
            {
                return NumBits(a.color).CompareTo(NumBits(b.color));
            }
        }

        List<ManaPaymentPart> parts;

        public override bool IsPayable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ManaPaymentComponent()
        {
            parts = new List<ManaPaymentPart>();
        }
        public ManaPaymentComponent(Dictionary<MTGColor, int> amounts)
        {
            parts = new List<ManaPaymentPart>();
            foreach(KeyValuePair<MTGColor, int> amount in amounts)
            {
                parts.Add(new ManaPaymentPart(amount.Key, amount.Value));
            }
        }

        static void AddParsedAmount(Dictionary<MTGColor, int> amounts, MTGColor color, int amount)
        {
            if (!amounts.ContainsKey(color))
                amounts[color] = amount;
            else
                amounts[color] += amount;
        }

        public static ManaPaymentComponent Parse(string value)
        {
            Dictionary<MTGColor, int> amounts = new Dictionary<MTGColor, int>();
            int genericCost = 0;

            foreach (char c in value)
            {
                switch (char.ToUpper(c))
                {
                    case 'R':
                        {
                            AddParsedAmount(amounts, MTGColor.Red, 1);
                            break;
                        }
                    case 'G':
                        {
                            AddParsedAmount(amounts, MTGColor.Green, 1);
                            break;
                        }
                    case 'U':
                        {
                            AddParsedAmount(amounts, MTGColor.Blue, 1);
                            break;
                        }
                    case 'B':
                        {
                            AddParsedAmount(amounts, MTGColor.Black, 1);
                            break;
                        }
                    case 'W':
                        {
                            AddParsedAmount(amounts, MTGColor.White, 1);
                            break;
                        }
                    case '{':
                    case '}':
                        break;
                    default:
                        {
                            genericCost *= 10;
                            genericCost += int.Parse(c.ToString());

                            break;
                        }
                }
            }

            amounts[MTGColor.GenericMana] = genericCost;
            return new ManaPaymentComponent(amounts);
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            foreach (ManaPaymentPart part in parts)
            {
                output.Append(part.ToString());
            }

            if (output.Length == 0)
                output.Append("0");

            return output.ToString();
        }

        public override Choices GetChoices()
        {
            throw new NotImplementedException();
        }

        public bool CanBePaidWith(ManaPool pool)
        {
            Dictionary<MTGColor, int> manaRemaining = pool.GetManaRemaining();
            return TryTakeMana(manaRemaining);
        }

        public void PayWith(ManaPool pool)
        {
            Dictionary<MTGColor, int> manaRemaining = pool.GetManaRemaining();
            if (!TryTakeMana(manaRemaining))
                throw new NotSupportedException("Can't afford this payment!");

            pool.SetManaRemaining(manaRemaining);
        }

        public bool TryPayWith(ManaPool pool)
        {
            Dictionary<MTGColor, int> manaRemaining = pool.GetManaRemaining();
            bool result = TryTakeMana(manaRemaining);

            if( result )
                pool.SetManaRemaining(manaRemaining);

            return result;
        }

        bool TryTakeMana(Dictionary<MTGColor, int> manaRemaining)
        {
            parts.Sort(ManaPaymentPart.SortByComplexity);

            ManaPaymentPart[] partsCopy = new ManaPaymentPart[parts.Count];
            parts.CopyTo(partsCopy);

            foreach(ManaPaymentPart part in partsCopy)
            {
                int partRemaining = part.amount;
                while (partRemaining > 0)
                {
                    bool didPay = false;
                    foreach (KeyValuePair<MTGColor, int> kv in manaRemaining)
                    {
                        if ((part.color & kv.Key) != 0 && kv.Value > 0)
                        {
                            // this type of mana can pay this cost; assume we don't have hybrid costs, so we can just greedily take it
                            if (kv.Value <= partRemaining)
                            {
                                partRemaining -= kv.Value;
                                manaRemaining.Remove(kv.Key);
                            }
                            else
                            {
                                manaRemaining[kv.Key] -= partRemaining;
                                partRemaining = 0;
                            }
                            didPay = true;
                            break;
                        }
                    }

                    if( !didPay )
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
