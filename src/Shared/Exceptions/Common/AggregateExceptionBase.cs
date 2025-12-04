namespace Shared.Exceptions.Common;

public abstract class AggregateExceptionBase(
    IEnumerable<string> errors,
    int statusCode,
    string message = "One or more errors occurred.")
    : BaseException(message, statusCode)
{
    public IReadOnlyCollection<string> Errors { get; } = errors.ToList().AsReadOnly();
}
