using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Common.Results
{
    public class Result<T>
    {
        public bool IsSuccess => Status == ResultStatus.Success;
        public ResultStatus Status { get; }
        public string Error { get; }
        public T Value { get; }

        private Result(ResultStatus status, T value = default, string error = null)
        {
            Status = status;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value)
            => new(ResultStatus.Success, value);

        public static Result<T> Failure(ResultStatus status, string error)
            => new(status, default, error);
    }

}
