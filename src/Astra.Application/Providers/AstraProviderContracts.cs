using Astra.Application.Errors;

namespace Astra.Application.Providers;

public interface IAstraProviderStatusService
{
    ValueTask<AstraProviderStatus> GetStatusAsync(CancellationToken cancellationToken = default);
}

public enum AstraProviderState
{
    Checking,
    Available,
    ServiceUnavailable,
    ModelUnavailable,
    InvalidConfiguration
}

public sealed record AstraProviderStatus
{
    public AstraProviderStatus(
        AstraProviderState state,
        string providerDisplayName,
        string modelDisplayName,
        DateTimeOffset checkedAt,
        AstraError? error = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(providerDisplayName);
        ArgumentException.ThrowIfNullOrWhiteSpace(modelDisplayName);

        if (state == AstraProviderState.Available && error is not null)
        {
            throw new ArgumentException("An available provider status must not contain an error.", nameof(error));
        }

        if (state is AstraProviderState.ServiceUnavailable or AstraProviderState.ModelUnavailable or AstraProviderState.InvalidConfiguration && error is null)
        {
            throw new ArgumentException("An unavailable provider status must contain a safe error.", nameof(error));
        }

        State = state;
        ProviderDisplayName = providerDisplayName;
        ModelDisplayName = modelDisplayName;
        CheckedAt = checkedAt;
        Error = error;
    }

    public AstraProviderState State { get; }

    public string ProviderDisplayName { get; }

    public string ModelDisplayName { get; }

    public DateTimeOffset CheckedAt { get; }

    public AstraError? Error { get; }
}
