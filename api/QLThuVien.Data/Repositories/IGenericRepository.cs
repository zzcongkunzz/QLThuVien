using System.Linq.Expressions;

namespace QuizApp.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T? GetById(params object?[]? keys);
        Task<T?> GetByIdAsync(params object?[]? keys);
        void Add(T entity);
        void Update(T entity);
        void Delete(params object?[]? keys);
        void Delete(T entity);
        IQueryable<T> GetQuery();
        IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate);
        IQueryable<T> Get(
        Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>,
       IOrderedQueryable<T>>? orderBy = null, string includeProperties = "");
    }
}
