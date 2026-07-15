namespace Astra.Application.Errors;

public enum AstraErrorCategory
{
    Validation,
    Cancelled,
    Timeout,
    Unavailable,
    NotFound,
    Conflict,
    Unauthorized,
    Rejected,
    RateLimited,
    Persistence,
    Integrity,
    ToolFailure,
    ProviderFailure,
    Unexpected
}
