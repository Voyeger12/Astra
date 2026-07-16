using Microsoft.Extensions.Logging;

namespace Astra.Infrastructure.Observability;

public static class AstraLogEventIds
{
    public static readonly EventId ApplicationStarting = new(1000, nameof(ApplicationStarting));
    public static readonly EventId ApplicationStarted = new(1001, nameof(ApplicationStarted));
    public static readonly EventId ApplicationStopping = new(1002, nameof(ApplicationStopping));
    public static readonly EventId ApplicationStopped = new(1003, nameof(ApplicationStopped));
    public static readonly EventId ApplicationStartupFailed = new(1900, nameof(ApplicationStartupFailed));
    public static readonly EventId ApplicationShutdownFailed = new(1901, nameof(ApplicationShutdownFailed));
    public static readonly EventId UnhandledException = new(1999, nameof(UnhandledException));
}
