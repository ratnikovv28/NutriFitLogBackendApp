using System.Diagnostics.CodeAnalysis;
using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class FoodExistsInMealDayPartException : BaseException
{
    public FoodExistsInMealDayPartException() : base(HttpStatusCode.Conflict)
    {
    }

    public FoodExistsInMealDayPartException(long foodId, long mealId, long dayPartId) 
        : base(HttpStatusCode.Conflict, $"The food with the ID = '{foodId}' already exists in meal with the ID = '{mealId}' and dayPart Id = '{dayPartId}'.")
    {
    }
}