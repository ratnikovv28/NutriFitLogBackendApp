using System.Diagnostics.CodeAnalysis;
using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class NoExerciseUserException : BaseException
{
    public NoExerciseUserException() : base(HttpStatusCode.NotFound)
    {
    }

    public NoExerciseUserException(long userId, long exerciseId) 
        : base(HttpStatusCode.NotFound, $"The exercise with the ID = '{exerciseId}' for User with Id = '{userId}' was not found.")
    {
    }
}