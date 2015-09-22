using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DartsKata
{
    public class Program
    {
        public void Main(string[] args)
        {
            Console.WriteLine("How many players?");
            int playersCount;
            if (!int.TryParse(Console.ReadLine(), out playersCount)) playersCount = 2;

            Console.WriteLine("Starting fame for " + playersCount + " players");
            var players = new List<IDartsPlayer>();
            var dartsThrower = new RandomDartsThrower();
            for (int i=1; i <= playersCount; i++)
            {
                players.Add(new DartsPlayer(dartsThrower) {Name = $"Player_{i}" });
            }

            var gameDriver = new GameDriver(
                                new ConsoleLoggingGame(
                                    new Darts301Game(players.ToArray())));
            gameDriver.Play();
            Console.ReadLine();
        }  
    }

    public class RandomDartsThrower : IDartsThrower
    {
        private Random randomGenerator = new Random();

        public int ThrowDart()
        {
            var number = randomGenerator.Next(0, 21);
            if (number == 21) number = 25;

            var multiplier = randomGenerator.Next(1, 3);
            if (number == 25 && multiplier == 3) multiplier = 2;

            var dartValue = number * multiplier;
            Console.WriteLine(dartValue);
            return dartValue;
        }
    }    

    public class ConsoleLoggingGame : IDartsGame
    {
        private IDartsGame game;
        public ConsoleLoggingGame(IDartsGame game)
        {
            this.game = game;
        }

        public bool Finished
        {
            get
            {
                if (this.game.Finished) Console.WriteLine("Game finished, winner is " + this.game.Winner.Name);
                return this.game.Finished;
            }
        }

        public IDartsPlayer NextTurnPlayer
        {
            get { return this.game.NextTurnPlayer; }
        }

        public IDartsPlayer[] Players
        {
            get { return this.game.Players; }
        }

        public int TurnNumber
        {
            get { return this.game.TurnNumber; }
        }

        public IDartsPlayer Winner
        {
            get { return this.game.Winner; }
        }

        public void PlayNextTurn()
        {            
            Console.WriteLine("Playing turn of " + this.game.NextTurnPlayer.Name + ". Points remaining: " + this.game.NextTurnPlayer.RemainingPoints);
            this.game.PlayNextTurn();
        }
    }
}
