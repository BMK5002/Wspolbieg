using Data;
using Model;

namespace BallServiceTests
{
    public class BallServiceTests
    {
        [Fact]
        public void ballMoveTest()
        {
            var service = new BallService();
            var ball = new Ball { X = 10, Y = 10, VelocityX = 1, VelocityY = 1, R = 5 };

            service.Update(new[] { ball }, 100, 100);

            Assert.Equal(11, ball.X);
            Assert.Equal(11, ball.Y);
        }

        [Fact]
        public void ballDoesNotLeaveLeftOrTop()
        {
            var service = new BallService();

            var ball = new Ball { X = 1, Y = 1, VelocityX = -5, VelocityY = -3, R = 5 };

            service.Update(new[] { ball }, 100, 100);

            Assert.Equal(5, ball.X);
            Assert.Equal(5, ball.Y);
            Assert.Equal(5, ball.VelocityX);
            Assert.Equal(3, ball.VelocityY);
        }

        [Fact]
        public void ballDoesNotLeaveRightOrBottom()
        {
            var service = new BallService();
            var ball = new Ball { X = 95, Y = 96, VelocityX = 10, VelocityY = 10, R = 5 };

            service.Update(new[] { ball }, 100, 100);

            Assert.Equal(95, ball.X);
            Assert.Equal(95, ball.Y);
            Assert.Equal(-10, ball.VelocityX);
            Assert.Equal(-10, ball.VelocityY);
        }

        [Fact]
        public void createBallsTest()
        {
            var service = new BallService();
            int count = 25;
            var balls = service.CreateBalls(count).ToList();

            Assert.Equal(count, balls.Count);
            foreach (var ball in balls)
            {
                Assert.True(ball.R > 0);
                Assert.True(ball.X >= 0);
                Assert.True(ball.Y >= 0);

                Assert.True(ball.VelocityX != 0 || ball.VelocityY != 0);
            }
        }
    }
}