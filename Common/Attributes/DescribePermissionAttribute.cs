using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Attributes
{
    public class DescribePermissionAttribute : Attribute
    {
        public SystemModule Module { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }

        public DescribePermissionAttribute(SystemModule module, string key, string title, string group)
        {
            Title = title;
            Group = group;
            Key = key;
            Module = module;
        }
    }

    public enum SystemModule
    {
        [EnumMember(Value = "user_management_module")]
        UserManagementModule = 1,

        [EnumMember(Value = "category")] 
        Category,

        [EnumMember(Value = "condition")]
        Condition,

        [EnumMember(Value = "baladya")]
        Baladya,

        [EnumMember(Value = "amana")]
        Amana,

        [EnumMember(Value = "information")]
        Information,
    }
}