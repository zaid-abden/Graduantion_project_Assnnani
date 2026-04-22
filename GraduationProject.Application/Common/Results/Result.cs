using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace GraduationProject.Application.Common.Results
{
    //public class Result<T>
    //{
    //	public bool IsSuccess => Status == ResultStatus.Success;
    //	public ResultStatus Status { get; }
    //	public string Error { get; } = string.Empty; // منع الـ Null
    //	public T? Value { get; }
    //	public HttpStatusCode StatusCode { get; } // الخاصية الجديدة

    //	// 1. المنشئ القديم (للحفاظ على التوافق)
    //	private Result(ResultStatus status, T? value = default, string? error = null)
    //	{
    //		Status = status;
    //		Value = value;
    //		Error = error ?? string.Empty;
    //		// تعيين StatusCode تلقائي بناءً على الـ Status القديم
    //		StatusCode = MapStatusToStatusCode(status);
    //	}

    //	// 2. المنشئ الجديد (إذا أردت تمرير StatusCode يدويًا مستقبلاً)
    //	private Result(ResultStatus status, T? value, string? error, HttpStatusCode statusCode)
    //	{
    //		Status = status;
    //		Value = value;
    //		Error = error ?? string.Empty;
    //		StatusCode = statusCode;
    //	}

    //	// --- الميثودز القديمة (لن تلمسها لضمان عدم حدوث Errors) ---
    //	public static Result<T> Success(T value)
    //		=> new(ResultStatus.Success, value);

    //	public static Result<T> Failure(ResultStatus status, string error)
    //		=> new(status, default, error);

    //	// --- الميثودز الجديدة (التي طلبناها للتقرير المالي والـ AI) ---
    //	public static Result<T> Success(T value, HttpStatusCode statusCode)
    //		=> new(ResultStatus.Success, value, null, statusCode);

    //	public static Result<T> NotFound(string error)
    //		=> new(ResultStatus.NotFound, default, error, HttpStatusCode.NotFound);

    //	public static Result<T> BadRequest(string error)
    //		=> new(ResultStatus.ValidationError, default, error, HttpStatusCode.BadRequest);

    //	public static Result<T> InternalError(string error)
    //		=> new(ResultStatus.Failure, default, error, HttpStatusCode.InternalServerError);

    //	// ميثود مساعدة لربط الـ Status بالـ Code تلقائيًا
    //	private static HttpStatusCode MapStatusToStatusCode(ResultStatus status) => status switch
    //	{
    //		ResultStatus.Success => HttpStatusCode.OK,
    //		ResultStatus.ValidationError => HttpStatusCode.BadRequest,
    //		ResultStatus.NotFound => HttpStatusCode.NotFound,
    //		ResultStatus.Conflict => HttpStatusCode.Conflict,
    //		ResultStatus.Unauthorized => HttpStatusCode.Unauthorized,
    //		ResultStatus.Forbidden => HttpStatusCode.Forbidden,
    //		_ => HttpStatusCode.InternalServerError
    //	};
    //}

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