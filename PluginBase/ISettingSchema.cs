using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPointer
{
    public enum SettingType
    {
        Hidden,
        Text,
        Password,
        BigText,
        Boolean,
        RadioGroup,
        RadioEntry,
        Label,
        BigLabel,
        Message,
        BigMessage,
        Separator
    };

    public interface ISettingSchema
    {
        string GetKey();
        string GetLabel();
        string GetDescription();
        SettingType GetType();
    }
}
