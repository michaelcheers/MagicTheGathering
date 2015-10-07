using System;
using System.Collections.Generic;

namespace MagicTheGathering
{
    public class MagicGame
    {
        List<Player> players = new List<Player>();
        public event Action StartGame;
        internal void Add (Player player)
        {
            players.Add(player);
        }
        public void ReadyGameForStart ()
        {
            if (StartGame != null)
            {
                StartGame();
            }
        }
        public void Update ()
        {
            foreach (var item in players)
            {
                item.Update();
            }
        }
        public void Draw ()
        {
            foreach (var item in players)
            {
                item.Draw();
            }
        }
    }
}