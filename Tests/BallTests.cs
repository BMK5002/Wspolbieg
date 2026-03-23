using Data;

namespace BallTests {
    public class BallTests
    {
        [Fact]
        public void BallValuesTest()
        {
            var ball = new Ball
            {
                X = 10,
                Y = 20,
                VelocityX = 1.5,
                VelocityY = -2.5
            };

            Assert.Equal(10, ball.X);
            Assert.Equal(20, ball.Y);
            Assert.Equal(1.5, ball.VelocityX);
            Assert.Equal(-2.5, ball.VelocityY);
        }

        [Fact]
        public void BallSetterTest()
        {
            var ball = new Ball();

            ball.X = 5;
            ball.Y = 6;

            ball.X = 15;
            ball.Y = 16;

            Assert.Equal(15, ball.X);
            Assert.Equal(16, ball.Y);
        }
    }
}