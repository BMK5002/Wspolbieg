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

            service.Update(new[] { ball }, 100, 100, 1);

            Assert.Equal(11, ball.X);
            Assert.Equal(11, ball.Y);
        }

        [Fact]
        public void ballDoesNotLeaveLeftOrTopTest()
        {
            var service = new BallService();

            var ball = new Ball { X = 1, Y = 1, VelocityX = -5, VelocityY = -3, R = 5 };

            service.Update(new[] { ball }, 100, 100, 1);

            Assert.Equal(5, ball.X);
            Assert.Equal(5, ball.Y);
            Assert.Equal(5, ball.VelocityX);
            Assert.Equal(3, ball.VelocityY);
        }

        [Fact]
        public void ballDoesNotLeaveRightOrBottomTest()
        {
            var service = new BallService();
            var ball = new Ball { X = 95, Y = 96, VelocityX = 10, VelocityY = 10, R = 5 };

            service.Update(new[] { ball }, 100, 100, 1);

            Assert.Equal(95, ball.X);
            Assert.Equal(95, ball.Y);
            Assert.Equal(-10, ball.VelocityX);
            Assert.Equal(-10, ball.VelocityY);
        }

        [Fact]
        public void deltaTimeTest()
        {
            var service = new BallService();

            var ball = new Ball
            {
                X = 0,
                Y = 0,
                VelocityX = 10,
                VelocityY = 5,
                R = 5
            };

            service.UpdateBallPosition(ball, 2.0);

            Assert.Equal(20, ball.X);
            Assert.Equal(10, ball.Y);
        }
    }
}