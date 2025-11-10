using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;


public class Manager : Employee
{
    [StringLength(50)]
    public string? ManagementLevel { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Bonus { get; set; }

    public virtual ICollection<Employee> ManagedEmployees { get; set; } = new List<Employee>();

    public override string GetRoleDescription()
    {
        return $"{ManagementLevel} Manager overseeing {GetManagedEmployeeCount()} employees in {Department?.Name ?? "No Department"} department";
    }

    public override string GetFullName()
    {
        return $"{base.GetFullName()} (Manager)";
    }

    public int GetManagedEmployeeCount()
    {
        return ManagedEmployees?.Count ?? 0;
    }

    public decimal GetTotalCompensation()
    {
        return GetAnnualSalary() + Bonus;
    }

    public bool CanApproveExpense(decimal amount)
    {
        return ManagementLevel switch
        {
            "Senior" => amount <= 10000,
            "Mid" => amount <= 5000,
            "Junior" => amount <= 2000,
            _ => amount <= 1000
        };
    }

    public void AddManagedEmployee(Employee employee)
    {
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        if (employee.Id == this.Id)
        {
            throw new InvalidOperationException("A manager cannot manage themselves");
        }

        if (ManagedEmployees.Any(e => e.Id == employee.Id))
        {
            throw new InvalidOperationException("Employee is already managed by this manager");
        }

        ManagedEmployees.Add(employee);
    }
}