using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        private Task? _loopTask;

        public ObservableCollection<IBall> Balls { get; } = new(); // For UI binding
        private List<IBall> simulationBalls = new();  // For simulation logic

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
            simulationBalls = _BallService.CreateBalls(BallCount).ToList();

            running = true;
            _loopTask = Task.Run(RunLoop);
        }

        private async Task RunLoop()
        {
            while (running)
            {
                if (App.Current == null)
                    return;

                List<IBall> snapshot;
                _BallService.Update(simulationBalls, Width, Height);
                snapshot = simulationBalls.Select(b => (IBall)new Ball(b)).ToList(); // Create a snapshot for UI update 

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