using Data;

namespace Model
{
    public interface IBallService
    {
        void Update( IEnumerable<IBall> balls, double width, double height);
        IBall Create(double x, double y, double r, double velocityX, double velocityY);
    }
}