using Data;
using Diagnostics;
using System.Diagnostics;

namespace Model
{
    public class BallSimulator
    {
        private readonly IBallService _ballService;
        private readonly List<Ball> _balls;
        private readonly double _width;
        private readonly double _height;
        private CancellationTokenSource _cts = new();
        private bool _running = false;
        private readonly ILogger _logger;
        private Timer? _timer;


        public BallSimulator(IEnumerable<Ball> balls, IBallService ballService, double width, double height, ILogger logger)
        {
            _balls = balls.ToList();
            _ballService = ballService;
            _width = width;
            _height = height;
            _logger = logger;
        }

        public Task StartAsync()
        {
            _running = true;

            const double deltaTime = 0.016;

            _timer = new Timer(_ =>
            {
                if (!_running || _cts.IsCancellationRequested)
                    return;

                var updateTasks = _balls.Select(ball =>
                    Task.Run(() =>
                    {
                        _ballService.UpdateBallPosition(ball, deltaTime);
                        _ballService.HandleWallCollisions(ball, _width, _height);
                    }, _cts.Token)
                ).ToList();

                Task.WaitAll(updateTasks.ToArray());

                List<DiagnosticsEntry> logs;

                _ballService.HandleBallCollisions(_balls);

                logs = _balls.Select(ball => new DiagnosticsEntry
                {
                    Time = DateTime.Now,
                    X = ball.X,
                    Y = ball.Y,
                    VelocityX = ball.VelocityX,
                    VelocityY = ball.VelocityY
                }).ToList();
                

                foreach (var log in logs)
                {
                    _logger.Log(log);
                }

            }, null, 0, 16);

            return Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            _running = false;
            _cts.Cancel();

            await Task.Delay(50);

            _cts.Dispose();
            _cts = new();
        }

        public List<Ball> GetSnapshot()
        {
            List<Ball> ballTemp = new List<Ball>();
            foreach (var ball in _balls)
            {
                lock (ball._ballLock)
                {
                    ballTemp.Add(new Ball(ball.R, ball.X, ball.Y)
                    {
                        VelocityX = ball.VelocityX,
                        VelocityY = ball.VelocityY
                    });
                }
            }
            return ballTemp.ToList();
        }
    }
}
