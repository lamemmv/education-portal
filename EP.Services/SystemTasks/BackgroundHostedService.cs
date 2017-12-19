using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace EP.Services.SystemTasks
{
    public abstract class BackgroundHostedService : IHostedService
    {
        private Task _executingTask;
        private CancellationTokenSource _cts;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a linked token so we can trigger cancellation outside of this token's cancellation.
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Store the task we're executing.
            _executingTask = ExecuteAsync(_cts.Token);

            // If the task is completed then return it.
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running.
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start.
            if (_executingTask == null)
            {
                return;
            }

            // Signal cancellation to the executing method.
            _cts.Cancel();

            // Wait until the task completes or the stop token triggers.
            await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));

            // Throw if cancellation triggered.
            cancellationToken.ThrowIfCancellationRequested();
        }

        protected virtual async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            int sleepInSeconds = (int)(LoopInSeconds * 1000);

            if (sleepInSeconds <= 0)
            {
                return;
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(sleepInSeconds, cancellationToken);
                
                try
                {
                    await ExecuteOnceAsync(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (AggregateException)
                {
                }
            }
        }

        protected abstract float LoopInSeconds { get; }

        // Derived classes should override this and execute a long running method until 
        // cancellation is requested.
        protected abstract Task ExecuteOnceAsync(CancellationToken cancellationToken);
    }
}