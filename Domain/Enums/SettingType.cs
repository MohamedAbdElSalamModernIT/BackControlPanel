﻿using System.ComponentModel;

namespace Domain.Enums
{
    public enum SettingType
    {
        String,
        Number,
        Boolean
    }

    public enum FileType
    {
        [Description("اوتو كاد")]
        Autocad,

        [Description("ريفيت")]
        Revit,
    }

    public enum DrawingType
    {
        [Description("إنشائي")]
        Structure,

        [Description("معماري")]
        Architectural,
    }
    public enum DrawingStatus
    {
        [Description("قيد المراجعة")]
        Pending,

        [Description("تم القبول")]
        Submitted,
        
        [Description("مرفوض")]
        Rejected,
    }

    public enum ConditionStatus
    {
        [Description("نجح")]
        Success,

        [Description("فشل")]
        Fail,

        [Description("أخرى")]
        Other,
    }
}