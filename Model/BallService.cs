using Data;

namespace Model
{
    public class BallService : IBallService
    {
        public void Update(Ball Ball, double width, double height)
        {
            Ball.X += Ball.VelocityX;
            Ball.Y += Ball.VelocityY;

            if (Ball.X < 0 || Ball.X > width)
                Ball.VelocityX *= -1;

            if (Ball.Y < 0 || Ball.Y > height)
                Ball.VelocityY *= -1;
        }
    }
}