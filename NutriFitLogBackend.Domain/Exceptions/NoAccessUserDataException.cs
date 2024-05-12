using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

public class NoAccessUserDataException : BaseException
{
    public NoAccessUserDataException() : base(HttpStatusCode.Forbidden)
    {
        
    }
    
    public NoAccessUserDataException(long userId, long trainerId) 
        : base(HttpStatusCode.Forbidden, $"Trainer with Id = '{trainerId}' has no access to User data with Id = '{userId}'")
    {
        
    }
}