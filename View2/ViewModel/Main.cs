using System.Collections.ObjectModel;
using System.Windows.Threading;
using Data;
using Model;

namespace View.ViewModel
{
    public class MainViewModel
    {
        private readonly IBallService _BallService;
        private readonly DispatcherTimer _timer;

        public ObservableCollection<Ball> Balls { get; } = new();

        public double Width { get; set; } = 400;
        public double Height { get; set; } = 300;

        public MainViewModel(IBallService BallService)
        {
            _BallService = BallService;

            var rand = new Random();
            for (int i = 0; i < 20; i++)
            {
                Balls.Add(new Ball
                {
                    X = rand.Next(0, 400),
                    Y = rand.Next(0, 300),
                    VelocityX = rand.NextDouble() * 4 - 2,
                    VelocityY = rand.NextDouble() * 4 - 2
                });
            }

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Tick += (s, e) => Update();
            _timer.Start();
        }

        private void Update()
        {
            foreach (var Ball in Balls)
            {
                _BallService.Update(Ball, Width, Height);
            }
        }
    }
}