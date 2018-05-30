using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRSystem.Service
{
    public static class RecurrentExecutor
    {
        public static Task Run(Action action, TimeSpan interval, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(interval, cancellationToken);

                    action();
                }
            }, cancellationToken);
        }
    }
}