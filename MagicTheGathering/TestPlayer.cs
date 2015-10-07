using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    class TestPlayer : Player
    {

        public TestPlayer(MagicGame game, Deck deck) : base(game, deck)
        {
            this.game = game;
            this.deck = deck;
        }

        protected override void GameStart()
        {
            List<HandCardReference> handClone = new List<HandCardReference>((HandCardReference[])Hand.ToArray().Clone());
            foreach (var item in handClone)
            {
                if (!(item.Card.Type.HasFlag(MagicCardType.Land)))
                    Play(item);
            }
        }
    }
}
