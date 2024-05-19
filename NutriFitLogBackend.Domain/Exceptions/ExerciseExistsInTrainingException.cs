using System.Diagnostics.CodeAnalysis;
using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class ExerciseExistsInTrainingException : BaseException
{
    public ExerciseExistsInTrainingException() : base(HttpStatusCode.Conflict)
    {
    }

    public ExerciseExistsInTrainingException(long exerciseId, long trainingId) 
        : base(HttpStatusCode.Conflict, $"The exercise with the ID = '{exerciseId}' already exists in training with the ID = '{trainingId}'.")
    {
    }
}