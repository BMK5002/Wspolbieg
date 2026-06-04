using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Data;
using Model;
using View2;

namespace View.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IBallService _BallService;
        private BallSimulator? _simulator;
        private Task? _loopTask;

        public ObservableCollection<Ball> Balls { get; } = new();
        private readonly List<Ball> simulationBalls = new();

        public double Width { get; set; } = 400;
        public double Height { get; set; } = 300;
        private int _ballCount = 20;
        public int BallCount
        {
            get => _ballCount;
            set
            {
                if (_ballCount == value) return;
                _ballCount = value;
                OnPropertyChanged(nameof(BallCount));
            }
        }

        public ICommand StartCommand { get; }


        public MainViewModel(IBallService BallService)
        {
            _BallService = BallService;
            StartCommand = new RelayCommand(async _ => await Start());
        }

        private async Task Start()
        {
            if (_simulator != null)
            {
                await _simulator.StopAsync();
                _simulator = null;
            }

            Balls.Clear();
            simulationBalls.Clear();
            var rand = new Random();
            for (int i = 0; i < BallCount; i++)
            {
                double radius = rand.NextDouble() * 5.0 + 3.0;

                double x = rand.Next((int)(radius + 1), (int)(Width - radius - 1));
                double y = rand.Next((int)(radius + 1), (int)(Height - radius - 1));
                double velocityAngle = rand.NextDouble() * 2 * Math.PI;

                simulationBalls.Add(new Ball(radius, x, y, velocityAngle));
            }

            _simulator = new BallSimulator(simulationBalls, _BallService, Width, Height);
            _loopTask = _simulator.StartAsync();

            _ = Task.Run(async () =>
            {
                while (_simulator != null)
                {
                    var snapshot = _simulator.GetSnapshot();
                    await App.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Balls.Clear();
                        foreach (var ball in snapshot)
                        {
                            Balls.Add(ball);
                        }
                    });
                    await Task.Delay(16);
                }
            });
        }

        private async Task Stop()
        {
            if (_simulator != null)
            {
                await _simulator.StopAsync();
                _simulator = null;
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}