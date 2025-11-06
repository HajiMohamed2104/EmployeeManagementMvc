using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;

/// <summary>
/// Department entity demonstrating relationships and data annotations
/// </summary>
public class Department
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Department name is required")]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(20)]
    public string? Location { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Budget { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Navigation property for related employees (one-to-many relationship)
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    // Method demonstrating business logic
    public int GetEmployeeCount()
    {
        return Employees?.Count ?? 0;
    }

    // Method demonstrating LINQ usage
    public decimal GetTotalSalaryExpense()
    {
        return Employees?.Where(e => e.IsActive).Sum(e => e.GetAnnualSalary()) ?? 0;
    }
}