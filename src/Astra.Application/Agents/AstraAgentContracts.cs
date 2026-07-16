using System.Runtime.CompilerServices;
using Astra.Application.Errors;
using Astra.Application.Observability;

namespace Astra.Application.Agents;

public interface IAstraAgent
{
    IAsyncEnumerable<AstraAgentEvent> RunAsync(
        AstraAgentRequest request,
        CancellationToken cancellationToken = default);
}

public sealed record AstraAgentRequest
{
    public const int MaximumUserTextLength = 16_384;

    public AstraAgentRequest(string userText, OperationId operationId, AgentRunId agentRunId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userText);
        if (userText.Length > MaximumUserTextLength)
        {
            throw new ArgumentOutOfRangeException(nameof(userText), $"User text must not exceed {MaximumUserTextLength} characters.");
        }

        UserText = userText;
        OperationId = operationId;
        AgentRunId = agentRunId;
    }

    public string UserText { get; }

    public OperationId OperationId { get; }

    public AgentRunId AgentRunId { get; }
}

public enum AstraAgentRunStatus
{
    Preparing,
    Connecting,
    Generating,
    Cancelling
}

public abstract record AstraAgentEvent(
    OperationId OperationId,
    AgentRunId AgentRunId,
    DateTimeOffset OccurredAt);

public sealed record AstraAgentRunStartedEvent(
    OperationId OperationId,
    AgentRunId AgentRunId,
    DateTimeOffset OccurredAt)
    : AstraAgentEvent(OperationId, AgentRunId, OccurredAt);

public sealed record AstraAgentStatusChangedEvent(
    OperationId OperationId,
    AgentRunId AgentRunId,
    DateTimeOffset OccurredAt,
    AstraAgentRunStatus Status)
    : AstraAgentEvent(OperationId, AgentRunId, OccurredAt);

public sealed record AstraAgentTextDeltaEvent : AstraAgentEvent
{
    public AstraAgentTextDeltaEvent(
        OperationId operationId,
        AgentRunId agentRunId,
        DateTimeOffset occurredAt,
        ProviderCallId providerCallId,
        string delta)
        : base(operationId, agentRunId, occurredAt)
    {
        ArgumentException.ThrowIfNullOrEmpty(delta);
        ProviderCallId = providerCallId;
        Delta = delta;
    }

    public ProviderCallId ProviderCallId { get; }

    public string Delta { get; }
}

public sealed record AstraAgentCompletedEvent(
    OperationId OperationId,
    AgentRunId AgentRunId,
    DateTimeOffset OccurredAt,
    TimeSpan Duration)
    : AstraAgentEvent(OperationId, AgentRunId, OccurredAt);

public sealed record AstraAgentCancelledEvent(
    OperationId OperationId,
    AgentRunId AgentRunId,
    DateTimeOffset OccurredAt)
    : AstraAgentEvent(OperationId, AgentRunId, OccurredAt);

public sealed record AstraAgentFailedEvent(
    OperationId OperationId,
    AgentRunId AgentRunId,
    DateTimeOffset OccurredAt,
    AstraError Error)
    : AstraAgentEvent(OperationId, AgentRunId, OccurredAt);

public static class AstraAgentEventValidationExtensions
{
    public static async IAsyncEnumerable<AstraAgentEvent> ValidateTerminalContract(
        this IAsyncEnumerable<AstraAgentEvent> source,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        var terminalCount = 0;

        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (item is AstraAgentCompletedEvent or AstraAgentCancelledEvent or AstraAgentFailedEvent)
            {
                terminalCount++;
                if (terminalCount > 1)
                {
                    throw new InvalidOperationException("An Astra agent run emitted more than one terminal event.");
                }
            }
            else if (terminalCount != 0)
            {
                throw new InvalidOperationException("An Astra agent run emitted an event after its terminal event.");
            }

            yield return item;
        }

        if (terminalCount != 1)
        {
            throw new InvalidOperationException("An Astra agent run must emit exactly one terminal event.");
        }
    }
}
