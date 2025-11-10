namespace WebApplication1.Exceptions;


public class EmployeeNotFoundException : EmployeeManagementException
{
    public EmployeeNotFoundException() : base("Employee not found")
    {
        ErrorCode = 1001;
    }

    public EmployeeNotFoundException(int employeeId) : base($"Employee with ID {employeeId} was not found")
    {
        ErrorCode = 1001;
    }

    public EmployeeNotFoundException(string message) : base(message)
    {
        ErrorCode = 1001;
    }

    public EmployeeNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = 1001;
    }
}