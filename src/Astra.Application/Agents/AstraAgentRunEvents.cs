using Astra.Application.Errors;
using Astra.Application.Observability;

namespace Astra.Application.Agents;

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
