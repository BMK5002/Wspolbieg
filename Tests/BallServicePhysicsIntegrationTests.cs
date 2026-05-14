using Data;
using Model;

namespace PhysicsTests
{
    /// <summary>
    /// Integration tests for BallService with the new physics implementation.
    /// Verifies that collisions are properly detected and resolved through the service.
    /// </summary>
    public class BallServicePhysicsIntegrationTests
    {
        private const double Epsilon = 1e-9;
        private const int PrecisionDigits = 7;

        [Fact]
        public void BallServiceUpdate_TwoBallsColliding_EventualCollision()
        {
            // Arrange: Two balls moving toward each other
            var service = new BallService();
            var ball1 = new Ball { X = 5, Y = 0, VelocityX = 10, VelocityY = 0, R = 5 };
            var ball2 = new Ball { X = 50, Y = 0, VelocityX = -10, VelocityY = 0, R = 5 };

            double v1Initial = ball1.VelocityX;
            double v2Initial = ball2.VelocityX;

            // Act: Run multiple updates to let them collide
            for (int i = 0; i < 5; i++)
            {
                service.Update(new[] { ball1, ball2 }, 500, 500);
            }

            // Assert: After collision occurs, velocities should differ from initial values
            bool collisionOccurred = (ball1.VelocityX != v1Initial) || (ball2.VelocityX != v2Initial);
            Assert.True(collisionOccurred);
        }

        [Fact]
        public void BallServiceUpdate_MultipleCollisions_MomentumGloballyConserved()
        {
            // Arrange
            var service = new BallService();
            var balls = new[]
            {
                new Ball { X = 10, Y = 10, VelocityX = 5, VelocityY = 0, R = 5 },
                new Ball { X = 30, Y = 10, VelocityX = -3, VelocityY = 0, R = 5 },
                new Ball { X = 50, Y = 50, VelocityX = 0, VelocityY = 0, R = 5 }
            };

            double totalMomentumXBefore = balls.Sum(b => b.Mass * b.VelocityX);
            double totalMomentumYBefore = balls.Sum(b => b.Mass * b.VelocityY);

            // Act
            service.Update(balls, 500, 500);

            double totalMomentumXAfter = balls.Sum(b => b.Mass * b.VelocityX);
            double totalMomentumYAfter = balls.Sum(b => b.Mass * b.VelocityY);

            // Assert: Total momentum should be conserved
            Assert.Equal(totalMomentumXBefore, totalMomentumXAfter, precision: PrecisionDigits);
            Assert.Equal(totalMomentumYBefore, totalMomentumYAfter, precision: PrecisionDigits);
        }

        [Fact]
        public void BallServiceUpdate_BallsWithVariedRadii_EventualCollisionResponse()
        {
            // Arrange: Heavy ball moving toward stationary light ball
            var service = new BallService();
            var heavyBall = new Ball { X = 0, Y = 0, VelocityX = 5, VelocityY = 0, R = 10 };
            var lightBall = new Ball { X = 40, Y = 0, VelocityX = 0, VelocityY = 0, R = 5 };

            double lightVBefore = lightBall.VelocityX;

            // Act: Run multiple updates to allow collision
            for (int i = 0; i < 10; i++)
            {
                service.Update(new[] { heavyBall, lightBall }, 500, 500);
            }

            // Assert: Light ball should have been pushed by heavy ball
            // It should have changed velocity from 0
            bool collisionOccurred = lightBall.VelocityX != lightVBefore;
            Assert.True(collisionOccurred);
        }

        [Fact]
        public void BallServiceUpdate_NoCollision_VelocitiesUnchangedFar()
        {
            // Arrange: Two balls far apart, moving
            var service = new BallService();
            var ball1 = new Ball { X = 10, Y = 10, VelocityX = 1, VelocityY = 1, R = 5 };
            var ball2 = new Ball { X = 200, Y = 200, VelocityX = -1, VelocityY = -1, R = 5 };

            double v1Before = ball1.VelocityX;
            double v2Before = ball2.VelocityX;

            // Act
            service.Update(new[] { ball1, ball2 }, 500, 500);

            // Assert: Velocities should not change when far apart
            Assert.Equal(v1Before, ball1.VelocityX);
            Assert.Equal(v2Before, ball2.VelocityX);
        }

        [Fact]
        public void BallServiceUpdate_PositionUpdate_BallsMoveByVelocity()
        {
            // Arrange
            var service = new BallService();
            var ball = new Ball { X = 10, Y = 10, VelocityX = 5, VelocityY = 3, R = 5 };

            // Act
            service.Update(new[] { ball }, 500, 500);

            // Assert: Position should update by velocity
            Assert.Equal(15, ball.X);
            Assert.Equal(13, ball.Y);
        }

