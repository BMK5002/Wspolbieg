using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
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

            

            StartCommand = new RelayCommand(_ => Start());
        }

        private void Start()
        {
            simulationBalls.Clear();
            var rand = new Random();
            for (int i = 0; i < BallCount; i++)
            {
                simulationBalls.Add(new Ball
                {
                    X = rand.Next(0, (int)Width),
                    Y = rand.Next(0, (int)Height),
                    VelocityX = rand.NextDouble() * 4 - 2,
                    VelocityY = rand.NextDouble() * 4 - 2,
                    R = 5
                }); 
            }

            running = true;
            _ = RunLoop();
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

                App.Current.Dispatcher.Invoke(() =>  // Update the UI with the snapshot
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