using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

public class TrainingNotFoundException : BaseException
{
    public TrainingNotFoundException() : base(HttpStatusCode.NotFound)
    {
    }

    public TrainingNotFoundException(long trainingId) 
        : base(HttpStatusCode.NotFound, $"The training with the ID = {trainingId} was not found.")
    {
    }
}