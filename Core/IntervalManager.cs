using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocalPointer
{
    class IntervalManager
    {
        private System.Timers.Timer _timer;

        public IntervalManager(SettingsManager settings)
        {
            Settings = settings;

            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.AutoReset = true;
            _timer.Elapsed += timer_Tick;
            _timer.Start();
        }

        public SettingsManager Settings { get; private set; }

        private List<IPlugin> _subscribers = new List<IPlugin>();
        public void Subscribe(IPlugin plugin)
        {
            _subscribers.Add(plugin);
        }

        private string _nextName;
        private string _nextTask;

        private Dictionary<string, Interval> _intervals = new Dictionary<string, Interval>();
        private List<string> _history = new List<string>();
        private string _currentIntervalId
        {
            get
            {
                if (_history.Count == 0)
                    return null;
                else
                    return _history[_history.Count - 1];
            }
        }

        private Interval getInterval(string id)
        {
            if (id == null)
                return null;

            if (_intervals.ContainsKey(id))
                return _intervals[id];
            else
                return null;
        }

        private Interval createInterval()
        {
            var interval = new Interval();
            interval.Id = Guid.NewGuid().ToString();
            _intervals.Add(interval.Id, interval);
            _history.Add(interval.Id);
            return interval;
        }

        public void LogMessage(PluginManager.Plugin caller, string message, PluginBase.LogLevel level = PluginBase.LogLevel.Info)
        {
            // TODO
        }

        public void ReportError(PluginManager.Plugin caller, string message, PluginBase.ErrorSeverity severity = PluginBase.ErrorSeverity.Major)
        {
            // TODO
        }

        public void RequestPopulateTasks()
        {
            // TODO
        }

        private void beginFocusInterval(string name, string task, DateTime timestamp)
        {
            var interval = createInterval();

            interval.Name = name;
            interval.Task = task;
            interval.Type = PluginBase.IntervalType.Focus;
            interval.State = PluginBase.StateChangeType.Start;
            interval.Begun = timestamp;
            interval.TargetDuration = new TimeSpan(0, 25, 0);

            broadcastPropertyChange(interval);
            interval.Elapsed = DateTime.Now - interval.Begun;
            broadcastStateChange(interval, PluginBase.StateChangeType.Start, timestamp);
        }

        private void beginRestInterval(DateTime timestamp)
        {
            var interval = createInterval();

            interval.Name = null;
            interval.Task = null;
            interval.Type = PluginBase.IntervalType.Rest;
            interval.State = PluginBase.StateChangeType.Start;
            interval.Begun = timestamp;
            interval.TargetDuration = new TimeSpan(0, 5, 0);

            broadcastPropertyChange(interval);
            interval.Elapsed = DateTime.Now - interval.Begun;
            broadcastStateChange(interval, PluginBase.StateChangeType.Start, timestamp);
        }

        private void endInterval(Interval interval, bool requested)
        {
            var timestamp = DateTime.Now;
            if (!requested)
                timestamp = interval.Begun + interval.Elapsed;
            interval.State = PluginBase.StateChangeType.End;
            broadcastStateChange(interval, PluginBase.StateChangeType.End, timestamp);

            if (!requested && interval.Type == PluginBase.IntervalType.Focus)
            {
                // Start a rest automatically
                beginRestInterval(timestamp);
            }
        }

        private void timer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            var timestamp = DateTime.Now;

            var current = getInterval(_currentIntervalId);

            if (current == null)
                return;

            if (current.State == PluginBase.StateChangeType.End)
                return;

            if (current.State == PluginBase.StateChangeType.Start)
            {
                current.Elapsed = timestamp - current.Begun;
                if (current.Elapsed >= current.TargetDuration)
                {
                    current.Elapsed = current.TargetDuration;
                    endInterval(current, false);
                }
            }

            broadcastClockTick(current, timestamp);
        }

        private void broadcastClockTick(Interval interval, DateTime timestamp)
        {
            foreach (IPlugin plugin in _subscribers)
            {
                plugin.OnClockTick(interval.Id, interval.State, timestamp, interval.Elapsed);
            }
        }

        private void broadcastPropertyChange(Interval interval)
        {
            foreach (IPlugin plugin in _subscribers)
            {
                plugin.OnIntervalPropertyChange(interval.Id, interval.Type, interval.Name, interval.Task);
            }
        }

        private void broadcastStateChange(Interval interval, PluginBase.StateChangeType state, DateTime timestamp)
        {
            foreach (IPlugin plugin in _subscribers)
            {
                plugin.OnIntervalStateChange(interval.Id, state, timestamp, interval.Elapsed);
            }
        }

        public void RequestModifyIntervalState(PluginManager.Plugin caller, string id, PluginBase.StateChangeType state)
        {
            // Can't change state of old intervals
            if (id != _currentIntervalId)
                return;

            Interval current = getInterval(_currentIntervalId);

            if (current.State == PluginBase.StateChangeType.End)
                return;

            if (current.State == state)
                return;

            switch (state)
            {
                case PluginBase.StateChangeType.Unpause:
                    if (current.State != PluginBase.StateChangeType.Pause)
                        return;
                    current.State = PluginBase.StateChangeType.Start;
                    broadcastStateChange(current, PluginBase.StateChangeType.Unpause, DateTime.Now);
                    break;

                case PluginBase.StateChangeType.Pause:
                    if (current.State != PluginBase.StateChangeType.Start)
                        return;
                    current.State = PluginBase.StateChangeType.Pause;
                    broadcastStateChange(current, PluginBase.StateChangeType.Pause, DateTime.Now);
                    break;

                case PluginBase.StateChangeType.End:
                    endInterval(current, true);
                    break;

                case PluginBase.StateChangeType.Start:
                    // Invalid
                    break;

                default:
                    // Shouldn't happen!
                    break;
            }
        }

        public void RequestSetIntervalName(PluginManager.Plugin caller, string name, string id = null)
        {
            if (id == null)
                _nextName = name;

            var modify = getInterval(id);
            if (modify == null)
                return;

            modify.Name = name;
            broadcastPropertyChange(modify);
        }

        public void RequestSetTaskName(PluginManager.Plugin caller, string name, string id = null)
        {
            if (id == null)
                _nextTask = name;

            var modify = getInterval(id);
            if (modify == null)
                return;

            modify.Task = name;
            broadcastPropertyChange(modify);
        }

        public void RequestStartInterval(PluginManager.Plugin caller, PluginBase.IntervalType type)
        {
            var current = getInterval(_currentIntervalId);
            if (current != null)
            {
                if (current.State != PluginBase.StateChangeType.End)
                    return;
            }

            if (type == PluginBase.IntervalType.Focus)
            {
                if (String.IsNullOrEmpty(_nextName))
                    _nextName = "[name not set]";
                if (_nextTask == null)
                    _nextTask = "";
                beginFocusInterval(_nextName, _nextTask, DateTime.Now);
            }
            else
                beginRestInterval(DateTime.Now);
        }
    }
}
