using Data;

namespace Model
{
    public class BallService : IBallService
    {
        public void Update(IEnumerable<Ball> balls, double width, double height)
        {
            var ballList = balls.ToList();

            // Step 1: Update positions using semi-implicit Euler (dt = 1.0)
            foreach (var ball in ballList)
            {
                ball.X += ball.VelocityX;
                ball.Y += ball.VelocityY;
            }

            // Step 2: Handle wall collisions (elastic bouncing)
            foreach (var ball in ballList)
            {
                // Left and right walls
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

                // Top and bottom walls
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

            // Step 3: Ball-to-ball collision detection and response
            for (int i = 0; i < ballList.Count; i++)
            {
                for (int j = i + 1; j < ballList.Count; j++)
                {
                    Ball a = ballList[i];
                    Ball b = ballList[j];

                    double distance = Physics.GetDistance(a, b);

                    // Skip if distance is zero (shouldn't happen, but safety check)
                    if (distance < 1e-10)
                    {
                        continue;
                    }

                    // Check if balls are colliding
                    if (distance <= a.R + b.R)
                    {
                        // Resolve the elastic collision (impulse-based)
                        Physics.ResolveElasticCollision(a, b);

                        // Separate overlapping balls to prevent sticking
                        Physics.SeparateOverlappingBalls(a, b);
                    }
                }
            }
        }
    }
}
