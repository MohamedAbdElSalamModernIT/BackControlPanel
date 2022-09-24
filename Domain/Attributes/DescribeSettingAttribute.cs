using System;
using Domain.Enums;

namespace Domain.Attributes {
  public class DescribeSettingAttribute : Attribute {

    public string Key { get; set; }
    public SettingType Type { get; set; }
    public string DefaultValue { get; set; }

    public DescribeSettingAttribute(string key,SettingType type,string defaultValue ) {
      Type = type;
      DefaultValue = defaultValue;
      Key = key;
    }
  }
}