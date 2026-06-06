using Data;

namespace Model
{
    public interface IBallService
    {
        void Update(IEnumerable<Ball> balls, double width, double height, double deltaTime);
        void UpdateBallPosition(Ball ball, double deltaTime);
        void HandleWallCollisions(Ball ball, double width, double height);
        void HandleBallCollisions(IEnumerable<Ball> balls);
    }
}
