using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPointer
{
    public interface ICoreApi
    {
        void LogMessage(string message, PluginBase.LogLevel level = PluginBase.LogLevel.Info);
        void ReportError(string message, PluginBase.ErrorSeverity severity = PluginBase.ErrorSeverity.Major);
        void RequestSetIntervalName(string name);
        void RequestSetTaskName(string name);
        void RequestStartInterval(PluginBase.IntervalType type);
        void RequestPauseInterval(string id);
        void RequestEndInterval(string id);
        void StoreSetting(string key, string value);
        void StoreBlob(string key, string blob, bool secure);
        string RetrieveBlob(string key);
    }
}
