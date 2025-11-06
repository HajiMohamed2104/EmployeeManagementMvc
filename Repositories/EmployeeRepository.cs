using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
using WebApplication1.Exceptions;

namespace WebApplication1.Repositories;

/// <summary>
/// Employee-specific repository implementation demonstrating LINQ, async/await, and business logic
/// </summary>
public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Get active employees using LINQ
    /// </summary>
    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
    {
        try
        {
            return await _dbSet
                .Where(e => e.IsActive)
                .Include(e => e.DepartmentNavigation)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException("Error retrieving active employees", ex);
        }
    }

    /// <summary>
    /// Get employees by department using LINQ
    /// </summary>
    public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string department)
    {
        try
        {
            return await _dbSet
                .Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
                .Include(e => e.DepartmentNavigation)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException($"Error retrieving employees for department {department}", ex);
        }
    }

    /// <summary>
    /// Get employees by salary range using LINQ
    /// </summary>
    public async Task<IEnumerable<Employee>> GetEmployeesBySalaryRangeAsync(decimal minSalary, decimal maxSalary)
    {
        try
        {
            return await _dbSet
                .Where(e => e.Salary >= minSalary && e.Salary <= maxSalary)
                .Include(e => e.DepartmentNavigation)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException($"Error retrieving employees in salary range {minSalary}-{maxSalary}", ex);
        }
    }

    /// <summary>
    /// Get employees hired after a specific date using LINQ
    /// </summary>
    public async Task<IEnumerable<Employee>> GetEmployeesHiredAfterAsync(DateTime date)
    {
        try
        {
            return await _dbSet
                .Where(e => e.HireDate > date)
                .Include(e => e.DepartmentNavigation)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException($"Error retrieving employees hired after {date}", ex);
        }
    }

    /// <summary>
    /// Search employees using LINQ with multiple criteria
    /// </summary>
    public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm)
    {
        try
        {
            return await _dbSet
                .Where(e => 
                    e.FirstName.Contains(searchTerm) ||
                    e.LastName.Contains(searchTerm) ||
                    e.Email.Contains(searchTerm) ||
                    e.EmployeeNumber.Contains(searchTerm) ||
                    e.Position.Contains(searchTerm))
                .Include(e => e.DepartmentNavigation)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException($"Error searching employees with term {searchTerm}", ex);
        }
    }

    /// <summary>
    /// Get employee count by department using LINQ grouping
    /// </summary>
    public async Task<Dictionary<string, int>> GetEmployeeCountByDepartmentAsync()
    {
        try
        {
            return await _dbSet
                .Where(e => e.IsActive)
                .GroupBy(e => e.Department)
                .Select(g => new { Department = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Department, x => x.Count);
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException("Error retrieving employee count by department", ex);
        }
    }

    /// <summary>
    /// Get average salary by department using LINQ aggregation
    /// </summary>
    public async Task<decimal> GetAverageSalaryByDepartmentAsync(string department)
    {
        try
        {
            return await _dbSet
                .Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase) && e.IsActive)
                .AverageAsync(e => e.Salary);
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException($"Error retrieving average salary for department {department}", ex);
        }
    }

    /// <summary>
    /// Get top earners using LINQ ordering with SQLite workaround
    /// </summary>
    public async Task<IEnumerable<Employee>> GetTopEarnersAsync(int count)
    {
        try
        {
            // SQLite doesn't support decimal ordering directly, so we need to cast to double
            return await _dbSet
                .Where(e => e.IsActive)
                .OrderByDescending(e => (double)e.Salary)
                .Take(count)
                .Include(e => e.DepartmentNavigation)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException($"Error retrieving top {count} earners", ex);
        }
    }

    /// <summary>
    /// Get employee by employee number with exception handling
    /// </summary>
    public async Task<Employee> GetByEmployeeNumberAsync(string employeeNumber)
    {
        try
        {
            var employee = await _dbSet
                .Where(e => e.EmployeeNumber == employeeNumber)
                .Include(e => e.DepartmentNavigation)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                throw new EmployeeNotFoundException($"Employee with number {employeeNumber} was not found");
            }

            return employee;
        }
        catch (EmployeeNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException($"Error retrieving employee with number {employeeNumber}", ex);
        }
    }

    /// <summary>
    /// Check if employee number is unique using LINQ
    /// </summary>
    public async Task<bool> IsEmployeeNumberUniqueAsync(string employeeNumber, int? excludeId = null)
    {
        try
        {
            var query = _dbSet.Where(e => e.EmployeeNumber == employeeNumber);
            
            if (excludeId.HasValue)
            {
                query = query.Where(e => e.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException($"Error checking uniqueness of employee number {employeeNumber}", ex);
        }
    }

    /// <summary>
    /// Get employees eligible for promotion using business logic
    /// </summary>
    public async Task<IEnumerable<Employee>> GetEmployeesEligibleForPromotionAsync()
    {
        try
        {
            var employees = await _dbSet
                .Where(e => e.IsActive)
                .Include(e => e.DepartmentNavigation)
                .ToListAsync();

            // Use business logic from the Employee class
            return employees.Where(e => e.IsEligibleForPromotion()).ToList();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException("Error retrieving employees eligible for promotion", ex);
        }
    }
}