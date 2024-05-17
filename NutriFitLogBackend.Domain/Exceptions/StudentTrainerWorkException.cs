using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

public class StudentTrainerWorkException : BaseException
{
    public StudentTrainerWorkException() : base(HttpStatusCode.Conflict)
    {
    }

    public StudentTrainerWorkException(string message) : base(HttpStatusCode.Conflict, message)
    {
    }
}