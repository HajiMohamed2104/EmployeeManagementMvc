namespace WebApplication1.Exceptions;

public class InvalidEmployeeDataException : EmployeeManagementException
{
    public InvalidEmployeeDataException() : base("Invalid employee data provided")
    {
        ErrorCode = 1002;
    }

    public InvalidEmployeeDataException(string message) : base(message)
    {
        ErrorCode = 1002;
    }

    public InvalidEmployeeDataException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = 1002;
    }
}