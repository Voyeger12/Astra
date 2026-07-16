using System.Runtime.CompilerServices;

namespace Astra.Application.Agents;

public static class AstraAgentEventValidationExtensions
{
    public static async IAsyncEnumerable<AstraAgentEvent> ValidateTerminalContract(
        this IAsyncEnumerable<AstraAgentEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(events);
        var terminalSeen = false;

        await foreach (var item in events.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (terminalSeen)
            {
                throw new InvalidOperationException("An Astra agent event sequence must not continue after a terminal event.");
            }

            if (item.IsTerminal)
            {
                terminalSeen = true;
            }

            yield return item;
        }

        if (!terminalSeen)
        {
            throw new InvalidOperationException("An Astra agent event sequence must end with exactly one terminal event.");
        }
    }
}
