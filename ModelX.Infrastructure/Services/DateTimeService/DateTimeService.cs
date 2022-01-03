using ModelX.Logic.Common.ExternalServices.DateTimeService;

namespace ModelX.Infrastructure.Services.DateTimeService;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}