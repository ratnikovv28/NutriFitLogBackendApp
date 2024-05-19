using System.Diagnostics.CodeAnalysis;
using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class ExerciseNotFoundException : BaseException
{
    public ExerciseNotFoundException() : base(HttpStatusCode.NotFound)
    {
    }

    public ExerciseNotFoundException(long exerciseId) 
        : base(HttpStatusCode.NotFound, $"The exercise with the ID = {exerciseId} was not found.")
    {
    }
}