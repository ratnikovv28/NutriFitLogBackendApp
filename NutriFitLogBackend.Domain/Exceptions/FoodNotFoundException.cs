using System.Diagnostics.CodeAnalysis;
using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class FoodNotFoundException : BaseException
{
    public FoodNotFoundException() : base(HttpStatusCode.NotFound)
    {
    }

    public FoodNotFoundException(long foodId) 
        : base(HttpStatusCode.NotFound, $"The food with the ID = {foodId} was not found.")
    {
    }
}