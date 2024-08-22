using QLThuVien.Data.Data;
using QLThuVien.Data.Repositories;

namespace QLThuVien.Data.Infrastructure;

public class UnitOfWork 
    (AppDbContext context)
    : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = [];
    
    public IGenericRepository<T> GetRepository<T>() where T : class
    {
        _repositories.TryGetValue(typeof(T), out var repo);
        
        if (repo != null) return (GenericRepository<T>) repo;
        
        repo = new GenericRepository<T>(context);
        _repositories.Add(typeof(T), repo);
        
        return (GenericRepository<T>) repo;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}