using System;
using System.Runtime.Serialization;

namespace Common.Attributes {
    public class CommandDescribeAttribute : Attribute {
    public string Template { get; set; }
    public string FileName { get; set; }
    public string IgnoreProps { get; set; }
    public CommandType CommandType { get; set; }
    public CommandDescribeAttribute(string fileName,string template,CommandType commandType,string ignoreProps=default) {
      FileName = fileName;
      Template = template;
      CommandType = commandType;
      IgnoreProps = ignoreProps;
    }
  }
  public enum CommandType {
    [EnumMember(Value = "Commands")]
    Command,
    [EnumMember(Value = "Queries")]
    Query
  }
}