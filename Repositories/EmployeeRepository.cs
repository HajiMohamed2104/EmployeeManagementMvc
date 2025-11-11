using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
using WebApplication1.Exceptions;

namespace WebApplication1.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
    {
        return await _dbSet
            .Where(e => e.IsActive)
            .Include(e => e.Department)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string department)
    {
        return await _dbSet
            .Where(e => e.Department != null && e.Department.Name.Equals(department, StringComparison.OrdinalIgnoreCase))
            .Include(e => e.Department)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesBySalaryRangeAsync(decimal minSalary, decimal maxSalary)
    {
        return await _dbSet
            .Where(e => e.Salary >= minSalary && e.Salary <= maxSalary)
            .Include(e => e.Department)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesHiredAfterAsync(DateTime date)
    {
        return await _dbSet
            .Where(e => e.HireDate > date)
            .Include(e => e.Department)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm)
    {
        return await _dbSet
            .Where(e =>
                e.FirstName.Contains(searchTerm) ||
                e.LastName.Contains(searchTerm) ||
                e.Email.Contains(searchTerm) ||
                e.EmployeeNumber.Contains(searchTerm) ||
                e.Position.Contains(searchTerm))
            .Include(e => e.Department)
            .ToListAsync();
    }

    public async Task<Dictionary<string, int>> GetEmployeeCountByDepartmentAsync()
    {
        return await _dbSet
            .Where(e => e.IsActive)
            .GroupBy(e => e.Department != null ? e.Department.Name : "No Department")
            .Select(g => new { Department = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Department, x => x.Count);
    }

    public async Task<decimal> GetAverageSalaryByDepartmentAsync(string department)
    {
        try
        {
            return await _dbSet
                .Where(e => e.Department != null && e.Department.Name.Equals(department, StringComparison.OrdinalIgnoreCase) && e.IsActive)
                .AverageAsync(e => e.Salary);
        }
        catch (InvalidOperationException)
        {
            // No employees found in the department
            return 0;
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException($"Error calculating average salary for department {department}", ex);
        }
    }

    public async Task<IEnumerable<Employee>> GetTopEarnersAsync(int count)
    {
        return await _dbSet
            .Where(e => e.IsActive)
            .OrderByDescending(e => (double)e.Salary)
            .Take(count)
            .Include(e => e.Department)
            .ToListAsync();
    }

    public async Task<Employee?> GetByEmployeeNumberAsync(string employeeNumber)
    {
        return await _dbSet
            .Where(e => e.EmployeeNumber == employeeNumber)
            .Include(e => e.Department)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsEmployeeNumberUniqueAsync(string employeeNumber, int? excludeId = null)
    {
        var query = _dbSet.Where(e => e.EmployeeNumber == employeeNumber);
        
        if (excludeId.HasValue)
        {
            query = query.Where(e => e.Id != excludeId.Value);
        }
        
        return !(await query.AnyAsync());
    }

    public async Task<IEnumerable<Employee>> GetEmployeesEligibleForPromotionAsync()
    {
        var twoYearsAgo = DateTime.Now.AddYears(-2);
        return await _dbSet
            .Where(e => e.IsActive && e.HireDate <= twoYearsAgo)
            .Include(e => e.Department)
            .ToListAsync();
    }
}