using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPointer
{
    public abstract class PluginBase
    {
        public enum StateChangeType
        {
            Start,
            Pause,
            Unpause,
            End
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
        public abstract void OnInitialize(ICoreApi api, IReadOnlyDictionary<string, string> settings);

        // The rest of these can be left alone
        public void OnPopulateTasks() { }
        public void OnTaskSelected(string taskName) { }
        public void OnIntervalCreated(string id, string intervalName, string taskName) { }
        public void OnIntervalStateChange(string id, StateChangeType change, DateTime timestamp, TimeSpan elapsed) { }
        public void OnBreakStateChange(string id, StateChangeType change, DateTime timestamp, TimeSpan elapsed) { }
        public void OnClockTick(string id, StateChangeType lastStateChange, DateTime timestamp, TimeSpan elapsed) { }
        public void OnSettingsEdited(IReadOnlyDictionary<string, string> settings) { }
        public void OnSettingsAccepted(IReadOnlyDictionary<string, string> settings) { }
    }
}
