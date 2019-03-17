using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocalPointer
{
    class SettingsManager
    {
        public IReadOnlyDictionary<string, string> GetSettings(PluginManager.Plugin source)
        {
            return null;
        }

        public string LastVersion(PluginManager.Plugin source)
        {
            return null;
        }

        public void StoreSetting(PluginManager.Plugin source, string key, string value)
        {

        }

        public void StoreBlob(PluginManager.Plugin source, string key, string blob, bool secure)
        {

        }

        public string RetrieveBlob(PluginManager.Plugin source, string key)
        {
            return null;
        }
    }
}
