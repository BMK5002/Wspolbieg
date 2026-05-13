using Data;

namespace Model
{
    public class BallService : IBallService
    {

        public IBall Create(double x, double y, double r, double velocityX, double velocityY)
        {
            return new Ball
            {
                X = x,
                Y = y,
                R = r,
                VelocityX = velocityX,
                VelocityY = velocityY
            };
        }
        public void Update( IEnumerable<IBall> balls, double width, double height)
        {
            foreach (var ball in balls)
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

            var list = balls.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    var a = list[i];
                    var b = list[j];

                    double dist = distance(a, b);

                    if (dist == 0)
                    {
                        continue;
                    }

                    if (dist <= a.R + b.R)
                    {
                        double tempVx = a.VelocityX;
                        double tempVy = a.VelocityY;

                        a.VelocityX = b.VelocityX;
                        a.VelocityY = b.VelocityY;

                        b.VelocityX = tempVx;
                        b.VelocityY = tempVy;

                        double overlap = a.R + b.R - dist;

                        double nx = (a.X - b.X) / dist;
                        double ny = (a.Y - b.Y) / dist;

                        a.X += nx * overlap / 2;
                        a.Y += ny * overlap / 2;

                        b.X -= nx * overlap / 2;
                        b.Y -= ny * overlap / 2;
                    }
                }
            }
        }

        private double distance(Ball a, Ball b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            return dist;
        }
    }
}