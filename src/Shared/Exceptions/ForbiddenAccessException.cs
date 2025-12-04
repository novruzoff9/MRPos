using Shared.Exceptions.Common;

namespace Shared.Exceptions;

public class ForbiddenAccessException : BaseException
{
    public ForbiddenAccessException() : base("You do not have access to this resource.", 403)
    {
    }
    public ForbiddenAccessException(string message) : base(message, 403)
    {
    }
    public ForbiddenAccessException(string message, Exception innerException) : base(message, innerException, 403)
    {
    }
}
