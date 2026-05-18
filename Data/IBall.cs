using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        double Mass { get; }

        double DrawX { get; }
        double DrawY { get; }
    }
}
