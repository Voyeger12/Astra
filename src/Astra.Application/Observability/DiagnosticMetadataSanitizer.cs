using System.Collections.ObjectModel;
using Astra.Application.Errors;

namespace Astra.Application.Observability;

public static class DiagnosticMetadataSanitizer
{
    private const int MaximumStringLength = 128;

    private static readonly HashSet<string> AllowedFields = new(StringComparer.Ordinal)
    {
        "AgentRunId",
        "ApprovalDecision",
        "AttachmentCount",
        "Component",
        "DurationMs",
        "ErrorCode",
        "ExceptionType",
        "MessageCount",
        "Model",
        "Operation",
        "OperationId",
        "Outcome",
        "Provider",
        "ProviderCallId",
        "RiskClass"
    };

    public static IReadOnlyDictionary<string, object?> Sanitize(IReadOnlyDictionary<string, object?> metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        var sanitized = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var (name, value) in metadata)
        {
            if (!AllowedFields.Contains(name))
            {
                continue;
            }

            sanitized[name] = SanitizeValue(value);
        }

        return new ReadOnlyDictionary<string, object?>(sanitized);
    }

    private static object? SanitizeValue(object? value) => value switch
    {
        null => null,
        OperationId id => id.ToString(),
        AgentRunId id => id.ToString(),
        ProviderCallId id => id.ToString(),
        AstraErrorCode code => code.ToString(),
        Guid guid => guid.ToString("N"),
        Enum enumeration => enumeration.ToString(),
        bool or byte or sbyte or short or ushort or int or uint or long or ulong or float or double or decimal => value,
        string text => SanitizeText(text),
        _ => "[REDACTED]"
    };

    private static string SanitizeText(string value)
    {
        var singleLine = value.Replace('\r', ' ').Replace('\n', ' ').Trim();
        if (Path.IsPathRooted(singleLine))
        {
            return "[REDACTED]";
        }

        return singleLine.Length <= MaximumStringLength
            ? singleLine
            : string.Concat(singleLine.AsSpan(0, MaximumStringLength), "…");
    }
}
