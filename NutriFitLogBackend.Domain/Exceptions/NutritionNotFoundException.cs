using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

public class NutritionNotFoundException : BaseException
{
    public NutritionNotFoundException() : base(HttpStatusCode.NotFound)
    {
    }

    public NutritionNotFoundException(long telegramId) 
        : base(HttpStatusCode.NotFound, $"The user with the ID = {telegramId} was not found.")
    {
    }
}