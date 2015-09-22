using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DartsKata
{
    public class GameDriver
    {
        private IDartsGame _game;

        public GameDriver(IDartsGame game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));

            this._game = game;
        }

        public void Play()
        {
            while(!this._game.Finished)
            {
                this._game.PlayNextTurn();
            }
        }
    }
}
