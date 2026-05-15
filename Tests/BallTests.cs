using Data;

namespace PhysicsTests
{
    public class BallTests
    {
        [Fact]
        public void ballConstructorWithAllParametersTest()
        {
            double radius = 5.0;
            double x = 10.0;
            double y = 20.0;
            double angle = Math.PI / 4;

            var ball = new Ball(radius, x, y, angle);

            Assert.Equal(radius, ball.R);
            Assert.Equal(x, ball.X);
            Assert.Equal(y, ball.Y);

            double expectedMass = Math.PI * radius * radius;
            double expectedSpeed = Ball.InitialMomentum / expectedMass;
            double expectedVx = expectedSpeed * Math.Cos(angle);
            double expectedVy = expectedSpeed * Math.Sin(angle);

            Assert.Equal(expectedVx, ball.VelocityX);
            Assert.Equal(expectedVy, ball.VelocityY);
            Assert.Equal(expectedMass, ball.Mass);
        }

        [Fact]
        public void ballConstructorWithoutParametersTest()
        {
            var ball = new Ball();

            Assert.Equal(0, ball.X);
            Assert.Equal(0, ball.Y);
            Assert.Equal(0, ball.R);
            Assert.Equal(0, ball.VelocityX);
            Assert.Equal(0, ball.VelocityY);
            Assert.Equal(0, ball.Mass);
        }

        [Fact]
        public void ballSettersTest()
        {
            var ball = new Ball();

            ball.X = 15.5;
            ball.Y = 25.5;
            ball.R = 7.5;
            ball.VelocityX = 3.5;
            ball.VelocityY = 4.5;

            Assert.Equal(15.5, ball.X);
            Assert.Equal(25.5, ball.Y);
            Assert.Equal(7.5, ball.R);
            Assert.Equal(3.5, ball.VelocityX);
            Assert.Equal(4.5, ball.VelocityY);

            double expectedMass = Math.PI * 7.5 * 7.5;
            Assert.Equal(expectedMass, ball.Mass);
        }
    }
}
