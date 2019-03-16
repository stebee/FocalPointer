using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPointer
{
    public enum SettingType
    {
        Hidden,
        String,
        Password,
        BigString,
        Boolean,
        RadioGroup,
        RadioEntry
    };

    public interface ISettingSchema
    {
        string GetKey();
        string GetLabel();
        string GetDescription();
        SettingType GetType();
    }
}
