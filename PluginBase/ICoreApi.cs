using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPointer
{
    public interface ICoreApi
    {
        void LogMessage(string message, PluginBase.LogLevel level = PluginBase.LogLevel.Info);
        void ReportError(string message, PluginBase.ErrorSeverity severity = PluginBase.ErrorSeverity.Major);
        void SetIntervalName(string name);
        void SetTaskName(string name);
        void StartInterval();
        void PauseInterval();
        void EndInterval();
        void StartBreak();
        void PauseBreak();
        void EndBreak();
        void PersistSetting(string key, string value);
        void PersistData(string blob);
        void PersistData(byte[] blob);
        void PersistDataSecurely(string blob);
        void PersistDataSecurely(byte[] blob);
    }
}
