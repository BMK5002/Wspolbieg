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
            var Ball = new Ball { X = 10, Y = 10, VelocityX = 1, VelocityY = 1 };

            service.Update(Ball, 100, 100);

            Assert.Equal(11, Ball.X);
            Assert.Equal(11, Ball.Y);
        }
    }
}