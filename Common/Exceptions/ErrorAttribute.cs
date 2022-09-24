
using System;
using System.Net;
using System.Reflection;

namespace Common {
  public class ErrorAttribute : Attribute {
    /// <summary>
    /// Constructor that receives a <paramref name="code"/> as a string
    /// </summary>
    /// <param name="code"></param>
    /// <param name="statusCode"></param>
    public ErrorAttribute(string code, HttpStatusCode statusCode) {
      Code = code;
      StatusCode = statusCode;
    }
    /// <summary>
    /// predefined error code
    /// </summary>
    public string Code { get; } = string.Empty;
    public HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;

    /// <summary>
    /// Description message for the error
    /// </summary>
    public string Message { get; set; }


    /// <summary>
    /// Get custom enum error 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ErrorAttribute GetErrorAttribute<T>(T type) where T : Enum {
      //TODO: Cach...!
      var enumType = typeof(T);
      var memInfo = enumType.GetMember(type.ToString());
      var attr = memInfo[0].GetCustomAttribute<ErrorAttribute>(false);
      if (attr == null)
        throw new ApplicationException("Type must be decorated with [Error] Attribute");
      return attr;
    }
  }
}
