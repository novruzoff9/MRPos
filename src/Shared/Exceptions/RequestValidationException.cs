using Shared.Exceptions.Common;

namespace Shared.Exceptions;

public class RequestValidationException(IEnumerable<string> errors, string? request = null) 
    : AggregateExceptionBase(errors, 400, "Validation failed")
{
    public string Request { get; } = request ?? string.Empty;
}
