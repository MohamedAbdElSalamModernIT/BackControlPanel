using System;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    public static class EnumExtensions {
    public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute {
      return enumValue.GetType()
                      .GetMember(enumValue.ToString())
                      .First()
                      .GetCustomAttribute<TAttribute>();
    }
  }
}