using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : INotifyPropertyChanged
    {
        private double x;
        private double y;
        private double r;

        public const double InitialMomentum = 100.0;

        public double X
        {
            get => x;
            set
            {
                if (x == value) return;
                x = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DrawX));
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
                OnPropertyChanged(nameof(DrawY));
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
                OnPropertyChanged(nameof(DrawX));
                OnPropertyChanged(nameof(DrawY));
            }
        }

        public double DrawX => X - R;
        public double DrawY => Y - R;

        public double VelocityX { get; set; }
        public double VelocityY { get; set; }

        public double Mass => Math.PI * R * R;

        public Ball()
        {
        }

        public Ball(double radius, double x = 0, double y = 0, double velocityAngle = 0)
        {
            R = radius;
            X = x;
            Y = y;

            double mass = Math.PI * radius * radius;
            double speed = InitialMomentum / mass;

            VelocityX = speed * Math.Cos(velocityAngle);
            VelocityY = speed * Math.Sin(velocityAngle);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
