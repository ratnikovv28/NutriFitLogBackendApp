using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

public class TrainingNotFoundException : BaseException
{
    public TrainingNotFoundException() : base(HttpStatusCode.NotFound)
    {
    }

    public TrainingNotFoundException(long telegramId) 
        : base(HttpStatusCode.NotFound, $"The user with the ID = {telegramId} was not found.")
    {
    }
}