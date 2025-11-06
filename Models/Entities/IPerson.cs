using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Entities;

/// <summary>
/// Interface demonstrating abstraction - defines contract for person-related entities
/// </summary>
public interface IPerson
{
    int Id { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Email { get; set; }
    string PhoneNumber { get; set; }
    DateTime DateOfBirth { get; set; }
    
    // Abstract method for calculating age (polymorphism)
    int CalculateAge();
    
    // Abstract method for getting full name
    string GetFullName();
}