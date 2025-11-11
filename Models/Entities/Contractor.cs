using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entities;

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

    public override string GetRoleDescription()
    {
        return $"Contractor from {Company ?? "Unknown Company"} specializing in {Specialty ?? "Unknown Specialty"}";
    }

    public override string GetFullName()
    {
        return $"{ContractorNumber ?? "Unknown"}: {base.GetFullName()} (Contractor)";
    }

    public decimal GetContractValue(int estimatedHours)
    {
        return HourlyRate * estimatedHours;
    }

    public bool IsContractValid()
    {
        var today = DateTime.Today;
        return today >= ContractStartDate && today <= ContractEndDate && IsActive;
    }

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

    public int GetRemainingContractDays()
    {
        if (!IsContractValid()) return 0;
        
        var today = DateTime.Today;
        return (ContractEndDate - today).Days;
    }
}