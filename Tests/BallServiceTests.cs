using Data;
using Model;

namespace BallServiceTests
{
    public class BallServiceTests
    {
        [Fact]
        public void BallMoveTest()
        {
            var service = new BallService();
            var ball = new Ball { X = 10, Y = 10, VelocityX = 1, VelocityY = 1, R = 5 };

            service.Update(ball, 100, 100);

            Assert.Equal(11, ball.X);
            Assert.Equal(11, ball.Y);
        }

        [Fact]
        public void BallDoesNotLeaveLeftOrTop()
        {
            var service = new BallService();
            
            var ball = new Ball { X = 1, Y = 1, VelocityX = -5, VelocityY = -3, R = 5 };

            service.Update(ball, 100, 100);

            Assert.Equal(0, ball.X);
            Assert.Equal(0, ball.Y);
            Assert.Equal(5, ball.VelocityX);
            Assert.Equal(3, ball.VelocityY);
        }

        [Fact]
        public void BallDoesNotLeaveRightOrBottom()
        {
            var service = new BallService();
            var ball = new Ball { X = 95, Y = 96, VelocityX = 10, VelocityY = 10, R = 5 };

            service.Update(ball, 100, 100);

            Assert.Equal(100 - 2 * ball.R, ball.X);
            Assert.Equal(100 - 2 * ball.R, ball.Y);
            Assert.Equal(-10, ball.VelocityX);
            Assert.Equal(-10, ball.VelocityY);
        }
    }
}