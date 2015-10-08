using System.Collections.Generic;
using System;

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
        CardReference old;

        internal MagicCard Card
        {
            get
            {
                return card;
            }
        }

        protected abstract void UpdateAbilities(List<AbilityInstance> abilities);

        public void UpdateAbilities ()
        {
            List<AbilityInstance> result = new List<AbilityInstance>();
            UpdateAbilities(result);
            Abilities = result;
        }

        public List<AbilityInstance> Abilities;

        public bool IsCreature { get { return (card.Type & MagicCardType.Creature) != 0; } }
        public bool IsLand { get { return (card.Type & MagicCardType.Land) != 0; } }

        public abstract CardLocation Location
        {
            get;
        }

        internal CardReference (MagicCard card, CardID id)
        {
            this.card = card;
            this.cardID = id;
            UpdateAbilities();
        }

        internal CardReference(CardReference cardRef, bool runUpdateAbilities = true)
        {
            this.card = cardRef.card;
            this.cardID = cardRef.cardID;
            this.old = cardRef;
            if (runUpdateAbilities)
            UpdateAbilities();
        }
    }
}
