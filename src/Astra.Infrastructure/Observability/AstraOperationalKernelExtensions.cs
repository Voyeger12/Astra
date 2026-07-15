using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Astra.Infrastructure.Observability;

public static class AstraOperationalKernelExtensions
{
    public const long FileSizeLimitBytes = 10 * 1024 * 1024;
    public const int RetainedFileCountLimit = 14;

    public static IHostApplicationBuilder AddAstraOperationalKernel(
        this IHostApplicationBuilder builder,
        AstraRuntimePaths runtimePaths,
        Serilog.ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(runtimePaths);
        ArgumentNullException.ThrowIfNull(logger);

        builder.Services.AddSingleton(runtimePaths);
        builder.Services.AddSerilog(logger, dispose: false);
        return builder;
    }

    public static Logger CreateLogger(AstraRuntimePaths runtimePaths)
    {
        ArgumentNullException.ThrowIfNull(runtimePaths);
        runtimePaths.EnsureCreated();

        var logPath = Path.Combine(runtimePaths.LogsDirectory, "astra-.clef");
        return new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "Astra")
            .WriteTo.File(
                new CompactJsonFormatter(),
                logPath,
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: FileSizeLimitBytes,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: RetainedFileCountLimit,
                buffered: false,
                shared: false)
            .CreateLogger();
    }
}
