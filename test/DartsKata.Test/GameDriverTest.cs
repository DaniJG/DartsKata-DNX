using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DartsKata.Test
{
    public class GameDriverTest
    {
        [Fact]
        public void RequiresANotNullGame()
        {
            Assert.Throws<ArgumentNullException>(() => new GameDriver(null));
        }

        [Fact]
        public void Play_WontPlayIfAlreadyFinished()
        {
            var game = new Mock<IDartsGame>();
            game.Setup(g => g.Finished).Returns(true);

            var gameDriver = new GameDriver(game.Object);
            gameDriver.Play();

            game.Verify(g => g.PlayNextTurn(), Times.Never);
        }

        [Fact]
        public void Play_KeepsPlayingTurnUntilGameIsFinished()
        {
            var game = new Mock<IDartsGame>();
            game.SetupSequence(g => g.Finished)
                .Returns(false)
                .Returns(false)
                .Returns(true);

            var gameDriver = new GameDriver(game.Object);
            gameDriver.Play();

            game.Verify(g => g.PlayNextTurn(), Times.Exactly(2));
        }
    }
}
