using MagicTheGathering.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class BattlefieldCardReference : CardReference
    {
        internal BattlefieldCardReference(CardReference card, Player controller) : base(card)
        {
        }

        Player controller;
        bool isTapped;

        public override CardLocation Location
        {
            get
            {
                return CardLocation.BattleField;
            }
        }

        protected override void UpdateAbilites(List<AbilityInstance> abilities)
        {
            if (card.Type == MagicCardType.Land)
            {
                if (card.IsSubtype("Forest"))
                {
                    abilities.Add(new AbilityInstance(new AddToManaPoolAction(controller, ManaAmount.Parse("G"))));
                }
            }
        }
    }
}
