namespace Astra.Infrastructure.Observability;

public sealed record AstraRuntimePaths(string RootDirectory, string LogsDirectory, string TracesDirectory)
{
    public static AstraRuntimePaths CreateDefault()
    {
        var localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return FromRoot(Path.Combine(localApplicationData, "Astra"));
    }

    public static AstraRuntimePaths FromRoot(string rootDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);

        var fullRoot = Path.GetFullPath(rootDirectory);
        return new AstraRuntimePaths(
            fullRoot,
            Path.Combine(fullRoot, "logs"),
            Path.Combine(fullRoot, "traces"));
    }

    public void EnsureCreated()
    {
        Directory.CreateDirectory(RootDirectory);
        Directory.CreateDirectory(LogsDirectory);
        Directory.CreateDirectory(TracesDirectory);
    }
}
