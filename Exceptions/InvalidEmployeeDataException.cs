namespace WebApplication1.Exceptions;

/// <summary>
/// Custom exception for invalid employee data
/// Demonstrates validation-specific exception handling
/// </summary>
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