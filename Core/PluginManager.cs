using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Reflection;

namespace FocalPointer
{
    class PluginManager : PluginBase
    {
        private IntervalManager _clock;
        private SettingsManager _settings;

        #region Plugin store
        private List<Plugin> _plugins;
        private Dictionary<string, int> _pluginIndex;

        public class Plugin : IPlugin
        {
            public string Id { get; private set; }
            public string Source { get; private set; }
            public string Title { get; private set; }
            public string Version { get; private set; }
            public ISettingSchema[] Schema { get; private set; }
            private IPlugin Instance;

            private class CoreShim : ICoreApi
            {
                private readonly Plugin _instance;
                private readonly IntervalManager _api;
                private readonly SettingsManager _settings;
                public CoreShim(Plugin caller, IntervalManager api, SettingsManager settings)
                {
                    _instance = caller;
                    _api = api;
                    _settings = settings;
                }

                public void LogMessage(string message, LogLevel level = LogLevel.Info)
                {
                    _api.LogMessage(_instance, message, level);
                }

                public void ReportError(string message, ErrorSeverity severity = ErrorSeverity.Major)
                {
                    _api.ReportError(_instance, message, severity);
                }

                public void RequestPopulateTasks()
                {
                    _api.RequestPopulateTasks();
                }

                public void RequestModifyIntervalState(string id, PluginBase.StateChangeType state)
                {
                    _api.RequestModifyIntervalState(_instance, id, state);
                }

                public void RequestSetIntervalName(string name, string id = null)
                {
                    _api.RequestSetIntervalName(_instance, name, id);
                }

                public void RequestSetTaskName(string name, string id = null)
                {
                    _api.RequestSetTaskName(_instance, name, id);
                }

                public void RequestStartInterval(IntervalType type)
                {
                    _api.RequestStartInterval(_instance, type);
                }

                public string RetrieveBlob(string key)
                {
                    return _settings.RetrieveBlob(_instance, key);
                }

                public void StoreBlob(string key, string blob, bool secure)
                {
                    _settings.StoreBlob(_instance, key, blob, secure);
                }

                public void StoreSetting(string key, string value)
                {
                    _settings.StoreSetting(_instance, key, value);
                }
            }

            public bool DoesExist
            {
                get
                {
                    return Instance != null;
                }

                set
                {
                    if (value == false)
                    {
                        Instance = null;
                    }
                }
            }

            private Plugin()
            {
            }

            public static Plugin Register(PluginManager container, IPlugin instance, string source)
            {
                if (instance == null)
                    return null;

                IRegistration registration = instance.GetRegistration();
                if (registration == null)
                    return null;

                Plugin entry = new Plugin();
                entry.Id = System.Guid.NewGuid().ToString();
                entry.Source = source;
                entry.Title = registration.GetTitle();
                entry.Version = registration.GetVersion();
                entry.Schema = registration.GetSchema();
                entry.Instance = instance;

                var settings = container._settings.GetSettings(entry);
                var lastVersion = container._settings.LastVersion(entry);
                if (settings == null)
                    settings = SettingsManager.EmptySettings;

                bool okay = instance.OnInitialize(
                    new CoreShim(entry, container._clock, container._settings),
                    lastVersion,
                    settings);
                if (!okay)
                    return null;

                container._pluginIndex.Add(entry.Id, container._plugins.Count);
                container._plugins.Add(entry);

                return entry;
            }

            public string[] OnPopulateTasks(string mostRecent)
            {
                if (DoesExist)
                    return Instance.OnPopulateTasks(mostRecent);
                else
                    return null;
            }

            public void OnTasksPopulated(string mostRecent, string[] list)
            {
                if (DoesExist)
                    Instance.OnTasksPopulated(mostRecent, list);
            }

            public void OnTaskSelected(string taskName)
            {
                if (DoesExist)
                    Instance.OnTaskSelected(taskName);
            }

            public void OnIntervalPropertyChange(string id, IntervalType mode, string intervalName = null, string taskName = null)
            {
                if (DoesExist)
                    Instance.OnIntervalPropertyChange(id, mode, intervalName, taskName);
            }

