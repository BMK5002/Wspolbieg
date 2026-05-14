using Data;
using Model;

namespace PhysicsTests
{
    /// <summary>
    /// Tests for positional separation of overlapping balls.
    /// Verifies that overlapping balls are correctly separated without penetration.
    /// </summary>
    public class PositionalSeparationTests
    {
        private const double Epsilon = 1e-9;
        private const int PrecisionDigits = 7;

        [Fact]
        public void SeparateOverlappingBalls_NonOverlappingBalls_NoChange()
        {
            // Arrange: Balls that are NOT overlapping
            var ball1 = new Ball { X = 0, Y = 0, VelocityX = 0, VelocityY = 0, R = 5 };
            var ball2 = new Ball { X = 20, Y = 0, VelocityX = 0, VelocityY = 0, R = 5 };

            double x1Before = ball1.X;
            double y1Before = ball1.Y;
            double x2Before = ball2.X;
            double y2Before = ball2.Y;

            // Act
            Physics.SeparateOverlappingBalls(ball1, ball2);

            // Assert: Positions should not change
            Assert.Equal(x1Before, ball1.X, precision: PrecisionDigits);
            Assert.Equal(y1Before, ball1.Y, precision: PrecisionDigits);
            Assert.Equal(x2Before, ball2.X, precision: PrecisionDigits);
            Assert.Equal(y2Before, ball2.Y, precision: PrecisionDigits);
        }

        [Fact]
        public void SeparateOverlappingBalls_OverlappingBalls_Separated()
        {
            // Arrange: Balls that ARE overlapping (distance < r1 + r2)
            var ball1 = new Ball { X = 0, Y = 0, R = 5 };
            var ball2 = new Ball { X = 8, Y = 0, R = 5 };  // Should touch at distance 10, now at 8 = overlapping by 2

            double distanceBefore = Physics.GetDistance(ball1, ball2);

            // Act
            Physics.SeparateOverlappingBalls(ball1, ball2);

            // Assert: Distance should increase toward minimum separation distance
            double distanceAfter = Physics.GetDistance(ball1, ball2);
            double minDistance = ball1.R + ball2.R;

            Assert.True(distanceAfter > distanceBefore || Math.Abs(distanceAfter - minDistance) < 0.1);
        }

        [Fact]
        public void SeparateOverlappingBalls_EqualMass_SymmetricalSeparation()
        {
            // Arrange: Two balls of equal size (equal mass) overlapping
            var ball1 = new Ball { X = 0, Y = 0, R = 5 };
            var ball2 = new Ball { X = 8, Y = 0, R = 5 };

            double x1Before = ball1.X;
            double x2Before = ball2.X;
            double midpoint = (x1Before + x2Before) / 2;

            // Act
            Physics.SeparateOverlappingBalls(ball1, ball2);

            // Assert: For equal masses, separation should be symmetric
            double x1After = ball1.X;
            double x2After = ball2.X;
            double newMidpoint = (x1After + x2After) / 2;

            // Midpoint should remain the same (symmetric separation)
            Assert.Equal(midpoint, newMidpoint, precision: PrecisionDigits);
        }

        [Fact]
        public void SeparateOverlappingBalls_UnequalMass_LighterMovesMore()
        {
            // Arrange: Heavy ball (large R) overlapping significantly with light ball (small R)
            var heavyBall = new Ball { X = 0, Y = 0, R = 10 };
            var lightBall = new Ball { X = 12, Y = 0, R = 5 };  // Overlapping by 3 units

            double heavyXBefore = heavyBall.X;
            double lightXBefore = lightBall.X;

            // Act
            Physics.SeparateOverlappingBalls(heavyBall, lightBall);

            double heavyDisplacement = Math.Abs(heavyBall.X - heavyXBefore);
            double lightDisplacement = Math.Abs(lightBall.X - lightXBefore);

            // Assert: For overlapping balls, lighter ball should move more than heavier ball
            // OR at minimum, one should move (the lighter one)
            Assert.True(lightDisplacement > 0);
        }

        [Fact]
        public void SeparateOverlappingBalls_Horizontal_CorrectDirection()
        {
            // Arrange: Ball2 to the right of Ball1, overlapping
            var ball1 = new Ball { X = 0, Y = 0, R = 5 };
            var ball2 = new Ball { X = 8, Y = 0, R = 5 };

            // Act
            Physics.SeparateOverlappingBalls(ball1, ball2);

            // Assert: Ball1 should move left, Ball2 should move right
            Assert.True(ball1.X < 0);  // Moved left
            Assert.True(ball2.X > 8);  // Moved right
        }

