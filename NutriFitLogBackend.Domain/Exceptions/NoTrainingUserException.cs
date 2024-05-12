using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

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