using Data;

namespace Model
{
    public class BallService : IBallService
    {
        public void Update(Ball ball, double width, double height)
        {
            ball.X += ball.VelocityX;
            ball.Y += ball.VelocityY;

            if (ball.X - ball.R < 0)
            {
                ball.X = ball.R;
                ball.VelocityX *= -1;
            }

            if (ball.X + ball.R > width)
            {
                ball.X = width - ball.R;
                ball.VelocityX *= -1;
            }

            if (ball.Y - ball.R < 0)
            {
                ball.Y = ball.R;
                ball.VelocityY *= -1;
            }

            if (ball.Y + ball.R > height)
            {
                ball.Y = height - ball.R;
                ball.VelocityY *= -1;
            }
        }
    }
}