using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Entities;

public interface IPerson
{
    int Id { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Email { get; set; }
    string PhoneNumber { get; set; }
    DateTime DateOfBirth { get; set; }
    
    int CalculateAge();
    
    string GetFullName();
}