using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
using Data;
using Model;

namespace View.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IBallService _BallService;
        private readonly DispatcherTimer _timer;

        public ObservableCollection<Ball> Balls { get; } = new();

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

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Tick += (s, e) => Update();

            StartCommand = new RelayCommand(_ => Start());
        }

        private void Start()
        {
            Balls.Clear();
            var rand = new Random();
            for (int i = 0; i < BallCount; i++)
            {
                Balls.Add(new Ball
                {
                    X = rand.Next(0, (int)Width),
                    Y = rand.Next(0, (int)Height),
                    VelocityX = rand.NextDouble() * 4 - 2,
                    VelocityY = rand.NextDouble() * 4 - 2,
                    R = 5
                });

            }

            _timer.Start();
        }

        private void Update()
        {
           _BallService.Update(Balls, Width, Height);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}