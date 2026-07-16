using Astra.Application.Observability;

namespace Astra.Application.Errors;

public sealed record AstraError
{
    public AstraError(
        AstraErrorCode code,
        AstraErrorCategory category,
        string message,
        bool isRetryable,
        OperationId? operationId = null,
        string? suggestedAction = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        Code = code;
        Category = category;
        Message = message;
        IsRetryable = isRetryable;
        OperationId = operationId;
        SuggestedAction = suggestedAction;
    }

    public AstraErrorCode Code { get; }

    public AstraErrorCategory Category { get; }

    public string Message { get; }

    public bool IsRetryable { get; }

    public OperationId? OperationId { get; }

    public string? SuggestedAction { get; }
}
