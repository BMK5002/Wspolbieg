using Data;

namespace Model
{
    public class BallService : IBallService
    {
        public void Update(Ball ball, double width, double height)
        {
            ball.X += ball.VelocityX;
            ball.Y += ball.VelocityY;

            if (ball.X < 0)
            {
                ball.X = 0;
                ball.VelocityX *= -1;
            }
            else if (ball.X + 2 * ball.R > width)
            {
                ball.X = width - 2 * ball.R;
                ball.VelocityX *= -1;
            }

            if (ball.Y < 0)
            {
                ball.Y = 0;
                ball.VelocityY *= -1;
            }
            else if (ball.Y + 2 * ball.R > height)
            {
                ball.Y = height - 2 * ball.R;
                ball.VelocityY *= -1;
            }
        }
    }
}