using System.Runtime.CompilerServices;
using Astra.Application.Agents;
using Astra.Application.Errors;
using Astra.Application.Observability;
using Astra.Application.Providers;
using Xunit;

namespace Astra.Application.Tests;

public sealed class AgentContractsTests
{
    [Fact]
    public void RequestRejectsEmptyAndOversizedUserText()
    {
        var operationId = OperationId.New();
        var runId = AgentRunId.New();

        Assert.Throws<ArgumentException>(() => new AstraAgentRequest(" ", operationId, runId));
        Assert.Throws<ArgumentOutOfRangeException>(() => new AstraAgentRequest(
            new string('x', AstraAgentRequest.MaximumUserTextLength + 1), operationId, runId));
    }

    [Fact]
    public void RequestPreservesOriginalTextAndCorrelationIdentifiers()
    {
        var operationId = OperationId.New();
        var runId = AgentRunId.New();
        var request = new AstraAgentRequest("  Hallo Astra  ", operationId, runId);

        Assert.Equal("  Hallo Astra  ", request.UserText);
        Assert.Equal(operationId, request.OperationId);
        Assert.Equal(runId, request.AgentRunId);
    }

    [Fact]
    public void TextDeltaRejectsEmptyContent()
    {
        Assert.Throws<ArgumentException>(() => new AstraAgentTextDeltaEvent(
            OperationId.New(), AgentRunId.New(), DateTimeOffset.UtcNow, ProviderCallId.New(), string.Empty));
    }

    [Fact]
    public async Task EventStreamAcceptsExactlyOneTerminalEvent()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var operationId = OperationId.New();
        var runId = AgentRunId.New();
        var events = GetEvents(
            new AstraAgentRunStartedEvent(operationId, runId, DateTimeOffset.UtcNow),
            new AstraAgentCompletedEvent(operationId, runId, DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1)),
            cancellationToken);

        var result = new List<AstraAgentEvent>();
        await foreach (var item in events.ValidateTerminalContract(cancellationToken))
        {
            result.Add(item);
        }

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task EventStreamRejectsMissingOrRepeatedTerminalEvents()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var operationId = OperationId.New();
        var runId = AgentRunId.New();

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await foreach (var _ in GetEvents(
                                   new AstraAgentRunStartedEvent(operationId, runId, DateTimeOffset.UtcNow),
                                   cancellationToken: cancellationToken)
                               .ValidateTerminalContract(cancellationToken))
            {
            }
        });

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await foreach (var _ in GetEvents(
                                   new AstraAgentCompletedEvent(operationId, runId, DateTimeOffset.UtcNow, TimeSpan.Zero),
                                   new AstraAgentCancelledEvent(operationId, runId, DateTimeOffset.UtcNow),
                                   cancellationToken)
                               .ValidateTerminalContract(cancellationToken))
            {
            }
        });
    }

    [Fact]
    public void ProviderStatusRequiresSafeErrorForUnavailableState()
    {
        Assert.Throws<ArgumentException>(() => new AstraProviderStatus(
            AstraProviderState.ServiceUnavailable, "Ollama", "qwen3:4b", DateTimeOffset.UtcNow));

        var error = new AstraError(
            AstraErrorCode.AgentProviderUnavailable,
            AstraErrorCategory.Unavailable,
            "Ollama ist nicht erreichbar.",
            true);

        var status = new AstraProviderStatus(
            AstraProviderState.ServiceUnavailable, "Ollama", "qwen3:4b", DateTimeOffset.UtcNow, error);

        Assert.Equal(error, status.Error);
    }

    private static async IAsyncEnumerable<AstraAgentEvent> GetEvents(
        AstraAgentEvent first,
        AstraAgentEvent? second = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        yield return first;
        if (second is not null)
        {
            await Task.Yield();
            yield return second;
        }
    }
}
