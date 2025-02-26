namespace Ambev.Dev.Test.Domain.Exceptions;

public class CustomException : Exception
{
    public CustomException(string message) : base(message) 
    {
    }

    public CustomException(Exception ex, string message) : base(message, ex)
    {
    }
}
