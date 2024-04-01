using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException() : base(HttpStatusCode.NotFound)
    {
    }

    public NotFoundException(string message) : base(HttpStatusCode.NotFound, message)
    {
    }
}
