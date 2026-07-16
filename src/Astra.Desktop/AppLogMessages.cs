using Microsoft.Extensions.Logging;

namespace Astra.Desktop;

internal static partial class AppLogMessages
{
    [LoggerMessage(EventId = 1001, Level = LogLevel.Information, Message = "Astra application host started")]
    public static partial void ApplicationStarted(ILogger logger);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Information, Message = "Astra application host is stopping")]
    public static partial void ApplicationStopping(ILogger logger);

    [LoggerMessage(EventId = 1003, Level = LogLevel.Information, Message = "Astra application host stopped")]
    public static partial void ApplicationStopped(ILogger logger);

    [LoggerMessage(EventId = 1901, Level = LogLevel.Error, Message = "Astra application host failed during shutdown")]
    public static partial void ApplicationShutdownFailed(ILogger logger, Exception exception);

    [LoggerMessage(EventId = 1999, Level = LogLevel.Critical, Message = "Unhandled dispatcher exception")]
    public static partial void DispatcherUnhandledException(ILogger logger, Exception exception);

    [LoggerMessage(EventId = 1999, Level = LogLevel.Critical, Message = "Unhandled process exception")]
    public static partial void ProcessUnhandledException(ILogger logger, Exception exception);
}
