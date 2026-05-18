using Data;

namespace Model
{
    public class BallService : IBallService
    {
        private readonly object _lock = new();
        public void Update(IEnumerable<IBall> balls, double width, double height)
        {
            var ballList = balls.ToList();
            List<(IBall ball, double x, double y, double vx, double vy)> results;

            // Concurrently calculate new positions and velocities for each ball, handling wall collisions
            results = ballList
                .AsParallel()
                .Select(ball =>
                {
                    double x = ball.X + ball.VelocityX;
                    double y = ball.Y + ball.VelocityY;
                    double vx = ball.VelocityX;
                    double vy = ball.VelocityY;

                    if (x - ball.R < 0)
                    { 
                        x = ball.R; vx *= -1;
                    }
                    if (x + ball.R > width)
                    { 
                        x = width - ball.R;
                        vx *= -1;
                    }
                    if (y - ball.R < 0)
                    { 
                        y = ball.R;
                        vy *= -1;
                    }
                    if (y + ball.R > height)
                    { 
                        y = height - ball.R;
                        vy *= -1;
                    }

                    return (ball, x, y, vx, vy);
                })
                .ToList();

            lock (_lock)
            {
                // Update ball positions and velocities after wall collision handling
                foreach (var r in results)
                {
                    r.ball.X = r.x;
                    r.ball.Y = r.y;
                    r.ball.VelocityX = r.vx;
                    r.ball.VelocityY = r.vy;
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
