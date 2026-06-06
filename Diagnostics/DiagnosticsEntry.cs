namespace Diagnostics
{
    public class DiagnosticsEntry
    {
        public DateTime Time { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }

        public override string ToString()
        {
            return $"{Time:O};{X};{Y};{VelocityX};{VelocityY}";
        }
    }
}
