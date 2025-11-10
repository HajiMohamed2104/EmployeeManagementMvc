using WebApplication1.Models.Entities;

namespace WebApplication1.Repositories;


public interface IEmployeeRepository : IRepository<Employee>
{
    Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
    Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string department);
    Task<IEnumerable<Employee>> GetEmployeesBySalaryRangeAsync(decimal minSalary, decimal maxSalary);
    Task<IEnumerable<Employee>> GetEmployeesHiredAfterAsync(DateTime date);
    Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm);
    
    Task<Dictionary<string, int>> GetEmployeeCountByDepartmentAsync();
    Task<decimal> GetAverageSalaryByDepartmentAsync(string department);
    Task<IEnumerable<Employee>> GetTopEarnersAsync(int count);
    
    Task<Employee?> GetByEmployeeNumberAsync(string employeeNumber);
    Task<bool> IsEmployeeNumberUniqueAsync(string employeeNumber, int? excludeId = null);
    Task<IEnumerable<Employee>> GetEmployeesEligibleForPromotionAsync();
}