using Data;

namespace Model
{
    public class BallService : IBallService
    {
        public void Update(IEnumerable<Ball> balls, double width, double height, double deltaTime)
        {
            var ballList = balls.ToList();

            foreach (var ball in ballList)
            {
                UpdateBallPosition(ball, deltaTime);
            }

            foreach (var ball in ballList)
            {
                HandleWallCollisions(ball, width, height);
            }

            HandleBallCollisions(ballList);
        }

        public void UpdateBallPosition(Ball ball, double deltaTime)
        {
            ball.X += ball.VelocityX * deltaTime;
            ball.Y += ball.VelocityY * deltaTime;
        }

        public void HandleWallCollisions(Ball ball, double width, double height)
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

        public void HandleBallCollisions(IEnumerable<Ball> balls)
        {
            var ballList = balls.ToList();

            for (int i = 0; i < ballList.Count; i++)
            {
                for (int j = i + 1; j < ballList.Count; j++)
                {
                    Ball a = ballList[i];
                    Ball b = ballList[j];

                    double distance = Physics.GetDistance(a, b);

                    if (distance < 1e-10)
                    {
                        continue;
                    }

                    if (distance <= a.R + b.R)
                    {
                        Physics.ResolveElasticCollision(a, b);

                        Physics.SeparateOverlappingBalls(a, b);
                    }
                }
            }
        }
    }
}
