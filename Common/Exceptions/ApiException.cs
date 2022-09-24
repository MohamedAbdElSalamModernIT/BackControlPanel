using Common.Extensions;
using System;
using System.Net;
using Common.Exceptions;

namespace Common {
  public class ApiException : ApplicationException {
    private ApiExceptionType _apiExceptionType;
    private string errorMessage;
    public HttpStatusCode StatusCode { get { return _apiExceptionType.GetAttribute<ErrorAttribute>().StatusCode; } }
    public string ErrorCode { get => _apiExceptionType.GetAttribute<ErrorAttribute>().Code; }
    public string ErrorMessage {
      get { return string.IsNullOrEmpty(errorMessage) ? _apiExceptionType.GetAttribute<ErrorAttribute>().Message : errorMessage; }
      set { errorMessage = value; }
    }
    public ErrorResult[] Errors { get; private set; }
    public ApiException(ApiExceptionType exceptionType) {
      _apiExceptionType = exceptionType;
    }
    public ApiException(ApiExceptionType exceptionType, string message) {
      ErrorMessage = message;
      _apiExceptionType = exceptionType;
    }
    public ApiException(ApiExceptionType exceptionType, ErrorResult[] errors) {
      Errors = errors;
      _apiExceptionType = exceptionType;
    }
  }
}