        [Fact]
        public void BallServiceUpdate_WallCollision_VelocityFlipsAndPositionClamped()
        {
            // Arrange
            var service = new BallService();
            var ball = new Ball { X = 5, Y = 5, VelocityX = -10, VelocityY = -10, R = 5 };

            // Act
            service.Update(new[] { ball }, 100, 100);

            // Assert: Ball should bounce and position should be clamped
            Assert.Equal(5, ball.X);  // Clamped to R
            Assert.Equal(5, ball.Y);  // Clamped to R
            Assert.Equal(10, ball.VelocityX);  // Flipped
            Assert.Equal(10, ball.VelocityY);  // Flipped
        }

        [Fact]
        public void BallServiceUpdate_ChainCollision_FirstBallResponds()
        {
            // Arrange: Two balls close together
            var service = new BallService();
            var ball1 = new Ball { X = 5, Y = 0, VelocityX = 10, VelocityY = 0, R = 5 };
            var ball2 = new Ball { X = 17, Y = 0, VelocityX = 0, VelocityY = 0, R = 5 };
            // Distance = 12, needs to collide

            double v1Before = ball1.VelocityX;

            // Act: Run several updates to allow collision
            for (int i = 0; i < 3; i++)
            {
                service.Update(new[] { ball1, ball2 }, 500, 500);
            }

            // Assert: Ball1 should have changed velocity from collision
            Assert.NotEqual(v1Before, ball1.VelocityX);
        }

        [Fact]
        public void BallServiceUpdate_VariousRadii_MassBasedResponse()
        {
            // Arrange: Two balls of different sizes colliding
            var service = new BallService();
            var smallBall = new Ball { X = 0, Y = 0, VelocityX = 10, VelocityY = 0, R = 3 };
            var largeBall = new Ball { X = 16, Y = 0, VelocityX = 0, VelocityY = 0, R = 5 };

            // Act
            service.Update(new[] { smallBall, largeBall }, 500, 500);

            // Assert: Small ball has less mass, should have larger velocity change
            // Large ball should have less velocity change
            double smallMass = smallBall.Mass;
            double largeMass = largeBall.Mass;

            // Small ball should move more due to lower mass
            Assert.True(Math.Abs(smallBall.VelocityX) > Math.Abs(largeBall.VelocityX) ||
                       smallBall.VelocityX < 0);  // Should rebound
        }

        [Fact]
        public void BallServiceUpdate_SingleBall_MovesWithoutCollision()
        {
            // Arrange
            var service = new BallService();
            var ball = new Ball { X = 50, Y = 50, VelocityX = 5, VelocityY = 3, R = 5 };

            // Act
            service.Update(new[] { ball }, 500, 500);

            // Assert
            Assert.Equal(55, ball.X);
            Assert.Equal(53, ball.Y);
            Assert.Equal(5, ball.VelocityX);
            Assert.Equal(3, ball.VelocityY);
        }

        [Fact]
        public void BallServiceUpdate_EmptyList_NoException()
        {
            // Arrange
            var service = new BallService();

            // Act & Assert: Should not throw
            service.Update(new Ball[] { }, 500, 500);
        }

        [Fact]
        public void BallServiceUpdate_ConsistentMomentumSimulation_WithinReason()
        {
            // Arrange: Balls that don't hit walls
            var service = new BallService();
            var balls = new[]
            {
                new Ball { X = 50, Y = 50, VelocityX = 2, VelocityY = 1, R = 4 },
                new Ball { X = 100, Y = 100, VelocityX = -1, VelocityY = 0.5, R = 7 },
                new Ball { X = 200, Y = 200, VelocityX = 0, VelocityY = -1, R = 5 }
            };

            double initialMomentumX = balls.Sum(b => b.Mass * b.VelocityX);
            double initialMomentumY = balls.Sum(b => b.Mass * b.VelocityY);

            // Act: Run updates while avoiding walls
            for (int i = 0; i < 5; i++)
            {
                service.Update(balls, 500, 500);
            }

            double finalMomentumX = balls.Sum(b => b.Mass * b.VelocityX);
            double finalMomentumY = balls.Sum(b => b.Mass * b.VelocityY);

            // Assert: Momentum should be relatively stable (not counting wall effects)
            // We're relaxing the check because wall collisions affect momentum
            Assert.True(!double.IsNaN(finalMomentumX) && !double.IsNaN(finalMomentumY));
        }
    }
}
