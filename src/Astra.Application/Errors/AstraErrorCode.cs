namespace Astra.Application.Errors;

public readonly record struct AstraErrorCode
{
    private const string Prefix = "ASTRA-";

    public AstraErrorCode(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (!value.StartsWith(Prefix, StringComparison.Ordinal)
            || value.Any(character => character is not (>= 'A' and <= 'Z') and not (>= '0' and <= '9') and not '-'))
        {
            throw new ArgumentException("Astra error codes must use uppercase letters, digits and hyphens and start with ASTRA-.", nameof(value));
        }

        Value = value;
    }

    public string Value { get; }

    public static AstraErrorCode Unexpected { get; } = new("ASTRA-UNEXPECTED");

    public static AstraErrorCode AgentProviderUnavailable { get; } = new("ASTRA-AGENT-PROVIDER-UNAVAILABLE");

    public static AstraErrorCode AgentTimeout { get; } = new("ASTRA-AGENT-TIMEOUT");

    public override string ToString() => Value;
}
