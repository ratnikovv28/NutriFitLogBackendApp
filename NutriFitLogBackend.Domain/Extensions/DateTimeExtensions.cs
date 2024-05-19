using System.Diagnostics.CodeAnalysis;

namespace NutriFitLogBackend.Domain.Extensions;

[ExcludeFromCodeCoverage]
public static class DateTimeExtensions
{
    // Расширение для перевода из DateTime в DateOnly.
    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return DateOnly.FromDateTime(dateTime);
    }
    
    // Расширение для перевода из DateOnly в DateTime.
    public static DateTime ToDateTimeUtc(this DateOnly dateOnly)
    {
        var dateTime = dateOnly.ToDateTime(new TimeOnly(0, 0));

        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}