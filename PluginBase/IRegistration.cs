using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPointer
{
    public interface IRegistration
    {
        string GetName();
        string GetVersion();
        object GetSetting(string key);
        ISettingSchema[] GetSchema();
    }
}
