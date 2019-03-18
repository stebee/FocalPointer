using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocalPointer
{
    public class Interval
    {
        public Interval()
        {
            Reset();
        }

        public void Reset()
        {
            State = PluginBase.StateChangeType.End;
            Begun = DateTime.MaxValue;
            Elapsed = TimeSpan.Zero;
        }

        public string Id;
        public string Name;
        public string Task;
        public PluginBase.IntervalType Type;
        public PluginBase.StateChangeType State;
        public DateTime Begun;
        public TimeSpan Elapsed;
        public TimeSpan TargetDuration;
    }
}
