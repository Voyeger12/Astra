using System.Windows;
using System.Windows.Threading;
using Astra.Infrastructure.Observability;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Astra.Desktop;

public partial class App : Application
{
    private IHost? host;
    private ILogger<App>? logger;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;
        DispatcherUnhandledException += HandleDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += HandleDomainUnhandledException;

        try
        {
            var runtimePaths = AstraRuntimePaths.CreateDefault();
            Log.Logger = AstraOperationalKernelExtensions.CreateLogger(runtimePaths);
            Log.Information("Astra application host is starting");

            var builder = Host.CreateApplicationBuilder(e.Args);
            builder.AddAstraOperationalKernel(runtimePaths, Log.Logger);
            host = builder.Build();
            await host.StartAsync().ConfigureAwait(true);

            logger = host.Services.GetRequiredService<ILogger<App>>();
            logger.LogInformation(AstraLogEventIds.ApplicationStarted, "Astra application host started");
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Astra application host failed during startup");
            Shutdown(-1);
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        try
        {
            logger?.LogInformation(AstraLogEventIds.ApplicationStopping, "Astra application host is stopping");
            if (host is not null)
            {
                using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                await host.StopAsync(timeout.Token).ConfigureAwait(true);
                logger?.LogInformation(AstraLogEventIds.ApplicationStopped, "Astra application host stopped");
            }
        }
        catch (Exception exception)
        {
            if (logger is not null)
            {
                logger.LogError(AstraLogEventIds.ApplicationShutdownFailed, exception, "Astra application host failed during shutdown");
            }
            else
            {
                Log.Error(exception, "Astra application host failed during shutdown");
            }
        }
        finally
        {
            host?.Dispose();
            await Log.CloseAndFlushAsync().ConfigureAwait(true);
            base.OnExit(e);
        }
    }

    private void HandleDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        if (logger is not null)
        {
            logger.LogCritical(AstraLogEventIds.UnhandledException, e.Exception, "Unhandled dispatcher exception");
        }
        else
        {
            Log.Fatal(e.Exception, "Unhandled dispatcher exception before host logging was available");
        }

        e.Handled = true;
        Shutdown(-1);
    }

    private void HandleDomainUnhandledException(object? sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is not Exception exception)
        {
            return;
        }

        if (logger is not null)
        {
            logger.LogCritical(AstraLogEventIds.UnhandledException, exception, "Unhandled process exception");
        }
        else
        {
            Log.Fatal(exception, "Unhandled process exception before host logging was available");
        }
    }
}
