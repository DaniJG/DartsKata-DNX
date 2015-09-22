using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DartsKata.Test
{
    public class IntegrationTest
    {
        [Fact]
        public void ADarts301GameCanBePlayed()
        {
            var playerAMockThrower = new Mock<IDartsThrower>();
            playerAMockThrower.SetupSequence(dt => dt.ThrowDart()).Returns(60).Returns(60).Returns(60).Returns(60).Returns(60).Returns(1);
            var playerBMockThrower = new Mock<IDartsThrower>();
            playerBMockThrower.Setup(dt => dt.ThrowDart()).Returns(20);

            var game = new Darts301Game(
                            new DartsPlayer(playerAMockThrower.Object),
                            new DartsPlayer(playerBMockThrower.Object));
            var gameDriver = new GameDriver(game);
            gameDriver.Play();

            Assert.True(game.Finished);
            Assert.Equal(game.Winner, game.Players.First());
        }
    }
}
