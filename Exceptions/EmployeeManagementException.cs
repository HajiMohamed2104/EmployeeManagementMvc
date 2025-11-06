namespace WebApplication1.Exceptions;

/// <summary>
/// Custom base exception for Employee Management System
/// Demonstrates custom exception creation and inheritance
/// </summary>
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

    // Additional property for error code
    public int ErrorCode { get; set; }
}