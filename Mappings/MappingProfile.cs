using AutoMapper;
using WebApplication1.DTOs;
using WebApplication1.Models.Entities;

namespace WebApplication1.Mappings;

public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public decimal Budget { get; set; }
    public DateTime CreatedDate { get; set; }
    public int EmployeeCount { get; set; }
    public decimal TotalSalaryExpense { get; set; }
}


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.DepartmentName,
                       opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : "No Department"))
            .ForMember(dest => dest.DepartmentId,
                       opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.ManagerId,
                       opt => opt.MapFrom(src => src.ManagerId))
            .ForMember(dest => dest.ManagerName,
                       opt => opt.MapFrom(src => src.Manager != null ? src.Manager.GetFullName() : "No Manager"))
            .ForMember(dest => dest.AnnualSalary,
                       opt => opt.MapFrom(src => src.Salary * 12))
            .ForMember(dest => dest.Age,
                       opt => opt.MapFrom(src => src.CalculateAge()));

        CreateMap<EmployeeDto, Employee>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Manager, opt => opt.Ignore());

        CreateMap<CreateEmployeeDto, Employee>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Manager, opt => opt.Ignore());

        CreateMap<Employee, CreateEmployeeDto>()
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId));

        CreateMap<Manager, EmployeeDto>()
            .IncludeBase<Employee, EmployeeDto>()
            .ForMember(dest => dest.DepartmentName,
                       opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : "No Department"))
            .ForMember(dest => dest.AnnualSalary,
                       opt => opt.MapFrom(src => src.Salary * 12 + src.Bonus))
            .ForMember(dest => dest.Age,
                       opt => opt.MapFrom(src => src.CalculateAge()));

        CreateMap<Contractor, EmployeeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.ContractorNumber))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Company))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Specialty))
            .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => src.ContractStartDate))
            .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.HourlyRate * 160)) // Approximate monthly salary
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsContractValid()))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Company))
            .ForMember(dest => dest.AnnualSalary, opt => opt.MapFrom(src => src.HourlyRate * 160 * 12))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.CalculateAge()));

        CreateMap<Department, DepartmentDto>()
            .ForMember(dest => dest.EmployeeCount,
                       opt => opt.MapFrom(src => src.Employees != null ? src.Employees.Count(e => e.IsActive) : 0))
            .ForMember(dest => dest.TotalSalaryExpense,
                       opt => opt.MapFrom(src => src.GetTotalSalaryExpense()));

        CreateMap<DepartmentDto, Department>()
            .ForMember(dest => dest.Employees, opt => opt.Ignore());
    }
}