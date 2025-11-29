using System.Text.Json.Serialization;

namespace Shared.ResultTypes;

public class Response<T>
{
    public T Data { get; private set; }
    public bool IsSuccess { get; private set; }
    public int StatusCode { get; private set; }
    public IReadOnlyList<string> Errors { get; private set; }

    private Response() { }

    [JsonConstructor]
    private Response(T data, bool isSuccess, int statusCode, IReadOnlyList<string> errors)
    {
        Data = data;
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Errors = errors;
    }

    public static Response<T> Success(T data, int statusCode)
    {
        return new Response<T>
        {
            Data = data,
            IsSuccess = true,
            StatusCode = statusCode
        };
    }

    public static Response<T> Success(int statusCode)
    {
        return new Response<T>
        {
            IsSuccess = true,
            StatusCode = statusCode
        };
    }
    public static Response<T> Fail(IEnumerable<string> errors, int statusCode)
    {
        return new Response<T>
        {
            IsSuccess = false,
            Errors = [.. errors],
            StatusCode = statusCode
        };
    }

    public static Response<T> Fail(string error, int statusCode)
    {
        return new Response<T>
        {
            IsSuccess = false,
            Errors = [error],
            StatusCode = statusCode
        };
    }
}