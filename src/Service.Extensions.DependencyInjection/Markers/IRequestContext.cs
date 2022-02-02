using System;

namespace Service.Extensions.DependencyInjection.Markers;

public interface IRequestContext {
    public DateTime RequestedDatetime { get; init; }

    public static T CreateRequestContext<T>(string timezoneId) where T : class, IRequestContext, new() =>
        new() { RequestedDatetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timezoneId)) };
}
