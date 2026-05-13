using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IBall
    {
        double X { get; set; }
        double Y { get; set; }
        double R { get; set; }
        double VelocityX { get; set; }
        double VelocityY { get; set; }
    }
}
