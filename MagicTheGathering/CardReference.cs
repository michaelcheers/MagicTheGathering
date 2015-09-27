using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    internal abstract class CardReference
    {
        internal class CardID { }

        internal enum CardLocation
        {
            BattleField,
            Hand,
            Deck
        }
        public readonly MagicCard card; // for example, Island. All Islands have the same MagicCard.
        internal readonly CardID cardID; // identifies a specific instance of a card.

        internal MagicCard Card
        {
            get
            {
                return card;
            }
        }

        List<AbilityInstance> UpdateAbilities
        {
            get
            {
                if (card.Type == MagicCardType.Land)
                {
                    if (card.IsSubtype("Forest"))
                    {

                    }
                }
                return new List<AbilityInstance>();
            }
        }

        public List<AbilityInstance> Abilities;



        public abstract CardLocation Location
        {
            get;
        }

        internal CardReference (MagicCard card, CardID id)
        {
            this.card = card;
            this.cardID = id;
        }

        internal CardReference(CardReference cardRef)
        {
            this.card = cardRef.card;
            this.cardID = cardRef.cardID;
        }
    }
}
