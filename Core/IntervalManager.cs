using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocalPointer
{
    class IntervalManager
    {
        public IntervalManager(SettingsManager settings)
        {
            Settings = settings;
        }

        public SettingsManager Settings { get; private set; }

        public void LogMessage(PluginManager.Plugin caller, string message, PluginBase.LogLevel level = PluginBase.LogLevel.Info)
        {
            throw new NotImplementedException();
        }

        public void ReportError(PluginManager.Plugin caller, string message, PluginBase.ErrorSeverity severity = PluginBase.ErrorSeverity.Major)
        {
            throw new NotImplementedException();
        }

        public void RequestEndInterval(PluginManager.Plugin caller, string id)
        {
            throw new NotImplementedException();
        }

        public void RequestPauseInterval(PluginManager.Plugin caller, string id)
        {
            throw new NotImplementedException();
        }

        public void RequestSetIntervalName(PluginManager.Plugin caller, string name)
        {
            throw new NotImplementedException();
        }

        public void RequestSetTaskName(PluginManager.Plugin caller, string name)
        {
            throw new NotImplementedException();
        }

        public void RequestStartInterval(PluginManager.Plugin caller, PluginBase.IntervalType type)
        {
            throw new NotImplementedException();
        }
    }
}
