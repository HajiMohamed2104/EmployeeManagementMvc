using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Entities;

namespace WebApplication1.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Contractor> Contractors { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EmployeeNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Position).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => e.EmployeeNumber).IsUnique();
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.Property(m => m.ManagementLevel).HasMaxLength(50);
            entity.Property(m => m.Bonus).HasColumnType("decimal(18,2)");
            
            entity.HasMany(m => m.ManagedEmployees)
                  .WithOne(e => e.Manager)
                  .HasForeignKey(e => e.ManagerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Contractor>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.ContractorNumber).IsRequired().HasMaxLength(20);
            entity.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(c => c.LastName).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
            entity.Property(c => c.PhoneNumber).HasMaxLength(20);
            entity.Property(c => c.Company).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Specialty).IsRequired().HasMaxLength(100);
            entity.Property(c => c.HourlyRate).HasColumnType("decimal(18,2)");
            entity.HasIndex(c => c.ContractorNumber).IsUnique();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(50);
            entity.Property(d => d.Description).HasMaxLength(500);
            entity.Property(d => d.Location).HasMaxLength(20);
            entity.Property(d => d.Budget).HasColumnType("decimal(18,2)");
            entity.HasIndex(d => d.Name).IsUnique();
            
            entity.HasMany(d => d.Employees)
                  .WithOne(e => e.Department)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "IT", Description = "Information Technology Department", Location = "Floor 3", Budget = 500000 },
            new Department { Id = 2, Name = "HR", Description = "Human Resources Department", Location = "Floor 2", Budget = 200000 },
            new Department { Id = 3, Name = "Finance", Description = "Finance Department", Location = "Floor 4", Budget = 300000 }
        );
    }

    public override int SaveChanges()
    {
        try
        {
            ValidateEntities();
            return base.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while saving changes", ex);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            ValidateEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while saving changes", ex);
        }
    }

    private void ValidateEntities()
    {
        var employeeEntries = ChangeTracker.Entries()
            .Where(e => e.Entity is Employee && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in employeeEntries)
        {
            if (entry.Entity is Employee employee)
            {
                if (employee.Salary < 0)
                {
                    throw new InvalidOperationException("Salary cannot be negative");
                }

                if (employee.HireDate > DateTime.Now)
                {
                    throw new InvalidOperationException("Hire date cannot be in the future");
                }

                if (employee.DateOfBirth > DateTime.Now.AddYears(-16))
                {
                    throw new InvalidOperationException("Employee must be at least 16 years old");
                }
            }
        }

        var managerEntries = ChangeTracker.Entries()
            .Where(e => e.Entity is Manager && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in managerEntries)
        {
            if (entry.Entity is Manager manager)
            {
                if (manager.Bonus < 0)
                {
                    throw new InvalidOperationException("Bonus cannot be negative");
                }
            }
        }
    }
}