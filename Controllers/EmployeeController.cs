using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models.Entities;
using WebApplication1.Repositories;
using WebApplication1.Exceptions;

namespace WebApplication1.Controllers;

/// <summary>
/// Employee Controller demonstrating CRUD operations, async/await, LINQ, and exception handling
/// </summary>
public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRepository<Department> _departmentRepository;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(
        IEmployeeRepository employeeRepository,
        IRepository<Department> departmentRepository,
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<EmployeeController> logger)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: Employee
    /// <summary>
    /// Index action demonstrating async/await and LINQ
    /// </summary>
    public async Task<IActionResult> Index(string searchString, string departmentFilter, decimal? minSalary, decimal? maxSalary)
    {
        try
        {
            IEnumerable<Employee> employees;

            // Demonstrate LINQ filtering based on parameters
            if (!string.IsNullOrEmpty(searchString))
            {
                employees = await _employeeRepository.SearchEmployeesAsync(searchString);
            }
            else if (!string.IsNullOrEmpty(departmentFilter))
            {
                employees = await _employeeRepository.GetEmployeesByDepartmentAsync(departmentFilter);
            }
            else if (minSalary.HasValue && maxSalary.HasValue)
            {
                employees = await _employeeRepository.GetEmployeesBySalaryRangeAsync(minSalary.Value, maxSalary.Value);
            }
            else
            {
                employees = await _employeeRepository.GetAllAsync();
            }

            // Map entities to DTOs using AutoMapper
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            // Pass filter data to view
            ViewBag.Departments = await GetDepartmentSelectList();
            ViewBag.CurrentFilter = searchString;
            ViewBag.DepartmentFilter = departmentFilter;
            ViewBag.MinSalary = minSalary;
            ViewBag.MaxSalary = maxSalary;

            return View(employeeDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee list");
            ModelState.AddModelError("", "An error occurred while retrieving employees");
            return View(new List<EmployeeDto>());
        }
    }

    // GET: Employee/Details/5
    /// <summary>
    /// Details action demonstrating async/await and exception handling
    /// </summary>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        try
        {
            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            // Map entity to DTO using AutoMapper
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return View(employeeDto);
        }
        catch (EmployeeNotFoundException ex)
        {
            _logger.LogWarning(ex, "Employee not found: {Id}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee details: {Id}", id);
            ModelState.AddModelError("", "An error occurred while retrieving employee details");
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Employee/Create
    /// <summary>
    /// Create GET action demonstrating dependency injection
    /// </summary>
    public async Task<IActionResult> Create()
    {
        try
        {
            ViewBag.Departments = await GetDepartmentSelectList();
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create employee form");
            ModelState.AddModelError("", "An error occurred while loading the form");
            return View();
        }
    }

    // POST: Employee/Create
    /// <summary>
    /// Create POST action demonstrating validation, async/await, and exception handling
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEmployeeDto createEmployeeDto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                // Check if employee number is unique using LINQ
                var isUnique = await _employeeRepository.IsEmployeeNumberUniqueAsync(createEmployeeDto.EmployeeNumber);
                if (!isUnique)
                {
                    ModelState.AddModelError("EmployeeNumber", "Employee number already exists");
                    ViewBag.Departments = await GetDepartmentSelectList();
                    return View(createEmployeeDto);
                }

                // Map DTO to entity using AutoMapper
                var employee = _mapper.Map<Employee>(createEmployeeDto);

                // Add employee using repository pattern
                await _employeeRepository.AddAsync(employee);

                _logger.LogInformation("Employee created successfully: {Id}", employee.Id);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = await GetDepartmentSelectList();
            return View(createEmployeeDto);
        }
        catch (InvalidEmployeeDataException ex)
        {
            _logger.LogWarning(ex, "Invalid employee data");
            ModelState.AddModelError("", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            ModelState.AddModelError("", "An error occurred while creating the employee");
        }

        ViewBag.Departments = await GetDepartmentSelectList();
        return View(createEmployeeDto);
    }

    // GET: Employee/Edit/5
    /// <summary>
    /// Edit GET action demonstrating async/await
    /// </summary>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        try
        {
            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            // Map entity to DTO using AutoMapper
            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);
            ViewBag.Departments = await GetDepartmentSelectList();
            return View(employeeDto);
        }
        catch (EmployeeNotFoundException ex)
        {
            _logger.LogWarning(ex, "Employee not found: {Id}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading edit form: {Id}", id);
            ModelState.AddModelError("", "An error occurred while loading the form");
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Employee/Edit/5
    /// <summary>
    /// Edit POST action demonstrating validation, async/await, and exception handling
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateEmployeeDto createEmployeeDto)
    {
        if (id != createEmployeeDto.Id && createEmployeeDto.Id != 0)
        {
            return BadRequest();
        }

        try
        {
            if (ModelState.IsValid)
            {
                // Get existing employee
                var existingEmployee = await _employeeRepository.GetByIdAsync(id);
                if (existingEmployee == null)
                {
                    return NotFound();
                }

                // Check if employee number is unique (excluding current employee)
                var isUnique = await _employeeRepository.IsEmployeeNumberUniqueAsync(createEmployeeDto.EmployeeNumber, id);
                if (!isUnique)
                {
                    ModelState.AddModelError("EmployeeNumber", "Employee number already exists");
                    ViewBag.Departments = await GetDepartmentSelectList();
                    return View(createEmployeeDto);
                }

                // Update properties using AutoMapper
                _mapper.Map(createEmployeeDto, existingEmployee);

                // Update employee using repository pattern
                await _employeeRepository.UpdateAsync(existingEmployee);

                _logger.LogInformation("Employee updated successfully: {Id}", id);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = await GetDepartmentSelectList();
            return View(createEmployeeDto);
        }
        catch (EmployeeNotFoundException ex)
        {
            _logger.LogWarning(ex, "Employee not found: {Id}", id);
            return NotFound();
        }
        catch (InvalidEmployeeDataException ex)
        {
            _logger.LogWarning(ex, "Invalid employee data");
            ModelState.AddModelError("", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee: {Id}", id);
            ModelState.AddModelError("", "An error occurred while updating the employee");
        }

        ViewBag.Departments = await GetDepartmentSelectList();
        return View(createEmployeeDto);
    }

    // GET: Employee/Delete/5
    /// <summary>
    /// Delete GET action demonstrating async/await
    /// </summary>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        try
        {
            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            // Map entity to DTO using AutoMapper
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return View(employeeDto);
        }
        catch (EmployeeNotFoundException ex)
        {
            _logger.LogWarning(ex, "Employee not found: {Id}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading delete confirmation: {Id}", id);
            ModelState.AddModelError("", "An error occurred while loading the delete confirmation");
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Employee/Delete/5
    /// <summary>
    /// Delete POST action demonstrating async/await and exception handling
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _employeeRepository.DeleteAsync(id);
            _logger.LogInformation("Employee deleted successfully: {Id}", id);
            return RedirectToAction(nameof(Index));
        }
        catch (EmployeeNotFoundException ex)
        {
            _logger.LogWarning(ex, "Employee not found: {Id}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee: {Id}", id);
            ModelState.AddModelError("", "An error occurred while deleting the employee");
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Employee/Statistics
    /// <summary>
    /// Statistics action demonstrating LINQ aggregation and grouping
    /// </summary>
    public async Task<IActionResult> Statistics()
    {
        try
        {
            // Demonstrate LINQ operations
            var employeeCountByDepartment = await _employeeRepository.GetEmployeeCountByDepartmentAsync();
            var topEarners = await _employeeRepository.GetTopEarnersAsync(5);
            var eligibleForPromotion = await _employeeRepository.GetEmployeesEligibleForPromotionAsync();

            var statisticsViewModel = new StatisticsViewModel
            {
                EmployeeCountByDepartment = employeeCountByDepartment,
                TopEarners = _mapper.Map<IEnumerable<EmployeeDto>>(topEarners),
                EligibleForPromotion = _mapper.Map<IEnumerable<EmployeeDto>>(eligibleForPromotion),
                TotalEmployees = await _employeeRepository.CountAsync()
            };

            return View(statisticsViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee statistics");
            ModelState.AddModelError("", "An error occurred while retrieving statistics");
            return View(new StatisticsViewModel());
        }
    }

    /// <summary>
    /// Helper method to get department select list
    /// </summary>
    private async Task<SelectList> GetDepartmentSelectList()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return new SelectList(departments, "Id", "Name");
    }
}

/// <summary>
/// Statistics view model for demonstrating view model pattern
/// </summary>
public class StatisticsViewModel
{
    public Dictionary<string, int> EmployeeCountByDepartment { get; set; } = new();
    public IEnumerable<EmployeeDto> TopEarners { get; set; } = new List<EmployeeDto>();
    public IEnumerable<EmployeeDto> EligibleForPromotion { get; set; } = new List<EmployeeDto>();
    public int TotalEmployees { get; set; }
}