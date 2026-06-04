using Data;

namespace Model
{
    public interface IBallService
    {
        void Update(IEnumerable<Ball> balls, double width, double height);
        void UpdateBallPosition(Ball ball);
        void HandleWallCollisions(Ball ball, double width, double height);
        void HandleBallCollisions(IEnumerable<Ball> balls);
    }
}
