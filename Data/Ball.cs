using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : INotifyPropertyChanged
    {
        private double x;
        private double y;
        private double r;

        public double X
        {
            get => x;
            set
            {
                if (x == value) return;
                x = value;
                OnPropertyChanged();
            }
        }

        public double Y
        {
            get => y;
            set
            {
                if (y == value) return;
                y = value;
                OnPropertyChanged();
            }
        }

        public double R
        {
            get => r;
            set
            {
                if (r == value) return;
                r = value;
                OnPropertyChanged();
            }
        }

        public double VelocityX { get; set; }
        public double VelocityY { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
