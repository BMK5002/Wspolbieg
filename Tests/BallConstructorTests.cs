using Data;

namespace PhysicsTests
{
    /// <summary>
    /// Tests for the Ball constructor with radius-based initialization.
    /// Verifies correct initialization of position, radius, and velocity.
    /// </summary>
    public class BallConstructorTests
    {
        private const double Epsilon = 1e-9;

        [Fact]
        public void Constructor_DefaultValues_ParameterlessConstructor()
        {
            // Act
            var ball = new Ball();

            // Assert
            Assert.Equal(0, ball.X);
            Assert.Equal(0, ball.Y);
            Assert.Equal(0, ball.VelocityX);
            Assert.Equal(0, ball.VelocityY);
            Assert.Equal(0, ball.R);
        }

        [Fact]
        public void Constructor_WithRadius_InitializesWithVelocity()
        {
            // Arrange
            double radius = 5.0;

            // Act
            var ball = new Ball(radius);

            // Assert
            Assert.Equal(radius, ball.R);
            Assert.Equal(0, ball.X);
            Assert.Equal(0, ball.Y);
            // With angle = 0, VelocityX should be non-zero and VelocityY should be zero
            Assert.NotEqual(0, ball.VelocityX);
            Assert.Equal(0, ball.VelocityY);
        }

        [Fact]
        public void Constructor_WithRadiusAndPosition_InitializesCorrectly()
        {
            // Arrange
            double radius = 5.0;
            double x = 50.0;
            double y = 75.0;

            // Act
            var ball = new Ball(radius, x, y);

            // Assert
            Assert.Equal(radius, ball.R);
            Assert.Equal(x, ball.X);
            Assert.Equal(y, ball.Y);
        }

        [Fact]
        public void Constructor_WithAllParameters_InitializesCorrectly()
        {
            // Arrange
            double radius = 5.0;
            double x = 50.0;
            double y = 75.0;
            double angle = Math.PI / 4;

            // Act
            var ball = new Ball(radius, x, y, angle);

            // Assert
            Assert.Equal(radius, ball.R);
            Assert.Equal(x, ball.X);
            Assert.Equal(y, ball.Y);

            // Verify velocity is correct for the given angle
            double expectedSpeed = Ball.InitialMomentum / (Math.PI * radius * radius);
            double expectedVx = expectedSpeed * Math.Cos(angle);
            double expectedVy = expectedSpeed * Math.Sin(angle);

            Assert.Equal(expectedVx, ball.VelocityX, precision: 9);
            Assert.Equal(expectedVy, ball.VelocityY, precision: 9);
        }

        [Fact]
        public void Constructor_AngleZero_VelocityTowardRight()
        {
            // Arrange
            double radius = 5.0;

            // Act
            var ball = new Ball(radius, velocityAngle: 0);

            // Assert
            Assert.True(ball.VelocityX > 0);
            Assert.Equal(0, ball.VelocityY, precision: 9);
        }

        [Fact]
        public void Constructor_AnglePiOver2_VelocityDownward()
        {
            // Arrange
            double radius = 5.0;

            // Act
            var ball = new Ball(radius, velocityAngle: Math.PI / 2);

            // Assert
            Assert.Equal(0, ball.VelocityX, precision: 9);
            Assert.True(ball.VelocityY > 0);
        }

        [Fact]
        public void Constructor_MassConsistency_MassMatchesRadius()
        {
            // Arrange
            double radius = 5.0;

            // Act
            var ball = new Ball(radius);

            // Assert
            double expectedMass = Math.PI * radius * radius;
            Assert.Equal(expectedMass, ball.Mass, precision: 10);
        }

        [Fact]
        public void Constructor_MultipleInstances_IndependentState()
        {
            // Arrange & Act
            var ball1 = new Ball(5.0, 10, 20, Math.PI / 6);
            var ball2 = new Ball(8.0, 30, 40, Math.PI / 3);

            // Assert
            Assert.NotEqual(ball1.R, ball2.R);
            Assert.NotEqual(ball1.X, ball2.X);
            Assert.NotEqual(ball1.Y, ball2.Y);
            Assert.NotEqual(ball1.VelocityX, ball2.VelocityX);
            Assert.NotEqual(ball1.Mass, ball2.Mass);
        }

        [Fact]
        public void Constructor_VerySmallRadius_ValidInitialization()
        {
            // Arrange
            double radius = 0.1;

            // Act
            var ball = new Ball(radius);

            // Assert
            Assert.Equal(radius, ball.R);
            double expectedMass = Math.PI * radius * radius;
            Assert.Equal(expectedMass, ball.Mass, precision: 10);
        }

        [Fact]
        public void Constructor_LargeRadius_ValidInitialization()
        {
            // Arrange
            double radius = 50.0;

            // Act
            var ball = new Ball(radius);

            // Assert
            Assert.Equal(radius, ball.R);
            double expectedMass = Math.PI * radius * radius;
            Assert.Equal(expectedMass, ball.Mass, precision: 8);
        }

        [Fact]
        public void Constructor_PositionBoundaries_PositionsSet()
        {
            // Arrange
            double x = 0;
            double y = 0;

            // Act
            var ball = new Ball(5.0, x, y);

            // Assert
            Assert.Equal(x, ball.X);
            Assert.Equal(y, ball.Y);
        }

        [Fact]
        public void Constructor_NegativePosition_ValidInitialization()
        {
            // Arrange
            double x = -50.0;
            double y = -75.0;

            // Act
            var ball = new Ball(5.0, x, y);

            // Assert
            Assert.Equal(x, ball.X);
            Assert.Equal(y, ball.Y);
        }
    }
}
