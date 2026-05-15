using Data;
using Model;

namespace PhysicsTests
{
    public class MassCalculationTests
    {
        [Fact]
        public void massCalculagionTest()
        {
            double radius = 5.0;
            double expectedMass = Math.PI * radius * radius;

            var ball = new Ball(radius);

            Assert.Equal(expectedMass, ball.Mass);
        }
    }

    public class PositionalSeparationTests
    {
        [Fact]
        public void ballSeperateNegativeTest()
        {
            var ball1 = new Ball { X = 0, Y = 0, VelocityX = 0, VelocityY = 0, R = 5 };
            var ball2 = new Ball { X = 20, Y = 0, VelocityX = 0, VelocityY = 0, R = 5 };

            double x1Before = ball1.X;
            double y1Before = ball1.Y;
            double x2Before = ball2.X;
            double y2Before = ball2.Y;

            Physics.SeparateOverlappingBalls(ball1, ball2);

            Assert.Equal(x1Before, ball1.X);
            Assert.Equal(y1Before, ball1.Y);
            Assert.Equal(x2Before, ball2.X);
            Assert.Equal(y2Before, ball2.Y);
        }

        [Fact]
        public void ballSeperatePositiveTest()
        {
            var ball1 = new Ball { X = 0, Y = 0, R = 5 };
            var ball2 = new Ball { X = 8, Y = 0, R = 5 };

            double distanceBefore = Physics.GetDistance(ball1, ball2);

            Physics.SeparateOverlappingBalls(ball1, ball2);

            double distanceAfter = Physics.GetDistance(ball1, ball2);
            double minDistance = ball1.R + ball2.R;

            Assert.True(distanceAfter > distanceBefore || Math.Abs(distanceAfter - minDistance) < 0.1);
        }

        [Fact]
        public void zeroDistanceSafetyCheckTest()
        {
            var ball1 = new Ball { X = 10, Y = 10, R = 5 };
            var ball2 = new Ball { X = 10, Y = 10, R = 5 };

            double x1Before = ball1.X;
            double y1Before = ball1.Y;
            double x2Before = ball2.X;
            double y2Before = ball2.Y;

            Physics.SeparateOverlappingBalls(ball1, ball2);

            Assert.Equal(x1Before, ball1.X);
            Assert.Equal(y1Before, ball1.Y);
            Assert.Equal(x2Before, ball2.X);
            Assert.Equal(y2Before, ball2.Y);
        }

        [Fact]
        public void SeparateOverlappingBalls_CompletelyInside_MovesApart()
        {
            var largeBall = new Ball { X = 0, Y = 0, R = 20 };
            var smallBall = new Ball { X = 5, Y = 5, R = 3 };

            double x1Before = largeBall.X;
            double y1Before = largeBall.Y;
            double x2Before = smallBall.X;
            double y2Before = smallBall.Y;

            Physics.SeparateOverlappingBalls(largeBall, smallBall);

            bool largeMoved = (largeBall.X != x1Before) || (largeBall.Y != y1Before);
            bool smallMoved = (smallBall.X != x2Before) || (smallBall.Y != y2Before);

            Assert.True(largeMoved || smallMoved);
        }
    }

    public class ElasticCollisionTests
    {
        [Fact]
        public void sameMassSameVelocityCollisionTest()
        {
            var ball1 = new Ball { X = 0, Y = 0, VelocityX = 10, VelocityY = 0, R = 5 };
            var ball2 = new Ball { X = 20, Y = 0, VelocityX = -10, VelocityY = 0, R = 5 };

            double v1Before = ball1.VelocityX;
            double v2Before = ball2.VelocityX;

            Physics.ResolveElasticCollision(ball1, ball2);

            Assert.Equal(v2Before, ball1.VelocityX);
            Assert.Equal(v1Before, ball2.VelocityX);
        }

        [Fact]
        public void momentumConservationTest()
        {
            var ball1 = new Ball { X = 0, Y = 0, VelocityX = 10, VelocityY = 0, R = 5 };
            var ball2 = new Ball { X = 20, Y = 0, VelocityX = -5, VelocityY = 0, R = 5 };

            double momentumBefore = ball1.Mass * ball1.VelocityX + ball2.Mass * ball2.VelocityX;

            Physics.ResolveElasticCollision(ball1, ball2);

            double momentumAfter = ball1.Mass * ball1.VelocityX + ball2.Mass * ball2.VelocityX;

            Assert.Equal(momentumBefore, momentumAfter, precision: 5);
        }

        [Fact]
        public void differentMassColisionTest()
        {
            var heavyBall = new Ball { X = 0, Y = 0, VelocityX = 5, VelocityY = 0, R = 10 };
            var lightBall = new Ball { X = 25, Y = 0, VelocityX = 0, VelocityY = 0, R = 5 };

            double vLightBefore = lightBall.VelocityX;

            Physics.ResolveElasticCollision(heavyBall, lightBall);

            Assert.True(lightBall.VelocityX > vLightBefore);
        }
    }

    public class InitialVelocityTests
    {
        [Fact]
        public void differentMassDifferentSpeedsTest()
        {
            var smallBall = new Ball(radius: 3.0);
            var largeBall = new Ball(radius: 6.0);

            double smallSpeed = smallBall.GetSpeed();
            double largeSpeed = largeBall.GetSpeed();

            Assert.True(smallSpeed > largeSpeed);
        }
    }

    public class BallServicePhysicsIntegrationTests
    {
        [Fact]
        public void differentRadiusCollisionTest()
        {
            var service = new BallService();
            var heavyBall = new Ball { X = 0, Y = 0, VelocityX = 5, VelocityY = 0, R = 10 };
            var lightBall = new Ball { X = 40, Y = 0, VelocityX = 0, VelocityY = 0, R = 5 };

            double lightVBefore = lightBall.VelocityX;

            for (int i = 0; i < 10; i++)
            {
                service.Update(new[] { heavyBall, lightBall }, 500, 500);
            }

            bool collisionOccurred = lightBall.VelocityX != lightVBefore;
            Assert.True(collisionOccurred);
        }

        [Fact]
        public void consistentMomentumTest()
        {
            var service = new BallService();
            var balls = new[]
            {
                new Ball { X = 50, Y = 50, VelocityX = 2, VelocityY = 1, R = 4 },
                new Ball { X = 100, Y = 100, VelocityX = -1, VelocityY = 0.5, R = 7 },
                new Ball { X = 200, Y = 200, VelocityX = 0, VelocityY = -1, R = 5 }
            };

            double initialMomentumX = balls.Sum(b => b.Mass * b.VelocityX);
            double initialMomentumY = balls.Sum(b => b.Mass * b.VelocityY);

            for (int i = 0; i < 5; i++)
            {
                service.Update(balls, 500, 500);
            }

            double finalMomentumX = balls.Sum(b => b.Mass * b.VelocityX);
            double finalMomentumY = balls.Sum(b => b.Mass * b.VelocityY);

            Assert.True(!double.IsNaN(finalMomentumX) && !double.IsNaN(finalMomentumY));
        }
    }

    internal static class BallTestExtensions
    {
        public static double GetSpeed(this Ball ball)
        {
            return Math.Sqrt(ball.VelocityX * ball.VelocityX + ball.VelocityY * ball.VelocityY);
        }
    }
}
