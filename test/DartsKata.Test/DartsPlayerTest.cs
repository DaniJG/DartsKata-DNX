using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DartsKata.Test
{
    public class DartsPlayerTest
    {
        [Fact]
        public void RequiresANotNullDartthrower()
        {
            Assert.Throws<ArgumentNullException>(() => new DartsPlayer(null));
        }

        [Fact]
        public void Initialize_SetsRemainingPoints()
        {
            var player = new DartsPlayer(new Mock<IDartsThrower>().Object);
            player.Initialize(42);

            Assert.Equal(42, player.RemainingPoints);
        }

        [Fact]
        public void PlayTurn_ThrowsThreeDarts()
        {
            var dartThrower = new Mock<IDartsThrower>();
            var player = new DartsPlayer(dartThrower.Object);

            player.PlayTurn();

            dartThrower.Verify(d => d.ThrowDart(), Times.Exactly(3));
        }

        [Fact]
        public void PlayTurn_DecreasesRemainingPoints()
        {
            var dartThrower = new Mock<IDartsThrower>();
            dartThrower.SetupSequence(d => d.ThrowDart())
                .Returns(10)
                .Returns(20)
                .Returns(60);
            var player = new DartsPlayer(dartThrower.Object);
            player.Initialize(100);

            player.PlayTurn();

            Assert.Equal(10, player.RemainingPoints);
        }

        [Fact]
        public void PlayTurn_DecreasesRemainingPointsUpToZero()
        {
            var dartThrower = new Mock<IDartsThrower>();
            dartThrower.SetupSequence(d => d.ThrowDart())
                .Returns(5)
                .Returns(10)
                .Returns(15);
            var player = new DartsPlayer(dartThrower.Object);
            player.Initialize(30);

            player.PlayTurn();

            Assert.Equal(0, player.RemainingPoints);
        }

        [Fact]
        public void PlayTurn_WillNotDecreasePointsIfPointsGetsLessThatZero()
        {
            var dartThrower = new Mock<IDartsThrower>();
            dartThrower.SetupSequence(d => d.ThrowDart())
                .Returns(10)
                .Returns(20)
                .Returns(60);
            var player = new DartsPlayer(dartThrower.Object);
            player.Initialize(89);

            player.PlayTurn();

            Assert.Equal(89, player.RemainingPoints);
        }
    }
}
