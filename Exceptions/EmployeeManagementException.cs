namespace WebApplication1.Exceptions;

public class EmployeeManagementException : Exception
{
    public EmployeeManagementException() : base()
    {
    }

    public EmployeeManagementException(string message) : base(message)
    {
    }

    public EmployeeManagementException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public int ErrorCode { get; set; }
}