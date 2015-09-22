using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DartsKata
{
    public interface IDartsGame
    {        
        IDartsPlayer[] Players { get; }
        int TurnNumber { get; }
        IDartsPlayer NextTurnPlayer { get; }
        IDartsPlayer Winner { get; }
        bool Finished { get; }
        void PlayNextTurn();
    }
}
