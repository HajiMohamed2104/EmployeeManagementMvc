using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;

/// <summary>
/// Employee class demonstrating inheritance from Person base class
/// </summary>
public class Employee : Person
{
    [Required(ErrorMessage = "Employee number is required")]
    [StringLength(20)]
    public string EmployeeNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required")]
    [StringLength(50)]
    public string Department { get; set; } = string.Empty;

    [Required(ErrorMessage = "Position is required")]
    [StringLength(100)]
    public string Position { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime HireDate { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }

    public bool IsActive { get; set; } = true;

    // Foreign key properties
    public int? DepartmentId { get; set; }
    public int? ManagerId { get; set; }

    // Navigation properties for related entities (demonstrating relationships)
    public virtual Department? DepartmentNavigation { get; set; }
    public virtual Manager? Manager { get; set; }

    // Override demonstrating polymorphism
    public override string GetRoleDescription()
    {
        return $"Employee working as {Position} in {Department} department";
    }

    // Override demonstrating polymorphism with additional employee-specific logic
    public override string GetFullName()
    {
        return $"{EmployeeNumber}: {base.GetFullName()}";
    }

    // Method demonstrating encapsulation and business logic
    public decimal GetAnnualSalary()
    {
        return Salary * 12;
    }

    // Method demonstrating conditional logic and access modifiers
    public bool IsEligibleForPromotion()
    {
        var yearsOfService = DateTime.Now.Year - HireDate.Year;
        return yearsOfService >= 2 && IsActive;
    }

    // Method demonstrating exception handling
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