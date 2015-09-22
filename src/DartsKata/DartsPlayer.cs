using System;

namespace DartsKata
{
    public class DartsPlayer : IDartsPlayer
    {
        private int _remainingPoints;
        private IDartsThrower _dartsThrower;

        public DartsPlayer(IDartsThrower dartsThrower)
        {
            if (dartsThrower == null) throw new ArgumentNullException(nameof(dartsThrower));

            this._dartsThrower = dartsThrower;
        }

        public string Name { get; set; }

        public int RemainingPoints
        {
            get
            {
                return this._remainingPoints;
            }
        }

        public void Initialize(int startingPoints)
        {
            this._remainingPoints = startingPoints;
        }

        public void PlayTurn()
        {
            var previousRemainingPoints = this._remainingPoints;
            
            for(int i = 0; i < 3; i++)
            {
                this._remainingPoints -= this._dartsThrower.ThrowDart();                
            }
            
            if (this._remainingPoints < 0) this._remainingPoints = previousRemainingPoints;
        }
    }
}