using Astra.Application.Errors;
using Astra.Application.Observability;

namespace Astra.Application.Agents;

public interface IAstraAgent
{
    IAsyncEnumerable<AstraAgentEvent> RunAsync(AstraAgentRequest request, CancellationToken cancellationToken);
}

public sealed record AstraAgentRequest
{
    public const int MaximumTextLength = 16_384;

    public AstraAgentRequest(string userText, OperationId operationId, AgentRunId agentRunId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userText);
        if (userText.Length > MaximumTextLength)
        {
            throw new ArgumentOutOfRangeException(nameof(userText), $"User text must not exceed {MaximumTextLength} characters.");
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

public abstract record AstraAgentEvent(OperationId OperationId, AgentRunId AgentRunId, DateTimeOffset OccurredAt)
{
    public bool IsTerminal => this is AstraAgentCompletedEvent or AstraAgentCancelledEvent or AstraAgentFailedEvent;
}

public sealed record AstraAgentRunStartedEvent(OperationId OperationId, AgentRunId AgentRunId, DateTimeOffset OccurredAt)
    : AstraAgentEvent(OperationId, AgentRunId, OccurredAt);

public sealed record AstraAgentStatusChangedEvent(OperationId OperationId, AgentRunId AgentRunId, DateTimeOffset OccurredAt, AstraAgentRunStatus Status)
    : AstraAgentEvent(OperationId, AgentRunId, OccurredAt);

public sealed record AstraAgentTextDeltaEvent : AstraAgentEvent
{
    public AstraAgentTextDeltaEvent(OperationId operationId, AgentRunId agentRunId, DateTimeOffset occurredAt, string delta)
        : base(operationId, agentRunId, occurredAt)
    {
        ArgumentException.ThrowIfNullOrEmpty(delta);
        Delta = delta;
    }

    public string Delta { get; }
}

public sealed record AstraAgentCompletedEvent(OperationId OperationId, AgentRunId AgentRunId, DateTimeOffset OccurredAt, TimeSpan Duration)
    : AstraAgentEvent(OperationId, AgentRunId, OccurredAt);

public sealed record AstraAgentCancelledEvent(OperationId OperationId, AgentRunId AgentRunId, DateTimeOffset OccurredAt)
    : AstraAgentEvent(OperationId, AgentRunId, OccurredAt);

public sealed record AstraAgentFailedEvent(OperationId OperationId, AgentRunId AgentRunId, DateTimeOffset OccurredAt, AstraError Error)
    : AstraAgentEvent(OperationId, AgentRunId, OccurredAt);

public static class AstraAgentEventStream
{
    public static async Task<IReadOnlyList<AstraAgentEvent>> CollectAndValidateAsync(IAsyncEnumerable<AstraAgentEvent> events, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(events);

        var collected = new List<AstraAgentEvent>();
        await foreach (var item in events.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            collected.Add(item);
        }

        if (collected.Count == 0 || collected.Count(item => item.IsTerminal) != 1 || !collected[^1].IsTerminal)
        {
            throw new InvalidOperationException("An Astra agent event stream must end with exactly one terminal event.");
        }

        return collected;
    }
}
