using Data;
using Xunit;

public class BallTests
{
    [Fact]
    public void BallValuesTest()
    {
        var Ball = new Ball
        {
            X = 10,
            Y = 20,
            VelocityX = 1.5,
            VelocityY = -2.5
        };

        Assert.Equal(10, Ball.X);
        Assert.Equal(20, Ball.Y);
        Assert.Equal(1.5, Ball.VelocityX);
        Assert.Equal(-2.5, Ball.VelocityY);
    }

    [Fact]
    public void BallSetterTest()
    {
        var Ball = new Ball();

        Ball.X = 5;
        Ball.Y = 6;

        Ball.X = 15;
        Ball.Y = 16;

        Assert.Equal(15, Ball.X);
        Assert.Equal(16, Ball.Y);
    }
}