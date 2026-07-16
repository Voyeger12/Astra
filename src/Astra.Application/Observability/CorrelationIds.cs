namespace Astra.Application.Observability;

public readonly record struct OperationId
{
    private OperationId(Guid value) => Value = value;

    public Guid Value { get; }

    public static OperationId New() => new(Guid.NewGuid());

    public static OperationId From(Guid value) => value == Guid.Empty
        ? throw new ArgumentException("Operation ID must not be empty.", nameof(value))
        : new(value);

    public override string ToString() => Value.ToString("N");
}

public readonly record struct AgentRunId
{
    private AgentRunId(Guid value) => Value = value;

    public Guid Value { get; }

    public static AgentRunId New() => new(Guid.NewGuid());

    public static AgentRunId From(Guid value) => value == Guid.Empty
        ? throw new ArgumentException("Agent run ID must not be empty.", nameof(value))
        : new(value);

    public override string ToString() => Value.ToString("N");
}

public readonly record struct ProviderCallId
{
    private ProviderCallId(Guid value) => Value = value;

    public Guid Value { get; }

    public static ProviderCallId New() => new(Guid.NewGuid());

    public static ProviderCallId From(Guid value) => value == Guid.Empty
        ? throw new ArgumentException("Provider call ID must not be empty.", nameof(value))
        : new(value);

    public override string ToString() => Value.ToString("N");
}
