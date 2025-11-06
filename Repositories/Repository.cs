using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Exceptions;

namespace WebApplication1.Repositories;

/// <summary>
/// Generic repository implementation demonstrating Repository Pattern, async/await, and exception handling
/// </summary>
/// <typeparam name="T">The type of entity</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    /// <summary>
    /// Get entity by ID with async/await
    /// </summary>
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        try
        {
            return await _dbSet.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException($"Error retrieving entity with ID {id}", ex);
        }
    }

    /// <summary>
    /// Get all entities with async/await
    /// </summary>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _dbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException("Error retrieving all entities", ex);
        }
    }

    /// <summary>
    /// Get all entities with ordering using LINQ
    /// </summary>
    public virtual async Task<IEnumerable<T>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, object>> orderBy, bool ascending = true)
    {
        try
        {
            var query = ascending 
                ? _dbSet.OrderBy(orderBy) 
                : _dbSet.OrderByDescending(orderBy);
            
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException("Error retrieving ordered entities", ex);
        }
    }

    /// <summary>
    /// Find entities based on predicate with LINQ
    /// </summary>
    public virtual async Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException("Error finding entities", ex);
        }
    }

    /// <summary>
    /// Add new entity with async/await
    /// </summary>
    public virtual async Task<T> AddAsync(T entity)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var addedEntity = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return addedEntity.Entity;
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException("Error adding entity", ex);
        }
    }

    /// <summary>
    /// Update existing entity with async/await
    /// </summary>
    public virtual async Task UpdateAsync(T entity)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException("Error updating entity", ex);
        }
    }

    /// <summary>
    /// Delete entity by ID with async/await
    /// </summary>
    public virtual async Task DeleteAsync(int id)
    {
        try
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException($"Error deleting entity with ID {id}", ex);
        }
    }

    /// <summary>
    /// Count entities with async/await
    /// </summary>
    public virtual async Task<int> CountAsync()
    {
        try
        {
            return await _dbSet.CountAsync();
        }
        catch (Exception ex)
        {
            throw new EmployeeManagementException("Error counting entities", ex);
        }
    }
}