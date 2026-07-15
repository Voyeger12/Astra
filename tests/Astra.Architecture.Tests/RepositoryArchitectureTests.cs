using System.Xml.Linq;
using Xunit;

namespace Astra.Architecture.Tests;

public sealed class RepositoryArchitectureTests
{
    private static readonly string Root = FindRepositoryRoot();

    [Fact]
    public void SolutionContainsExpectedProjects()
    {
        var solution = XDocument.Load(Path.Combine(Root, "Astra.slnx"));
        var actual = solution.Descendants("Project")
            .Select(x => (string?)x.Attribute("Path"))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => Normalize(x!))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var expected = new[]
        {
            "src/Astra.Application/Astra.Application.csproj",
            "src/Astra.AgentFramework/Astra.AgentFramework.csproj",
            "src/Astra.Desktop/Astra.Desktop.csproj",
            "src/Astra.Domain/Astra.Domain.csproj",
            "src/Astra.Infrastructure/Astra.Infrastructure.csproj",
            "src/Astra.Presentation/Astra.Presentation.csproj",
            "src/Astra.Tools/Astra.Tools.csproj",
            "tests/Astra.Architecture.Tests/Astra.Architecture.Tests.csproj"
        };

        Assert.Equal(expected.Order(), actual.Order());
    }

    [Fact]
    public void ProjectReferencesFollowAllowedGraph()
    {
        var allowed = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["Astra.Domain"] = [],
            ["Astra.Application"] = ["Astra.Domain"],
            ["Astra.AgentFramework"] = ["Astra.Application", "Astra.Domain"],
            ["Astra.Infrastructure"] = ["Astra.Application", "Astra.Domain"],
            ["Astra.Tools"] = ["Astra.Application", "Astra.Domain"],
            ["Astra.Presentation"] = ["Astra.Application"],
            ["Astra.Desktop"] = ["Astra.Presentation", "Astra.AgentFramework", "Astra.Infrastructure", "Astra.Tools"],
            ["Astra.Architecture.Tests"] = []
        };

        foreach (var project in FindProjects())
        {
            var name = Path.GetFileNameWithoutExtension(project);
            var actual = XDocument.Load(project).Descendants("ProjectReference")
                .Select(x => (string?)x.Attribute("Include"))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => Path.GetFileNameWithoutExtension(x!))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            Assert.True(allowed.TryGetValue(name, out var expected), $"Unexpected project: {name}");
            Assert.Equal(expected!.Order(), actual.Order());
        }
    }

    [Fact]
    public void OnlyDesktopAndPresentationAreWindowsWpfProjects()
    {
        foreach (var project in FindProjects())
        {
            var name = Path.GetFileNameWithoutExtension(project);
            var document = XDocument.Load(project);
            var target = document.Descendants("TargetFramework").Select(x => x.Value).Single();
            var useWpf = document.Descendants("UseWPF").Select(x => x.Value).SingleOrDefault();
            var shouldUseWindows = name is "Astra.Desktop" or "Astra.Presentation";

            Assert.Equal(shouldUseWindows, target.EndsWith("-windows", StringComparison.OrdinalIgnoreCase));
            Assert.Equal(shouldUseWindows, string.Equals(useWpf, "true", StringComparison.OrdinalIgnoreCase));
        }
    }

    [Fact]
    public void RepositoryDoesNotUseLatestOrPreviewLanguageVersion()
    {
        foreach (var file in Directory.EnumerateFiles(Root, "*.props", SearchOption.TopDirectoryOnly)
                     .Concat(FindProjects()))
        {
            var values = XDocument.Load(file).Descendants("LangVersion").Select(x => x.Value);
            Assert.DoesNotContain(values, value => value.Contains("latest", StringComparison.OrdinalIgnoreCase)
                || value.Contains("preview", StringComparison.OrdinalIgnoreCase));
        }
    }

    [Fact]
    public void PackageVersionsAreCentrallyManaged()
    {
        foreach (var project in FindProjects())
        {
            var versionedReferences = XDocument.Load(project).Descendants("PackageReference")
                .Where(x => x.Attribute("Version") is not null || x.Element("Version") is not null);
            Assert.Empty(versionedReferences);
        }
    }

    [Fact]
    public void NoTemplatePlaceholderFilesExist()
    {
        var forbidden = Directory.EnumerateFiles(Root, "*", SearchOption.AllDirectories)
            .Where(path => Path.GetFileName(path) is "Class1.cs" or "UnitTest1.cs");
        Assert.Empty(forbidden);
    }

    [Fact]
    public void ScriptsReferenceModernSolutionFormat()
    {
        foreach (var script in Directory.EnumerateFiles(Path.Combine(Root, "scripts"), "*.ps1"))
        {
            var content = File.ReadAllText(script);
            Assert.DoesNotContain("Astra.sln'", content, StringComparison.Ordinal);
            Assert.Contains("Astra.slnx", content, StringComparison.Ordinal);
        }
    }

    private static IEnumerable<string> FindProjects() =>
        Directory.EnumerateFiles(Path.Combine(Root, "src"), "*.csproj", SearchOption.AllDirectories)
            .Concat(Directory.EnumerateFiles(Path.Combine(Root, "tests"), "*.csproj", SearchOption.AllDirectories));

    private static string Normalize(string path) => path.Replace('\\', '/');

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Astra.slnx")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new DirectoryNotFoundException("Repository root not found.");
    }
}
