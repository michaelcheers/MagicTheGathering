using MagicTheGathering.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class BattlefieldCardReference : CardReference
    {
        internal BattlefieldCardReference(CardReference card, Player controller) : base(card, false) { this.controller = controller; UpdateAbilities(); }


        internal Player controller;
        internal bool isTapped = false;
        internal bool isUntapped
        {
            get
            {
                return !isTapped;
            }
            set
            {
                isTapped = !value;
            }
        }

        public override CardLocation Location
        {
            get
            {
                return CardLocation.BattleField;
            }
        }

        protected override void UpdateAbilities(List<AbilityInstance> abilities)
        {
            if (card.Type == MagicCardType.Land)
            {
                if (card.IsSubtype("Plains"))
                {
                    abilities.Add(new AbilityInstance(new ActivatedAbility(new Cost(), new AddToManaPoolAction(controller, ManaAmount.Parse("W")), true, true)));
                }
                else if (card.IsSubtype("Island"))
                {
                    abilities.Add(new AbilityInstance(new ActivatedAbility(new Cost(), new AddToManaPoolAction(controller, ManaAmount.Parse("U")), true, true)));
                }
                else if (card.IsSubtype("Swamp"))
                {
                    abilities.Add(new AbilityInstance(new ActivatedAbility(new Cost(), new AddToManaPoolAction(controller, ManaAmount.Parse("B")), true, true)));
                }
                else if (card.IsSubtype("Mountain"))
                {
                    abilities.Add(new AbilityInstance(new ActivatedAbility(new Cost(), new AddToManaPoolAction(controller, ManaAmount.Parse("R")), true, true)));
                }
                else if (card.IsSubtype("Forest"))
                {
                    abilities.Add(new AbilityInstance(new ActivatedAbility(new Cost(), new AddToManaPoolAction(controller, ManaAmount.Parse("G")), true, true)));
                }
            }
        }
    }
}
