using System.Diagnostics.CodeAnalysis;
using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class BaseException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public BaseException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public BaseException(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }
}
