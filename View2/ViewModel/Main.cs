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
        private bool running;
        private readonly object _lock = new object();
        private Task? _loopTask;

        public ObservableCollection<Ball> Balls { get; } = new(); // For UI binding
        private readonly List<Ball> simulationBalls = new();  // For simulation logic

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
            running = false; // Stop any existing simulation
            if (_loopTask != null)
            {
                await _loopTask; // Wait for the existing loop to finish
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

            running = true;
            _loopTask = RunLoop();
        }

        private async Task RunLoop()
        {
            while (running)
            {
                List<Ball> snapshot;
                lock (_lock)  // Ensure thread safety when updating the simulation
                {
                    _BallService.Update(simulationBalls, Width, Height);
                    snapshot = simulationBalls.ToList();  // Create a snapshot for UI update
                }

                await App.Current.Dispatcher.InvokeAsync(() =>  // Update the UI with the snapshot
                {
                    Balls.Clear();
                    foreach (var ball in snapshot)
                    {
                        Balls.Add(ball);
                    }
                });
                await Task.Delay(16);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}