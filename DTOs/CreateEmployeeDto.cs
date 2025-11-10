using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class CreateEmployeeDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Employee number is required")]
    [StringLength(20)]
    [Display(Name = "Employee Number")]
    public string EmployeeNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(100)]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(20)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Position is required")]
    [StringLength(100)]
    [Display(Name = "Position")]
    public string Position { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hire date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Hire Date")]
    public DateTime HireDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Salary is required")]
    [Range(0, 1000000, ErrorMessage = "Salary must be between 0 and 1,000,000")]
    [DataType(DataType.Currency)]
    [Display(Name = "Monthly Salary")]
    public decimal Salary { get; set; }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Department")]
    public int? DepartmentId { get; set; }
}