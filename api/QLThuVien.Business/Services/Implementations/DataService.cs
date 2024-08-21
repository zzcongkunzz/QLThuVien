using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using QLThuVien.Business.Exceptions;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;

namespace QLThuVien.Business.Services.Implementations;

public class DataService<T>
    (IUnitOfWork unitOfWork, ILogger<DataService<T>> logger)
    : IDataService<T>
    where T : class
{
    public virtual async void AddAsync(T entity)
    {
        if (entity == null)
        {
            logger.LogError($"{nameof(T)} is null!");
            throw new ArgumentNullException(nameof(entity));
        }

        unitOfWork.GetRepository<T>().Add(entity);

        var result = await unitOfWork.SaveChangesAsync();

        if (result > 0)
            logger.LogInformation($"{nameof(T)} added successfully!");
        else
        {
            logger.LogError($"{nameof(T)} add failed!");
            throw new BadRequestException($"{nameof(T)} add failed!");
        }
    }

    public virtual async void DeleteAsync(T entity)
    {
        if (entity == null)
        {
            logger.LogError("entity is null!");
            throw new ArgumentNullException(nameof(entity));
        }

        unitOfWork.GetRepository<T>().Delete(entity);

        var result = await unitOfWork.SaveChangesAsync();

        if (result > 0)
            logger.LogInformation("Delete successfully!");
        else
        {
            logger.LogError("Delete failed!");
            throw new BadRequestException("Delete failed!");
        }
    }

    public virtual async void DeleteAsync(params object?[]? keys)
    {
        unitOfWork.GetRepository<T>().Delete(keys);

        var result = await unitOfWork.SaveChangesAsync();

        if (result > 0)
            logger.LogInformation("Delete successfully!");
        else
        {
            logger.LogError("Delete failed!");
            throw new BadRequestException("Delete failed!");
        }
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await unitOfWork.GetRepository<T>().GetAllAsync();
    }


    public virtual async Task<PaginatedResult<T>> GetAsync(
        int pageIndex = 1, 
        int pageSize = 10,
        string includeProperties = "", 
        Expression<Func<T, bool>>? filter = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
        )
    {
        var query = unitOfWork.GetRepository<T>()
            .Get(filter, orderBy, includeProperties);

        return await PaginatedResult<T>.CreateAsync(query, pageIndex, pageSize);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await unitOfWork.GetRepository<T>().GetByIdAsync(id);
    }

    public virtual async void UpdateAsync(T entity)
    {
        if (entity == null)
        {
            logger.LogError("Entity is null!");
            throw new ArgumentNullException(nameof(entity));
        }

        unitOfWork.GetRepository<T>().Update(entity);

        var result = await unitOfWork.SaveChangesAsync();

        if (result > 0)
            logger.LogInformation("Update successfully!");
        else
        {
            logger.LogError("Update failed!");
            throw new BadRequestException("Update failed!");
        }
    }
}