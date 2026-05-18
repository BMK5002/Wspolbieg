using Data;

namespace Model
{
    public interface IBallService
    {
        void Update( IEnumerable<IBall> balls, double width, double height);

        IEnumerable<IBall> CreateBalls(int ballCount);
    }
}