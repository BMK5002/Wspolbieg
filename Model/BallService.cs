using Data;

namespace Model
{
    public class BallService : IBallService
    {
        public void Update(IEnumerable<IBall> balls, double width, double height)
        {
            var ballList = balls.ToList();

            // Update positions
            foreach (var ball in ballList)
            {
                ball.X += ball.VelocityX;
                ball.Y += ball.VelocityY;
            }

            // Handle wall collisions
            foreach (var ball in ballList)
            {
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

            // Handle ball collisions
            for (int i = 0; i < ballList.Count; i++)
            {
                for (int j = i + 1; j < ballList.Count; j++)
                {
                    IBall a = ballList[i];
                    IBall b = ballList[j];

                    double distance = Physics.GetDistance(a, b);

                    // Skip if distance is zero
                    if (distance < 1e-10)
                    {
                        continue;
                    }

                    // Check if balls are colliding
                    if (distance <= a.R + b.R)
                    {
                        Physics.ResolveElasticCollision(a, b);

                        Physics.SeparateOverlappingBalls(a, b);
                    }
                }
            }
        }

        public IEnumerable<IBall> CreateBalls(int ballCount)
        {
            var rand = new Random();
            var balls = new List<IBall>();
            for (int i = 0; i < ballCount; i++)
            {
                double radius = rand.NextDouble() * 5.0 + 3.0;
                double x = rand.Next((int)(radius + 1), (int)(800 - radius - 1));
                double y = rand.Next((int)(radius + 1), (int)(600 - radius - 1));
                double velocityAngle = rand.NextDouble() * 2 * Math.PI;
                balls.Add(new Ball(radius, x, y, velocityAngle));
            }
            return balls;
        }
    }
}
