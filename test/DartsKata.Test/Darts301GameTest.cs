using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DartsKata.Test
{    
    public class Darts301GameTest
    {        
        [Fact]
        public void ANewGame_RequiresAtLeastOnePlayer()
        {            
            Assert.Throws<ArgumentException>(() => new Darts301Game());
        }

        [Fact]
        public void ANewGame_InitializesEveryPlayer()
        {
            var mockPlayerA = new Mock<IDartsPlayer>();
            var mockPlayerB = new Mock<IDartsPlayer>();
            var game = new Darts301Game(mockPlayerA.Object, mockPlayerB.Object);

            mockPlayerA.Verify(p => p.Initialize(It.Is<int>(a => a == 301)), Times.Once);
            mockPlayerB.Verify(p => p.Initialize(It.Is<int>(a => a == 301)), Times.Once);
        }

        [Fact]
        public void IsNotFinished_WhenPlayersHavePointsRemaining()
        {
            var mockPlayerA = new Mock<IDartsPlayer>();
            mockPlayerA.SetupGet(p => p.RemainingPoints).Returns(1);
            var mockPlayerB = new Mock<IDartsPlayer>();
            mockPlayerB.SetupGet(p => p.RemainingPoints).Returns(301);
            var game = new Darts301Game(mockPlayerA.Object, mockPlayerB.Object);

            Assert.False(game.Finished);
        }

        [Fact]
        public void IsFinished_WhenAPlayerHasNoRemainingPoints()
        {
            var mockPlayerA = new Mock<IDartsPlayer>();
            mockPlayerA.SetupGet(p => p.RemainingPoints).Returns(1);
            var mockPlayerB = new Mock<IDartsPlayer>();
            mockPlayerB.SetupGet(p => p.RemainingPoints).Returns(0);
            var game = new Darts301Game(mockPlayerA.Object, mockPlayerB.Object);

            Assert.True(game.Finished);
        }

        [Fact]
        public void Winner_WhenGameIsNotFinished_ThrowsException()
        {
            var mockPlayerA = new Mock<IDartsPlayer>();
            mockPlayerA.SetupGet(p => p.RemainingPoints).Returns(1);
            var mockPlayerB = new Mock<IDartsPlayer>();
            mockPlayerB.SetupGet(p => p.RemainingPoints).Returns(301);
            var game = new Darts301Game(mockPlayerA.Object, mockPlayerB.Object);

            Assert.Throws<InvalidOperationException>(() => game.Winner);
        }

        [Fact]
        public void Winner_WhenGameIsFinished_ReturnsPlayerWithZeroRemainingPoints()
        {
            var mockPlayerA = new Mock<IDartsPlayer>();
            mockPlayerA.SetupGet(p => p.RemainingPoints).Returns(1);
            var mockPlayerB = new Mock<IDartsPlayer>();
            mockPlayerB.SetupGet(p => p.RemainingPoints).Returns(0);
            var game = new Darts301Game(mockPlayerA.Object, mockPlayerB.Object);

            Assert.Equal(mockPlayerB.Object, game.Winner);
        }

        [Fact]
        public void ANewGame_StartsOnTurnOne()
        {
            var game = new Darts301Game(new Mock<IDartsPlayer>().Object);

            Assert.Equal(1, game.TurnNumber);
        }

        [Fact]
        public void ANewGame_StartsByFirstPlayer()
        {
            var mockPlayerA = new Mock<IDartsPlayer>();
            var mockPlayerB = new Mock<IDartsPlayer>();            
            var game = new Darts301Game(mockPlayerA.Object, mockPlayerB.Object);

            Assert.Equal(mockPlayerA.Object, game.NextTurnPlayer);
        }

        [Fact]
        public void PlayNextTurn_IncreasesTheTurnIndex()
        {
            var mockPlayer = new Mock<IDartsPlayer>();
            mockPlayer.SetupGet(p => p.RemainingPoints).Returns(5);
            var game = new Darts301Game(mockPlayer.Object);
            game.PlayNextTurn();            

            Assert.Equal(2, game.TurnNumber);
        }

        [Fact]
        public void PlayNextTurn_WhenGameIsFinished_ThrowsException()
        {
            var mockPlayer = new Mock<IDartsPlayer>();
            mockPlayer.SetupGet(p => p.RemainingPoints).Returns(0);
            var game = new Darts301Game(mockPlayer.Object);

            Assert.Throws<InvalidOperationException>(() => game.PlayNextTurn());            
        }

        [Fact]
        public void PlayNextTurn_StartsByFirstPlayerAndMovesToSecond()
        {
            var mockPlayerA = new Mock<IDartsPlayer>();
            mockPlayerA.SetupGet(p => p.RemainingPoints).Returns(100);
            var mockPlayerB = new Mock<IDartsPlayer>();
            mockPlayerB.SetupGet(p => p.RemainingPoints).Returns(100);
            var game = new Darts301Game(mockPlayerA.Object, mockPlayerB.Object);
            
            game.PlayNextTurn();
            var nextPlayer = game.NextTurnPlayer;

            Assert.Equal(mockPlayerB.Object, nextPlayer);
            mockPlayerA.Verify(p => p.PlayTurn(), Times.Once);
        }

        [Fact]
        public void PlayNextTurn_CyclesEveryPlayerInOrder()
        {
            var mockPlayerA = new Mock<IDartsPlayer>();
            mockPlayerA.SetupGet(p => p.RemainingPoints).Returns(100);
            var mockPlayerB = new Mock<IDartsPlayer>();
            mockPlayerB.SetupGet(p => p.RemainingPoints).Returns(100);
            var game = new Darts301Game(mockPlayerA.Object, mockPlayerB.Object);

            game.PlayNextTurn();
            mockPlayerA.Verify(p => p.PlayTurn(), Times.Once, "First player should play first");
            
            game.PlayNextTurn();            
            mockPlayerB.Verify(p => p.PlayTurn(), Times.Once, "Second player should play next");
            Assert.Equal(mockPlayerA.Object, game.NextTurnPlayer);
        }

        [Fact]
        public void PlayNextTurn_WillGetBackToFirstPlayer_AfterAllPlayersPlayed()
        {
            var mockPlayerA = new Mock<IDartsPlayer>();
            mockPlayerA.SetupGet(p => p.RemainingPoints).Returns(100);
            var mockPlayerB = new Mock<IDartsPlayer>();
            mockPlayerB.SetupGet(p => p.RemainingPoints).Returns(100);
            var game = new Darts301Game(mockPlayerA.Object, mockPlayerB.Object);

            game.PlayNextTurn();
            mockPlayerA.Verify(p => p.PlayTurn(), Times.Once, "First player should play first");

            game.PlayNextTurn();
            mockPlayerB.Verify(p => p.PlayTurn(), Times.Once, "Second player should play next");

            game.PlayNextTurn();
            mockPlayerA.Verify(p => p.PlayTurn(), Times.Exactly(2), "First player should play again");
        }
    }
}
