using Data;

namespace Model
{
    public class BallService : IBallService
    {
        public void Update(Ball ball, double width, double height)
        {
            ball.X += ball.VelocityX;
            ball.Y += ball.VelocityY;

            if (ball.X < 0 || ball.X > width)
                ball.VelocityX *= -1;

            if (ball.Y < 0 || ball.Y > height)
                ball.VelocityY *= -1;
        }
    }
}