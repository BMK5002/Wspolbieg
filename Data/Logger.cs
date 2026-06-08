using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Diagnostics
{
    public class Logger : ILogger
    {
        private readonly ConcurrentQueue<DiagnosticsEntry> entryQueue;
        private CancellationTokenSource _cts = new();
        private Task? loggingTask;

        public Logger()
        {
            entryQueue = new ConcurrentQueue<DiagnosticsEntry>();
        }

        public void Log(DiagnosticsEntry entry)
        {
            entryQueue.Enqueue(entry);
        }



        public Task StartAsync()
        {
            if (loggingTask != null) { 
                return Task.CompletedTask;
            }
            loggingTask = Task.Run(async () =>
            {
                using var writer = new StreamWriter("diagnostics.log", append: true, encoding: Encoding.ASCII);
                Debug.WriteLine(Path.GetFullPath("diagnostics.log"));
                CancellationToken token = _cts.Token;
                while (!token.IsCancellationRequested || !entryQueue.IsEmpty)
                {
                    while (entryQueue.TryDequeue(out var entry))
                    {
                        await writer.WriteLineAsync(entry.ToString());
                    }

                    if (!token.IsCancellationRequested)
                    {
                        await Task.Delay(100, token);
                    }
                }
                await writer.FlushAsync();
            });
            return Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            _cts.Cancel();
            if (loggingTask != null)
            {
                try
                {
                    await loggingTask;

                }
                catch (OperationCanceledException)
                {
                    // Logging task was cancelled
                }
            }
            _cts.Dispose();
            _cts = new CancellationTokenSource();
            loggingTask = null;
        }
    }
}