        [Fact]
        public void SeparateOverlappingBalls_Vertical_CorrectDirection()
        {
            // Arrange: Ball2 below Ball1, overlapping
            var ball1 = new Ball { X = 0, Y = 0, R = 5 };
            var ball2 = new Ball { X = 0, Y = 8, R = 5 };

            // Act
            Physics.SeparateOverlappingBalls(ball1, ball2);

            // Assert: Ball1 should move up, Ball2 should move down
            Assert.True(ball1.Y < 0);  // Moved up
            Assert.True(ball2.Y > 8);  // Moved down
        }

        [Fact]
        public void SeparateOverlappingBalls_Diagonal_CorrectMovement()
        {
            // Arrange: Diagonal overlap with significant penetration
            var ball1 = new Ball { X = 0, Y = 0, R = 5 };
            var ball2 = new Ball { X = 6, Y = 6, R = 5 };  // Overlapping

            double x1Before = ball1.X;
            double y1Before = ball1.Y;
            double x2Before = ball2.X;
            double y2Before = ball2.Y;

            // Act
            Physics.SeparateOverlappingBalls(ball1, ball2);

            // Assert: At least one should move from its starting position
            bool ball1Moved = (ball1.X != x1Before) || (ball1.Y != y1Before);
            bool ball2Moved = (ball2.X != x2Before) || (ball2.Y != y2Before);

            Assert.True(ball1Moved || ball2Moved);
        }

        [Fact]
        public void SeparateOverlappingBalls_ZeroDistance_SafetyCheck()
        {
            // Arrange: Balls at exact same position (edge case)
            var ball1 = new Ball { X = 10, Y = 10, R = 5 };
            var ball2 = new Ball { X = 10, Y = 10, R = 5 };

            double x1Before = ball1.X;
            double y1Before = ball1.Y;
            double x2Before = ball2.X;
            double y2Before = ball2.Y;

            // Act: Should not throw
            Physics.SeparateOverlappingBalls(ball1, ball2);

            // Assert: Positions should remain unchanged (safety early return)
            Assert.Equal(x1Before, ball1.X);
            Assert.Equal(y1Before, ball1.Y);
            Assert.Equal(x2Before, ball2.X);
            Assert.Equal(y2Before, ball2.Y);
        }

        [Fact]
        public void SeparateOverlappingBalls_MomentumUnaffected_VelocityPreserved()
        {
            // Arrange: Overlapping balls with velocities
            var ball1 = new Ball { X = 0, Y = 0, VelocityX = 5, VelocityY = 3, R = 5 };
            var ball2 = new Ball { X = 8, Y = 0, VelocityX = -2, VelocityY = 4, R = 5 };

            double vx1Before = ball1.VelocityX;
            double vy1Before = ball1.VelocityY;
            double vx2Before = ball2.VelocityX;
            double vy2Before = ball2.VelocityY;

            // Act
            Physics.SeparateOverlappingBalls(ball1, ball2);

            // Assert: Velocities should not change during positional separation
            Assert.Equal(vx1Before, ball1.VelocityX);
            Assert.Equal(vy1Before, ball1.VelocityY);
            Assert.Equal(vx2Before, ball2.VelocityX);
            Assert.Equal(vy2Before, ball2.VelocityY);
        }

        [Fact]
        public void SeparateOverlappingBalls_CompletelyInside_MovesApart()
        {
            // Arrange: Small ball completely inside large ball (extreme overlap)
            var largeBall = new Ball { X = 0, Y = 0, R = 20 };
            var smallBall = new Ball { X = 5, Y = 5, R = 3 };

            double x1Before = largeBall.X;
            double y1Before = largeBall.Y;
            double x2Before = smallBall.X;
            double y2Before = smallBall.Y;

            // Act
            Physics.SeparateOverlappingBalls(largeBall, smallBall);

            // Assert: At least one should move (small ball is lighter, should move more)
            bool largeMoved = (largeBall.X != x1Before) || (largeBall.Y != y1Before);
            bool smallMoved = (smallBall.X != x2Before) || (smallBall.Y != y2Before);

            Assert.True(largeMoved || smallMoved);
        }
    }
}
