using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Entities;

/// <summary>
/// Base class demonstrating encapsulation and implementation of IPerson interface
/// </summary>
public abstract class Person : IPerson
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    // Protected field demonstrating encapsulation
    protected DateTime _createdDate = DateTime.Now;

    // Public property with private setter demonstrating encapsulation
    public DateTime CreatedDate 
    { 
        get { return _createdDate; }
        private set { _createdDate = value; }
    }

    // Virtual method allowing for polymorphic behavior
    public virtual int CalculateAge()
    {
        var today = DateTime.Today;
        var age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

    // Virtual method for polymorphism
    public virtual string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }

    // Abstract method that must be implemented by derived classes
    public abstract string GetRoleDescription();
}