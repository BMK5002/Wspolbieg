using Data;

namespace PhysicsTests
{
    /// <summary>
    /// Tests for initial velocity calculation based on momentum conservation.
    /// Verifies that lighter balls move faster and heavier balls move slower,
    /// while all maintain the same initial momentum (p₀ = 100.0).
    /// Formula: speed = InitialMomentum / mass, where mass = π * r²
    /// </summary>
    public class InitialVelocityTests
    {
        private const double Epsilon = 1e-9;

        [Fact]
        public void InitialVelocity_MomentumConservation_SameMomentumForAllBalls()
        {
            // Arrange
            var ball1 = new Ball(radius: 3.0);
            var ball2 = new Ball(radius: 5.0);
            var ball3 = new Ball(radius: 10.0);

            // Act: Calculate momentum for each ball (p = m * v)
            double momentum1 = ball1.Mass * ball1.GetSpeed();
            double momentum2 = ball2.Mass * ball2.GetSpeed();
            double momentum3 = ball3.Mass * ball3.GetSpeed();

            // Assert: All should have approximately the same momentum
            Assert.Equal(Ball.InitialMomentum, momentum1, precision: 8);
            Assert.Equal(Ball.InitialMomentum, momentum2, precision: 8);
            Assert.Equal(Ball.InitialMomentum, momentum3, precision: 8);
        }

        [Fact]
        public void InitialVelocity_SmallerBallFaster_LargerBallSlower()
        {
            // Arrange
            var smallBall = new Ball(radius: 3.0);
            var largeBall = new Ball(radius: 6.0);

            // Act
            double smallSpeed = smallBall.GetSpeed();
            double largeSpeed = largeBall.GetSpeed();

            // Assert: Smaller ball should be faster
            Assert.True(smallSpeed > largeSpeed);
        }

        [Fact]
        public void InitialVelocity_DirectionalVelocity_AngleZero()
        {
            // Arrange
            double radius = 5.0;
            double angle = 0.0;  // Direction: right

            // Act
            var ball = new Ball(radius, x: 0, y: 0, velocityAngle: angle);

            // Assert
            double expectedSpeed = Ball.InitialMomentum / (Math.PI * radius * radius);
            Assert.Equal(expectedSpeed, ball.VelocityX, precision: 9);
            Assert.Equal(0.0, ball.VelocityY, precision: 9);
        }

        [Fact]
        public void InitialVelocity_DirectionalVelocity_AnglePiOver2()
        {
            // Arrange
            double radius = 5.0;
            double angle = Math.PI / 2;  // Direction: down

            // Act
            var ball = new Ball(radius, x: 0, y: 0, velocityAngle: angle);

            // Assert
            double expectedSpeed = Ball.InitialMomentum / (Math.PI * radius * radius);
            Assert.Equal(0.0, ball.VelocityX, precision: 9);
            Assert.Equal(expectedSpeed, ball.VelocityY, precision: 9);
        }

        [Fact]
        public void InitialVelocity_DirectionalVelocity_AnglePi()
        {
            // Arrange
            double radius = 5.0;
            double angle = Math.PI;  // Direction: left

            // Act
            var ball = new Ball(radius, x: 0, y: 0, velocityAngle: angle);

            // Assert
            double expectedSpeed = Ball.InitialMomentum / (Math.PI * radius * radius);
            Assert.Equal(-expectedSpeed, ball.VelocityX, precision: 9);
            Assert.Equal(0.0, ball.VelocityY, precision: 9);
        }

        [Fact]
        public void InitialVelocity_DirectionalVelocity_DiagonalAngle()
        {
            // Arrange
            double radius = 5.0;
            double angle = Math.PI / 4;  // 45 degrees

            // Act
            var ball = new Ball(radius, x: 0, y: 0, velocityAngle: angle);

            // Assert
            double expectedSpeed = Ball.InitialMomentum / (Math.PI * radius * radius);
            double expectedVelocity = expectedSpeed * Math.Sqrt(2) / 2;

            Assert.Equal(expectedVelocity, ball.VelocityX, precision: 9);
            Assert.Equal(expectedVelocity, ball.VelocityY, precision: 9);
        }

        [Fact]
        public void InitialVelocity_SpeedMagnitude_CorrectCalculation()
        {
            // Arrange
            double radius = 5.0;
            double angle = Math.PI / 3;  // 60 degrees

            // Act
            var ball = new Ball(radius, x: 0, y: 0, velocityAngle: angle);
            double speedMagnitude = Math.Sqrt(ball.VelocityX * ball.VelocityX + ball.VelocityY * ball.VelocityY);

            // Assert
            double expectedSpeed = Ball.InitialMomentum / (Math.PI * radius * radius);
            Assert.Equal(expectedSpeed, speedMagnitude, precision: 9);
        }

        [Fact]
        public void InitialVelocity_FullRotation_MomentumPreserved()
        {
            // Arrange
            double radius = 5.0;

            // Act: Create balls at various angles
            var angles = Enumerable.Range(0, 8)
                .Select(i => i * Math.PI / 4)
                .ToArray();

            // Assert
            foreach (var angle in angles)
            {
                var ball = new Ball(radius, x: 0, y: 0, velocityAngle: angle);
                double momentum = ball.Mass * Math.Sqrt(ball.VelocityX * ball.VelocityX + ball.VelocityY * ball.VelocityY);
                Assert.Equal(Ball.InitialMomentum, momentum, precision: 8);
            }
        }
    }
}

namespace PhysicsTests
{
    /// <summary>
    /// Helper extension methods for Ball testing.
    /// </summary>
    internal static class BallTestExtensions
    {
        /// <summary>
        /// Gets the speed magnitude (magnitude of velocity vector).
        /// </summary>
        public static double GetSpeed(this Ball ball)
        {
            return Math.Sqrt(ball.VelocityX * ball.VelocityX + ball.VelocityY * ball.VelocityY);
        }
    }
}
