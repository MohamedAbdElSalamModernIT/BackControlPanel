using System;

namespace Common.Attributes {
  public class DirectoryPathAttribute : Attribute {
    public string Path { get; set; }

    public DirectoryPathAttribute(string path) {
      Path = path;
    }
  }
}