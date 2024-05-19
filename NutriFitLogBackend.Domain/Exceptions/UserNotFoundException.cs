using System.Diagnostics.CodeAnalysis;
using NutriFitLogBackend.Domain.Common;

namespace NutriFitLogBackend.Domain.Exceptions;

[ExcludeFromCodeCoverage]
public class UserNotFoundException : BaseException
{
    public UserNotFoundException() : base(HttpStatusCode.NotFound)
    {
    }

    public UserNotFoundException(long telegramId) 
        : base(HttpStatusCode.NotFound, $"The user with the ID = {telegramId} was not found.")
    {
    }
}
