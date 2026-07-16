using Astra.Infrastructure.Observability;
using Serilog.Events;
using Xunit;

namespace Astra.Infrastructure.Tests;

public sealed class OperationalInfrastructureTests
{
    [Fact]
    public void RuntimePathsRemainInsideConfiguredRoot()
    {
        var root = Path.Combine(Path.GetTempPath(), "astra-tests", Guid.NewGuid().ToString("N"));
        var paths = AstraRuntimePaths.FromRoot(root);

        Assert.Equal(Path.GetFullPath(root), paths.RootDirectory);
        Assert.Equal(Path.Combine(paths.RootDirectory, "logs"), paths.LogsDirectory);
        Assert.Equal(Path.Combine(paths.RootDirectory, "traces"), paths.TracesDirectory);
    }

    [Fact]
    public void LoggerCreatesCompactLocalLogFile()
    {
        var root = Path.Combine(Path.GetTempPath(), "astra-tests", Guid.NewGuid().ToString("N"));
        var paths = AstraRuntimePaths.FromRoot(root);

        try
        {
            using (var logger = AstraOperationalKernelExtensions.CreateLogger(paths))
            {
                logger.Write(LogEventLevel.Information, "Operational kernel test event {Outcome}", "Success");
            }

            var logFile = Assert.Single(Directory.GetFiles(paths.LogsDirectory, "astra-*.clef"));
            var content = File.ReadAllText(logFile);
            Assert.Contains("Operational kernel test event", content, StringComparison.Ordinal);
            Assert.Contains("Success", content, StringComparison.Ordinal);
            Assert.True(Directory.Exists(paths.TracesDirectory));
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }
}
