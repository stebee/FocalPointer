using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPointer
{
    public class SimpleRegistration : IRegistration
    {
        public string Title;
        public string Version;

        private List<ISettingSchema> _schema;
        public void SetSchema(ISettingSchema[] schema)
        {
            _schema = new List<ISettingSchema>(schema);
        }

        public void AddSchemaEntry(ISettingSchema entry)
        {
            if (_schema == null)
                _schema = new List<ISettingSchema>();

            _schema.Add(entry);
        }

        public ISettingSchema[] GetSchema()
        {
            if (_schema == null)
                return new ISettingSchema[0];
            else
                return _schema.ToArray();
        }

        public string GetTitle()
        {
            return Title;
        }

        public string GetVersion()
        {
            return Version;
        }
    }
}
