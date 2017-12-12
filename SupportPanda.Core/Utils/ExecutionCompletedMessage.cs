using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace SupportPanda.Core
{
    public class ExecutionCompletedMessage : TinyMessageBase
    {
        Benchmark benchMark;

        public ExecutionCompletedMessage(object sender) : base(sender)
        {
        }

        public ExecutionCompletedMessage(object sender,Benchmark benchMark) : base(sender)
        {
            this.benchMark = benchMark;
        }

        public Benchmark BenchMark { get => benchMark; }
    }
}
