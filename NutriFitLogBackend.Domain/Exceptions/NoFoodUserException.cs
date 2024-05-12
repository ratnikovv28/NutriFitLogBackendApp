using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

public class NoFoodUserException : BaseException
{
    public NoFoodUserException() : base(HttpStatusCode.NotFound)
    {
    }

    public NoFoodUserException(long userId, long mealId, long foodId, long dayPartId) 
        : base(HttpStatusCode.NotFound, $"The food with the ID = '{foodId}' for User with Id = '{userId}' was not found and Meal Id = '{mealId}' with DayPart Id = '{dayPartId}' was not found.")
    {
    }
}