using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;

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

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public int GetEmployeeCount()
    {
        return Employees?.Count ?? 0;
    }

    public decimal GetTotalSalaryExpense()
    {
        return Employees?.Where(e => e.IsActive).Sum(e => e.GetAnnualSalary()) ?? 0;
    }
}