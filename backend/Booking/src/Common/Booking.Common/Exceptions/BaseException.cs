namespace Booking.Common.Exceptions;

public class BaseException : Exception
{
    public BaseException(string message, int statusCode = 500)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }
}