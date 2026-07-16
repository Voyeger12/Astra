using System.Runtime.CompilerServices;
using Astra.Application.Errors;
using Astra.Application.Observability;

namespace Astra.Application.Agents;

public interface IAstraAgent
{
    IAsyncEnumerable<AstraAgentEvent> RunAsync(AstraAgentRequest request, CancellationToken cancellationToken);
}

public sealed record AstraAgentRequest
{
    public const int MaximumUserTextLength = 16_384;

    public AstraAgentRequest(string userText, OperationId operationId, AgentRunId agentRunId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userText);
        if (userText.Length > MaximumUserTextLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(userText),
                $"User text must not exceed {MaximumUserTextLength} characters.");
        }

        UserText = userText;
        OperationId = operationId;
        Agent