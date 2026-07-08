using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Commons.Results
{
    public class Result<T>
    {
        public bool IsSuccess => Status == ResultStatus.Success;
        public ResultStatus Status { get; }
        public string Error { get; }
        public T? Value { get; }
        public string Message { get; }
        private Result(ResultStatus status, T value = default, string error = null, string message = null)
        {
            Status = status;
            Value = value;
            Error = error;
            Message = message;
        }

        public static Result<T> Success(T value, string message = null)
            => new(ResultStatus.Success, value, message);

        public static Result<T> Failure(ResultStatus status, string error)
            => new(status, default, error);
    }
}
