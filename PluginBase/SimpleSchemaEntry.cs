using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPointer
{
    public class SimpleSchemaEntry : ISettingSchema
    {
        public string Key;
        public string Label;
        public string Description;
        public SettingType Type;

        public string GetDescription()
        {
            return Description;
        }

        public string GetKey()
        {
            return Key;
        }

        public string GetLabel()
        {
            return Label;
        }

        SettingType ISettingSchema.GetType()
        {
            return Type;
        }

        public static SimpleSchemaEntry[] CreateRadioGroup(string key, string label, string description, Dictionary<string, string> entries)
        {
            List<SimpleSchemaEntry> group = new List<SimpleSchemaEntry>();

            var parent = new SimpleSchemaEntry();
            parent.Key = key;
            parent.Label = label;
            parent.Description = description;
            parent.Type = SettingType.RadioGroup;
            group.Add(parent);

            foreach (string value in entries.Keys)
            {
                var entry = new SimpleSchemaEntry();
                entry.Key = key;
                entry.Label = value;
                entry.Description = entries[value];
                entry.Type = SettingType.RadioEntry;
                group.Add(entry);
            }

            return group.ToArray();
        }
    }
}
