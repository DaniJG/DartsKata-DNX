using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DartsKata
{
    public class Darts301Game : IDartsGame
    {
        private const int TargetScore = 301;
        private int _turnIndex;
        private IDartsPlayer[] _players;

        public Darts301Game(params IDartsPlayer[] players)
        {
            if (players.Length < 1) throw new ArgumentException("A game requires at least one player", nameof(players));

            this._turnIndex = 0;
            this._players = players;
            foreach(var player in this._players)
            {
                player.Initialize(TargetScore);
            }
        }

        public bool Finished
        {
            get { return this._players.Any(p => p.RemainingPoints == 0); }
        }
        public IDartsPlayer Winner
        {
            get
            {
                if (!this.Finished) throw new InvalidOperationException("Game is not finished yet!");
                return this.Players.First(p => p.RemainingPoints == 0);
            }
        }
        public IDartsPlayer[] Players
        {
            get { return this._players; }
        }

        public int TurnNumber
        {
            get { return this._turnIndex + 1; }
        }

        public IDartsPlayer NextTurnPlayer
        {
            get
            {
                var playerIndex = this._turnIndex % this._players.Length;
                return this._players[playerIndex];
            }
        }

        public void PlayNextTurn()
        {
            if (this.Finished) throw new InvalidOperationException("Game is already finished!");
            
            this.NextTurnPlayer.PlayTurn();
            this._turnIndex++;            
        }
    }
}
