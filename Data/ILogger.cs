using System;
using System.Collections.Generic;
using System.Text;

namespace Diagnostics
{
    public interface ILogger
    {
        void Log(DiagnosticsEntry entry);
        Task StartAsync();
        Task StopAsync();
    }
}
