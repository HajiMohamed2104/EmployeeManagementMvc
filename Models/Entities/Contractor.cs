using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;

/// <summary>
/// Contractor class demonstrating inheritance from Person (not Employee) and polymorphism
/// </summary>
public class Contractor : Person
{
    [Required(ErrorMessage = "Contractor number is required")]
    [StringLength(20)]
    public string ContractorNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company is required")]
    [StringLength(100)]
    public string Company { get; set; } = string.Empty;

    [Required(ErrorMessage = "Specialty is required")]
    [StringLength(100)]
    public string Specialty { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime ContractStartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime ContractEndDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal HourlyRate { get; set; }

    public bool IsActive { get; set; } = true;

    // Override demonstrating polymorphism
    public override string GetRoleDescription()
    {
        return $"Contractor from {Company} specializing in {Specialty}";
    }

    // Override demonstrating polymorphism with contractor-specific information
    public override string GetFullName()
    {
        return $"{ContractorNumber}: {base.GetFullName()} (Contractor)";
    }

    // Method demonstrating business logic specific to contractors
    public decimal GetContractValue(int estimatedHours)
    {
        return HourlyRate * estimatedHours;
    }

    // Method demonstrating conditional logic and access modifiers
    public bool IsContractValid()
    {
        var today = DateTime.Today;
        return today >= ContractStartDate && today <= ContractEndDate && IsActive;
    }

    // Method demonstrating exception handling
    public void ExtendContract(DateTime newEndDate)
    {
        if (newEndDate <= ContractEndDate)
        {
            throw new ArgumentException("New end date must be after current end date", nameof(newEndDate));
        }

        if (newEndDate > DateTime.Today.AddYears(2))
        {
            throw new InvalidOperationException("Contract cannot be extended more than 2 years from today");
        }

        ContractEndDate = newEndDate;
    }

    // Method demonstrating encapsulation and business logic
    public int GetRemainingContractDays()
    {
        if (!IsContractValid()) return 0;
        
        var today = DateTime.Today;
        return (ContractEndDate - today).Days;
    }
}