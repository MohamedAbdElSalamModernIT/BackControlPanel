using System;
using System.Collections.Generic;
using System.Linq;
using Common.Exceptions;
using Common.Extensions;
using MediatR;
using X.PagedList;

namespace Common
{


    public class Result : IRequest
    {
        public Result()
        {

        }
        internal Result(bool success, ApiExceptionType code, object payload = null, string message = default)
        {
            Success = success;
            Payload = payload;
            Code = code;
            Message = GetMessage(message);
            CodeNumber = code.GetAttribute<ErrorAttribute>().Code;
        }


        internal Result(bool success, ApiExceptionType code, string message = default, IEnumerable<string> errors = default)
        {
            Success = success;
            Code = code;
            Errors = errors?.ToArray();
            Message = GetMessage(message);
            CodeNumber = code.GetAttribute<ErrorAttribute>().Code;
        }




        private string GetMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
                return message;
            var attr = Code.GetAttribute<ErrorAttribute>();
            return attr.Message;

        }
        #region Properties

        public bool Success { get; set; }
        public string[] Errors { get; set; } = new string[] { };

        public string CodeNumber { get; set; }
        public object Payload { get; set; }
        public string Message { get; set; }
        public ApiExceptionType Code { get; set; }
        public string RequestId { get; set; }
        #endregion


        public static Result Successed()
        {
            return new Result(true, ApiExceptionType.Ok, null);
        }

        public static Result Successed(object data, string message = default)
        {
            return new Result(true, ApiExceptionType.Ok, data, message);
        }
        public static Result Successed(object data, ApiExceptionType code, string message = default)
        {
            return new Result(true, code, data, message);
        }

        public static Result Successed<T>(PagedList<T> data)
        {
            // var items = new {
            //   Items = data,
            //   MetaDate = data.GetMetaData()
            // };

            return new Result(true, ApiExceptionType.Ok, data);
        }

        public static Result Failure(ApiExceptionType code, string message = default, IEnumerable<string> errors = default)
        {
            return new Result(false, code, message, errors);
        }
    }

    public class Result<T> : Result
    {
        public new T Payload { get; set; }
    }

    public class ResultSuccess<T> : Result<T>
    {

    }
    public class ResultFail<T> : Result<T>
    {

    }
}