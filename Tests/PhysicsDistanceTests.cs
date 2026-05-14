using Data;
using Model;

namespace PhysicsTests
{
    /// <summary>
    /// Tests for Physics helper methods: distance calculation and collision detection.
    /// </summary>
    public class PhysicsDistanceTests
    {
        private const double Epsilon = 1e-9;

        [Fact]
        public void GetDistance_BallsAtSameLocation_ZeroDistance()
        {
            // Arrange
            var ball1 = new Ball { X = 50, Y = 50, R = 5 };
            var ball2 = new Ball { X = 50, Y = 50, R = 5 };

            // Act
            double distance = Physics.GetDistance(ball1, ball2);

            // Assert
            Assert.Equal(0, distance, precision: 10);
        }

        [Fact]
        public void GetDistance_HorizontalAlignment_CorrectDistance()
        {
            // Arrange
            var ball1 = new Ball { X = 0, Y = 0, R = 5 };
            var ball2 = new Ball { X = 10, Y = 0, R = 5 };

            // Act
            double distance = Physics.GetDistance(ball1, ball2);

            // Assert
            Assert.Equal(10, distance, precision: 10);
        }

        [Fact]
        public void GetDistance_VerticalAlignment_CorrectDistance()
        {
            // Arrange
            var ball1 = new Ball { X = 0, Y = 0, R = 5 };
            var ball2 = new Ball { X = 0, Y = 15, R = 5 };

            // Act
            double distance = Physics.GetDistance(ball1, ball2);

            // Assert
            Assert.Equal(15, distance, precision: 10);
        }

        [Fact]
        public void GetDistance_DiagonalPositions_PythagoreanDistance()
        {
            // Arrange
            var ball1 = new Ball { X = 0, Y = 0, R = 5 };
            var ball2 = new Ball { X = 3, Y = 4, R = 5 };
            double expectedDistance = 5;  // 3-4-5 triangle

            // Act
            double distance = Physics.GetDistance(ball1, ball2);

            // Assert
            Assert.Equal(expectedDistance, distance, precision: 10);
        }

        [Fact]
        public void GetDistance_NegativeCoordinates_CorrectDistance()
        {
            // Arrange
            var ball1 = new Ball { X = -10, Y = -10, R = 5 };
            var ball2 = new Ball { X = 10, Y = 10, R = 5 };
            double expectedDistance = Math.Sqrt(20 * 20 + 20 * 20);

            // Act
            double distance = Physics.GetDistance(ball1, ball2);

            // Assert
            Assert.Equal(expectedDistance, distance, precision: 10);
        }

        [Fact]
        public void GetDistance_IgnoresRadius_OnlyUsesPosition()
        {
            // Arrange
            var ball1 = new Ball { X = 0, Y = 0, R = 5 };
            var ball2 = new Ball { X = 10, Y = 0, R = 20 };

            // Act: Distance should only use positions, not radii
            double distance = Physics.GetDistance(ball1, ball2);

            // Assert
            Assert.Equal(10, distance, precision: 10);
        }

        [Fact]
        public void GetDistance_Commutative_SameResultBothDirections()
        {
            // Arrange
            var ball1 = new Ball { X = 5, Y = 7, R = 5 };
            var ball2 = new Ball { X = 15, Y = 22, R = 5 };

            // Act
            double distance1 = Physics.GetDistance(ball1, ball2);
            double distance2 = Physics.GetDistance(ball2, ball1);

            // Assert
            Assert.Equal(distance1, distance2, precision: 10);
        }

        [Fact]
        public void GetDistance_LargeValues_HighPrecision()
        {
            // Arrange
            var ball1 = new Ball { X = 1000, Y = 1000, R = 5 };
            var ball2 = new Ball { X = 1030, Y = 1040, R = 5 };
            double expectedDistance = Math.Sqrt(30 * 30 + 40 * 40);

            // Act
            double distance = Physics.GetDistance(ball1, ball2);

            // Assert
            Assert.Equal(expectedDistance, distance, precision: 9);
        }

        [Fact]
        public void GetDistance_SmallValues_HighPrecision()
        {
            // Arrange
            var ball1 = new Ball { X = 0.001, Y = 0.001, R = 5 };
            var ball2 = new Ball { X = 0.004, Y = 0.005, R = 5 };
            double expectedDistance = Math.Sqrt(0.003 * 0.003 + 0.004 * 0.004);

            // Act
            double distance = Physics.GetDistance(ball1, ball2);

            // Assert
            Assert.Equal(expectedDistance, distance, precision: 12);
        }
    }
}
