using System.Net;

namespace GraduationProject.Application.Common.Results
{
    public class Result<T>
    {
        public bool IsSuccess => Status == ResultStatus.Success;
        public ResultStatus Status { get; }
        public string Error { get; }
        public T? Value { get; }
        public HttpStatusCode StatusCode { get; }
        public Result() { }
        public Result(ResultStatus status, T value = default, string error = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Status = status;
            Value = value;
            Error = error;
            StatusCode = statusCode;
        }

        public static Result<T> Success(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
        => new Result<T>(ResultStatus.Success, value, null, statusCode);
        public static Result<T> Created(T value, HttpStatusCode statusCode = HttpStatusCode.Created)
        => new Result<T>(ResultStatus.Success, value, null, statusCode);

        public static Result<T> BadRequest(string error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new Result<T>(ResultStatus.BadRequest, default, error, statusCode);

        public static Result<T> NotFound(string? error = null, HttpStatusCode statusCode = HttpStatusCode.NotFound)
            => new Result<T>(ResultStatus.NotFound, default, error, statusCode);

        public static Result<T> Conflict(string error, HttpStatusCode statusCode = HttpStatusCode.Conflict)
            => new Result<T>(ResultStatus.Conflict, default, error, statusCode);

        public static Result<T> Unauthorized(string? error = null, HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
            => new Result<T>(ResultStatus.Unauthorized, default, error, statusCode);

        public static Result<T> Forbidden(string? error = null, HttpStatusCode statusCode = HttpStatusCode.Forbidden)
            => new Result<T>(ResultStatus.Forbidden, default, error, statusCode);

        public static Result<T> Failure(string error, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            => new Result<T>(ResultStatus.Failure, default, error, statusCode);
    }

}
