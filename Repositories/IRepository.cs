namespace WebApplication1.Repositories;

/// <summary>
/// Generic repository interface demonstrating abstraction and generics
/// </summary>
/// <typeparam name="T">The type of entity</typeparam>
public interface IRepository<T> where T : class
{
    // CRUD operations with async/await
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
    
    // Method demonstrating LINQ usage
    Task<IEnumerable<T>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, object>> orderBy, bool ascending = true);
}