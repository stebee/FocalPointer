using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPointer
{
    public interface ICoreApi
    {
        void LogMessage(string message, PluginBase.LogLevel level = PluginBase.LogLevel.Info);
        void ReportError(string message, PluginBase.ErrorSeverity severity = PluginBase.ErrorSeverity.Major);
        void RequestSetIntervalName(string name, string id = null);
        void RequestSetTaskName(string name, string id = null);
        void RequestPopulateTasks();
        void RequestStartInterval(PluginBase.IntervalType type);
        void RequestModifyIntervalState(string id, PluginBase.StateChangeType state);
        void StoreSetting(string key, string value);
        void StoreBlob(string key, string blob, bool secure);
        string RetrieveBlob(string key);
    }
}
