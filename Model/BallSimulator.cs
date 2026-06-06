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
        private readonly object _collisionLock = new object();
        private bool _running = false;
        private readonly ILogger _logger;


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
            return Task.Run(async () =>
            {
                _running = true;
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                double lastTime = stopWatch.Elapsed.TotalSeconds;

                try
                {
                    while (_running && !_cts.IsCancellationRequested)
                    {
                        double currentTime = stopWatch.Elapsed.TotalSeconds;
                        double deltaTime = currentTime - lastTime;
                        lastTime = currentTime;
                        var updateTasks = _balls.Select(ball =>
                            Task.Run(() =>
                            {
                                _ballService.UpdateBallPosition(ball, deltaTime);
                                _ballService.HandleWallCollisions(ball, _width, _height);
                            }, _cts.Token)
                        ).ToList();

                        await Task.WhenAll(updateTasks);

                        List<DiagnosticsEntry> logs;
                        lock (_collisionLock)
                        {
                            _ballService.HandleBallCollisions(_balls);

                            logs = _balls.Select(ball => new DiagnosticsEntry
                            {
                                Time = DateTime.Now,
                                X = ball.X,
                                Y = ball.Y,
                                VelocityX = ball.VelocityX,
                                VelocityY = ball.VelocityY
                            }).ToList();
                        }

                        foreach (var log in logs)
                        {
                            _logger.Log(log);
                        }

                        await Task.Delay(16, _cts.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Simulation was cancelled
                }
            }, _cts.Token);
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
            lock (_collisionLock)
            {
                return _balls.Select(ball =>
                    new Ball(ball.R, ball.X, ball.Y)
                    {
                        VelocityX = ball.VelocityX,
                        VelocityY = ball.VelocityY
                    }
                ).ToList();
            }
        }
    }
}
