namespace WebApplication1.Repositories;


public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
    
    Task<IEnumerable<T>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, object>> orderBy, bool ascending = true);
}