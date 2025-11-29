using Shared.Exceptions.Common;

namespace Shared.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException() : base("Resource not found.", 404)
    {
    }
    public NotFoundException(string message) : base(message, 404)
    {
    }
    public NotFoundException(string message, Exception innerException) : base(message, innerException, 404)
    {
    }
}