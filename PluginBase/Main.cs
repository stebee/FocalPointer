using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPointer
{
    public abstract class PluginBase : IPlugin
    {
        public enum IntervalType
        {
            Focus,
            Rest
        }

        public enum StateChangeType
        {
            Start,
            Pause,
            Unpause,
            End,
            TextUpdate
        }

        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error
        }

        public enum ErrorSeverity
        {
            Minor,      // Don't even report
            Major,      // Report but continue  
            Fatal       // The plugin is shutting down
        }

        // This functionality *must* be implemented.
        public abstract IRegistration GetRegistration();
        public abstract bool OnInitialize(ICoreApi api, string lastVersion, IReadOnlyDictionary<string, string> settings);

        // The rest of these can be left alone
        public virtual string[] OnPopulateTasks(string mostRecent) { return null; }
        public virtual void OnTasksPopulated(string mostRecent, string[] list) { }
        public virtual void OnTaskSelected(string taskName) { }
        public virtual void OnIntervalCreated(string id, IntervalType mode, string intervalName = null, string taskName = null) { }
        public virtual void OnIntervalStateChange(string id, StateChangeType change, DateTime timestamp, TimeSpan elapsed) { }
        public virtual void OnClockTick(string id, StateChangeType lastStateChange, DateTime timestamp, TimeSpan elapsed) { }

        public virtual string OnSettingEdited(string key, string value, IReadOnlyDictionary<string, string> context) { return null; }
        public virtual void OnSettingsAccepted(IReadOnlyDictionary<string, string> settings) { }
    }
}
