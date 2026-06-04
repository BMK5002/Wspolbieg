using Data;

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

        public BallSimulator(IEnumerable<Ball> balls, IBallService ballService, double width, double height)
        {
            _balls = balls.ToList();
            _ballService = ballService;
            _width = width;
            _height = height;
        }

        public Task StartAsync()
        {
            return Task.Run(async () =>
            {
                _running = true;

                try
                {
                    while (_running && !_cts.IsCancellationRequested)
                    {
                        var updateTasks = _balls.Select(ball =>
                            Task.Run(() =>
                            {
                                _ballService.UpdateBallPosition(ball);
                                _ballService.HandleWallCollisions(ball, _width, _height);
                            }, _cts.Token)
                        ).ToList();

                        await Task.WhenAll(updateTasks);

                        lock (_collisionLock)
                        {
                            _ballService.HandleBallCollisions(_balls);
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
