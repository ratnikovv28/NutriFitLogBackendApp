using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

public class NoSetUserException : BaseException
{
    public NoSetUserException() : base(HttpStatusCode.NotFound)
    {
    }

    public NoSetUserException(long userId, long setId) 
        : base(HttpStatusCode.NotFound, $"The set with the ID = '{setId}' for User with Id = '{userId}' was not found.")
    {
    }
}