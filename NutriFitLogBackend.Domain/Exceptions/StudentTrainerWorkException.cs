using System.Diagnostics.CodeAnalysis;
using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class StudentTrainerWorkException : BaseException
{
    public StudentTrainerWorkException() : base(HttpStatusCode.Conflict)
    {
    }

    public StudentTrainerWorkException(string message) : base(HttpStatusCode.Conflict, message)
    {
    }
}