using System.Diagnostics.CodeAnalysis;
using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class NoTrainingUserException : BaseException
{
    public NoTrainingUserException() : base(HttpStatusCode.NotFound)
    {
    }

    public NoTrainingUserException(long userId, long trainingId) 
        : base(HttpStatusCode.NotFound, $"The training with the ID = '{trainingId}' for User with Id = '{userId}' was not found.")
    {
    }
}