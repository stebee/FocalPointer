using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPointer
{
    public interface IRegistration
    {
        string GetTitle();
        string GetVersion();
        ISettingSchema[] GetSchema();
    }
}
