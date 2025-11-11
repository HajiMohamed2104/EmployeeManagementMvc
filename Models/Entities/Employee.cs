using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;

public class Employee : Person
{
    [Required(ErrorMessage = "Employee number is required")]
    [StringLength(20)]
    public string EmployeeNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Position is required")]
    [StringLength(100)]
    public string Position { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime HireDate { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }

    public bool IsActive { get; set; } = true;

    public int? DepartmentId { get; set; }
    public int? ManagerId { get; set; }

    public virtual Department? Department { get; set; }
    public virtual Manager? Manager { get; set; }

    public override string GetRoleDescription()
    {
        return $"Employee working as {Position ?? "Unknown Position"} in {Department?.Name ?? "No Department"} department";
    }

    public override string GetFullName()
    {
        return $"{EmployeeNumber ?? "Unknown"}: {base.GetFullName()}";
    }

    public decimal GetAnnualSalary()
    {
        return Salary * 12;
    }

    public bool IsEligibleForPromotion()
    {
        var yearsOfService = DateTime.Now.Year - HireDate.Year;
        // Adjust for whether the birthday has occurred this year
        if (HireDate.Date > DateTime.Now.AddYears(-yearsOfService))
        {
            yearsOfService--;
        }
        return yearsOfService >= 2 && IsActive;
    }

    public void GiveRaise(decimal percentage)
    {
        if (percentage <= 0)
        {
            throw new ArgumentException("Raise percentage must be positive", nameof(percentage));
        }

        if (percentage > 50)
        {
            throw new InvalidOperationException("Raise percentage cannot exceed 50%");
        }

        Salary = Salary * (1 + (percentage / 100));
    }
}