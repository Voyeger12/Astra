using Astra.Application.Errors;
using Astra.Application.Observability;
using Xunit;

namespace Astra.Application.Tests;

public sealed class OperationalContractsTests
{
    [Fact]
    public void CorrelationIdentifiersAreNonEmptyAndDistinct()
    {
        var first = OperationId.New();
        var second = OperationId.New();

        Assert.NotEqual(Guid.Empty, first.Value);
        Assert.NotEqual(first, second);
        Assert.Equal(32, first.ToString().Length);
    }

    [Fact]
    public void EmptyCorrelationIdentifierIsRejected()
    {
        Assert.Throws<ArgumentException>(() => OperationId.From(Guid.Empty));
        Assert.Throws<ArgumentException>(() => AgentRunId.From(Guid.Empty));
        Assert.Throws<ArgumentException>(() => ProviderCallId.From(Guid.Empty));
    }

    [Theory]
    [InlineData("astra-unexpected")]
    [InlineData("ASTRA_UNEXPECTED")]
    [InlineData("OTHER-UNEXPECTED")]
    public void InvalidErrorCodeIsRejected(string value)
    {
        Assert.Throws<ArgumentException>(() => new AstraErrorCode(value));
    }

    [Fact]
    public void PublicErrorContractDoesNotExposeExceptions()
    {
        var exceptionProperties = typeof(AstraError)
            .GetProperties()
            .Where(property => typeof(Exception).IsAssignableFrom(property.PropertyType));

        Assert.Empty(exceptionProperties);
        Assert.NotEqual(AstraErrorCategory.Cancelled, AstraErrorCategory.Timeout);
    }

    [Fact]
    public void DiagnosticMetadataUsesAnAllowlistAndRedactsRootedPaths()
    {
        var operationId = OperationId.New();
        var metadata = new Dictionary<string, object?>
        {
            ["OperationId"] = operationId,
            ["Component"] = @"C:\Users\Duncan\private.txt",
            ["Prompt"] = "private conversation",
            ["Secret"] = "token"
        };

        var result = DiagnosticMetadataSanitizer.Sanitize(metadata);

        Assert.Equal(operationId.ToString(), result["OperationId"]);
        Assert.Equal("[REDACTED]", result["Component"]);
        Assert.DoesNotContain("Prompt", result.Keys);
        Assert.DoesNotContain("Secret", result.Keys);
    }
}
