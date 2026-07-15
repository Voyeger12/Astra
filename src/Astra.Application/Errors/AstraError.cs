using Astra.Application.Observability;

namespace Astra.Application.Errors;

public sealed record AstraError(
    AstraErrorCode Code,
    AstraErrorCategory Category,
    string Message,
    bool IsRetryable,
    OperationId? OperationId = null,
    string? SuggestedAction = null)
{
    public AstraError
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Message);
    }
}
