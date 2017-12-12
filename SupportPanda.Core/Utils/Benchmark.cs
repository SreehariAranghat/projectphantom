using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{

    public class Benchmark : IDisposable
    {
        private readonly Stopwatch timer = new Stopwatch();
        private readonly string benchmarkName;

        public Benchmark(string benchmarkName)
        {
            this.benchmarkName = benchmarkName;
            timer.Start();
        }

        public override string ToString()
        {
            return String.Format($"{benchmarkName} {timer.Elapsed}");
        }

        public void Dispose()
        {
            timer.Stop();
            Debug.WriteLine(this.ToString());
            ExecutionCompletedMessage completeMessage = new ExecutionCompletedMessage(this, this);
            MessengerHub.Instance.Publish<ExecutionCompletedMessage>(completeMessage);
        }
    }
}
