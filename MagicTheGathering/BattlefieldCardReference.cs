using MagicTheGathering.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class BattlefieldCardReference : CardReference
    {
        internal BattlefieldCardReference(CardReference card, Player controller) : base(card, false)
        {
            if (plainsAbility == null)
                plainsAbility = new ActivatedAbility(new Cost(), new AddToManaPoolAction(controller, ManaAmount.Parse("W")), true, true);
            if (islandAbility == null)
                islandAbility = new ActivatedAbility(new Cost(), new AddToManaPoolAction(controller, ManaAmount.Parse("U")), true, true);
            if (swampAbility == null)
                swampAbility = new ActivatedAbility(new Cost(), new AddToManaPoolAction(controller, ManaAmount.Parse("B")), true, true);
            if (mountainAbility == null)
                mountainAbility = new ActivatedAbility(new Cost(), new AddToManaPoolAction(controller, ManaAmount.Parse("R")), true, true);
            if (forestAbility == null)
                forestAbility = new ActivatedAbility(new Cost(), new AddToManaPoolAction(controller, ManaAmount.Parse("G")), true, true);
            this.controller = controller;
            UpdateAbilities();
        }


        internal Player controller;
        internal bool hasSummoningSickness = true;
        internal bool isTapped = false;
        public int Power
        {
            get
            {
                return card.power;
            }
        }
        public int Toughness
        {
            get
            {
                return card.toughness;
            }
        }
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

        internal void Upkeep()
        {
            hasSummoningSickness = false;
        }

        public override CardLocation Location
        {
            get
            {
                return CardLocation.BattleField;
            }
        }

        static ActivatedAbility forestAbility;
        static ActivatedAbility islandAbility;
        static ActivatedAbility mountainAbility;
        static ActivatedAbility plainsAbility;
        static ActivatedAbility swampAbility;

        protected override void UpdateAbilities(List<AbilityInstance> abilities)
        {
                if (card.IsSubtype("Plains"))
                {
                    abilities.Add(new AbilityInstance(plainsAbility));
                }
                else if (card.IsSubtype("Island"))
                {
                    abilities.Add(new AbilityInstance(islandAbility));
                }
                else if (card.IsSubtype("Swamp"))
                {
                    abilities.Add(new AbilityInstance(swampAbility));
                }
                else if (card.IsSubtype("Mountain"))
                {
                    abilities.Add(new AbilityInstance(mountainAbility));
                }
                else if (card.IsSubtype("Forest"))
                {
                    abilities.Add(new AbilityInstance(forestAbility));
                }
        }
    }
}
