using WebApplication1.Models.Entities;

namespace WebApplication1.Repositories;

/// <summary>
/// Employee-specific repository interface demonstrating specialized operations
/// </summary>
public interface IEmployeeRepository : IRepository<Employee>
{
    // Employee-specific methods demonstrating LINQ and business logic
    Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
    Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string department);
    Task<IEnumerable<Employee>> GetEmployeesBySalaryRangeAsync(decimal minSalary, decimal maxSalary);
    Task<IEnumerable<Employee>> GetEmployeesHiredAfterAsync(DateTime date);
    Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm);
    
    // Methods demonstrating complex LINQ operations
    Task<Dictionary<string, int>> GetEmployeeCountByDepartmentAsync();
    Task<decimal> GetAverageSalaryByDepartmentAsync(string department);
    Task<IEnumerable<Employee>> GetTopEarnersAsync(int count);
    
    // Methods demonstrating async/await with exception handling
    Task<Employee> GetByEmployeeNumberAsync(string employeeNumber);
    Task<bool> IsEmployeeNumberUniqueAsync(string employeeNumber, int? excludeId = null);
    Task<IEnumerable<Employee>> GetEmployeesEligibleForPromotionAsync();
}