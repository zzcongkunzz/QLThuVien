using Microsoft.EntityFrameworkCore;

namespace QLThuVien.Business.ViewModels;

public class PaginatedResult<T>
    where T : class
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set;}
    public List<T> Items { get; private set; }

    public PaginatedResult(List<T> items,int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling((decimal)count / pageSize);

        Items = items;

    }

    public bool HasPreviousPage => PageIndex > 1;
        
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedResult<T>> CreateAsync(IQueryable<T> query, int pageIndex, int pageSize)
    {
        var count = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedResult<T>(items, count, pageIndex, pageSize);
    }
}