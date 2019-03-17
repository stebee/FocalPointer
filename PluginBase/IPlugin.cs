using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPointer
{
    public interface IPlugin
    {
        IRegistration GetRegistration();
        bool OnInitialize(ICoreApi api, string lastVersion, IReadOnlyDictionary<string, string> settings);
        string[] OnPopulateTasks(string mostRecent);
        void OnTasksPopulated(string mostRecent, string[] list);
        void OnTaskSelected(string taskName);
        void OnIntervalCreated(string id, PluginBase.IntervalType mode, string intervalName = null, string taskName = null);
        void OnIntervalStateChange(string id, PluginBase.StateChangeType change, DateTime timestamp, TimeSpan elapsed);
        void OnClockTick(string id, PluginBase.StateChangeType lastStateChange, DateTime timestamp, TimeSpan elapsed);
        string OnSettingEdited(string key, string value, IReadOnlyDictionary<string, string> context);
        void OnSettingsAccepted(IReadOnlyDictionary<string, string> settings);
    }
}
