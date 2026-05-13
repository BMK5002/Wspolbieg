using Data;

namespace Model
{
    public interface IBallService
    {
        void Update( IEnumerable<Ball> balls, double width, double height);
    }
}