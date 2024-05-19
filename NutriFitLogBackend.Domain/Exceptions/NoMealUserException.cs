using System.Diagnostics.CodeAnalysis;
using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class NoMealUserException : BaseException
{
    public NoMealUserException() : base(HttpStatusCode.NotFound)
    {
    }

    public NoMealUserException(long userId, long mealId) 
        : base(HttpStatusCode.NotFound, $"The meal with the ID = '{mealId}' for User with Id = '{userId}' was not found.")
    {
    }
}