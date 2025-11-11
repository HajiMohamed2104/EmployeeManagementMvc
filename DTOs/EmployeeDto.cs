using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Employee number is required")]
    [StringLength(20)]
    public string EmployeeNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Department is required")]
    [StringLength(50)]
    public string Department { get; set; } = string.Empty;

    [Required(ErrorMessage = "Position is required")]
    [StringLength(100)]
    public string Position { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime HireDate { get; set; }

    [DataType(DataType.Currency)]
    public decimal Salary { get; set; }

    public bool IsActive { get; set; } = true;
    public int? DepartmentId { get; set; }
    public int? ManagerId { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public int Age => CalculateAge();
    public decimal AnnualSalary => Salary * 12;
    public string DepartmentName { get; set; } = string.Empty;
    public string ManagerName { get; set; } = string.Empty;

    private int CalculateAge()
    {
        var today = DateTime.Today;
        var age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}