            public void OnIntervalStateChange(string id, StateChangeType change, DateTime timestamp, TimeSpan elapsed)
            {
                if (DoesExist)
                    Instance.OnIntervalStateChange(id, change, timestamp, elapsed);
            }

            public void OnClockTick(string id, StateChangeType lastStateChange, DateTime timestamp, TimeSpan elapsed)
            {
                if (DoesExist)
                    Instance.OnClockTick(id, lastStateChange, timestamp, elapsed);
            }

            public string OnSettingEdited(string key, string value, IReadOnlyDictionary<string, string> context)
            {
                if (DoesExist)
                    return Instance.OnSettingEdited(key, value, context);
                else
                    return null;
            }

            public void OnSettingsAccepted(IReadOnlyDictionary<string, string> settings)
            {
                if (DoesExist)
                    Instance.OnSettingsAccepted(settings);
            }

            #region Unused overrides
            public IRegistration GetRegistration()
            {
                // This won't actually be called
                return null;
            }

            public bool OnInitialize(ICoreApi api, string lastVersion, IReadOnlyDictionary<string, string> settings)
            {
                // This won't actually be called
                return false;
            }
            #endregion
        }

        public Plugin GetPlugin(string id)
        {
            if (!_pluginIndex.ContainsKey(id))
                return null;

            int index = _pluginIndex[id];
            if (index < 0 || index >= _plugins.Count)
                return null;

            return _plugins[index];
        }
        #endregion

        public PluginManager()
        {
            _plugins = new List<Plugin>();
            _pluginIndex = new Dictionary<string, int>();
        }

        private void findAndRegisterPlugins()
        {
            var files = Directory.GetFiles(Path.Combine(Application.StartupPath, "Plugins"), "*.dll", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                var dll = Assembly.LoadFile(file);
                if (dll == null)
                {
                    Console.WriteLine($"Cannot load {file}");
                    continue;
                }

                foreach (var type in dll.GetExportedTypes())
                {
                    var constructor = type.GetConstructor(Type.EmptyTypes);

                    // Definitely not interested in any types that 
                    //  can't be constructed
                    if (constructor == null)
                        continue;

                    if (type.GetInterface("IPlugin") != null)
                    {
                        var instance = Activator.CreateInstance(type);
                        if (instance != null)
                        {
                            Plugin.Register(this, instance as IPlugin, dll.GetName().Name);
                        }
                    }
                }
            }
        }

        public bool Initialize(IntervalManager core, params IPlugin[] builtins)
        {
            _clock = core;
            _settings = core.Settings;
            _plugins = new List<Plugin>();

            foreach (IPlugin builtin in builtins)
            {
                Plugin.Register(this, builtin, $"internal");
            }

            findAndRegisterPlugins();

            _clock.Subscribe(this);

            return true;
        }

        public override string[] OnPopulateTasks(string mostRecent)
        {
            List<string> list = new List<string>();

            foreach (IPlugin plugin in _plugins)
            {
                string[] add = plugin.OnPopulateTasks(mostRecent);
                if (add != null)
                    list.AddRange(add);
            }

            return list.ToArray();
        }

        public override void OnTaskSelected(string taskName)
        {
            _plugins.ForEach(plugin => plugin.OnTaskSelected(taskName));
        }

        public override void OnIntervalPropertyChange(string id, IntervalType mode, string intervalName = null, string taskName = null)
        {
            _plugins.ForEach(plugin => plugin.OnIntervalPropertyChange(id, mode, intervalName, taskName));
        }

        public override void OnIntervalStateChange(string id, StateChangeType change, DateTime timestamp, TimeSpan elapsed)
        {
            _plugins.ForEach(plugin => plugin.OnIntervalStateChange(id, change, timestamp, elapsed));
        }

        public override void OnClockTick(string id, StateChangeType lastStateChange, DateTime timestamp, TimeSpan elapsed)
        {
            _plugins.ForEach(plugin => plugin.OnClockTick(id, lastStateChange, timestamp, elapsed));
        }

        #region Unused overrides
        public override IRegistration GetRegistration()
        {
            // This won't actually be called
            return null;
        }

        public override bool OnInitialize(ICoreApi api, string lastVersion, IReadOnlyDictionary<string, string> settings)
        {
            // This won't actually be called
            return false;
        }
        #endregion
    }
}
