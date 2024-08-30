using QLThuVien.Data.Repositories;

namespace QLThuVien.Data.Infrastructure;

public interface IUnitOfWork
{
    public IGenericRepository<T> GetRepository<T>() where T : class;

    public Task<int> SaveChangesAsync();
}