using System;
using System.Collections.Generic;
using FocalPointer;

namespace PluginToggl
{
    public class PluginToggl : FocalPointer.PluginBase, FocalPointer.IRegistration
    {
        public string GetTitle()
        {
            return "Toggl Integration";
        }

        public string GetVersion()
        {
            return SiobhanDev.AssemblyAttributes.Version.Substring(0, 1);
        }

        public ISettingSchema[] GetSchema()
        {
            List<SimpleSchemaEntry> schema = new List<SimpleSchemaEntry>();

            var entry = new SimpleSchemaEntry();
            entry.Key = "TITLE";
            entry.Description = $"{SiobhanDev.AssemblyAttributes.Title}";
            entry.Type = SettingType.Label;
            schema.Add(entry);

            entry = new SimpleSchemaEntry();
            entry.Key = "AUTHOR";
            entry.Description = $"by {SiobhanDev.AssemblyAttributes.Company}";
            entry.Type = SettingType.Label;
            schema.Add(entry);

            entry = new SimpleSchemaEntry();
            entry.Key = "COPYRIGHT";
            entry.Description = 
                $"{SiobhanDev.AssemblyAttributes.Copyright}\n" +
                 "Toggl API for .NET © 2018 Panoramic Data Limited.\n" +
                 "Toggl® is a trademark of Toggl OÜ";
            entry.Type = SettingType.BigLabel;
            schema.Add(entry);

            entry = new SimpleSchemaEntry();
            entry.Key = "API_KEY";
            entry.Label = "API Token";
            entry.Description = "Copy the value found at the bottom of your profile page at Toggl.com.";
            entry.Type = SettingType.Password;
            schema.Add(entry);

            entry = new SimpleSchemaEntry();
            entry.Type = SettingType.Separator;
            schema.Add(entry);

            return schema.ToArray();
        }

        public override IRegistration GetRegistration()
        {
            return this;
        }

        public override bool OnInitialize(ICoreApi api, string lastVersion, IReadOnlyDictionary<string, string> settings)
        {
            int x = 0;

            return true;
        }
    }
}
