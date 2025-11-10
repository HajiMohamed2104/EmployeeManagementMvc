using WebApplication1.DTOs;

namespace WebApplication1.ViewModels;

public class StatisticsViewModel
{
    public Dictionary<string, int> EmployeeCountByDepartment { get; set; } = new();
    public IEnumerable<EmployeeDto> TopEarners { get; set; } = new List<EmployeeDto>();
    public IEnumerable<EmployeeDto> EligibleForPromotion { get; set; } = new List<EmployeeDto>();
    public int TotalEmployees { get; set; }
}