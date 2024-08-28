using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QLThuVien.Business.Exceptions;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using System.Linq.Expressions;

namespace QLThuVien.Business.Services.Implementations;

public class BorrowService(IUnitOfWork unitOfWork, ILogger<DataService<Borrow>> logger) 
    : DataService<Borrow>(unitOfWork, logger), IBorrowService
{
    public async Task<IEnumerable<BorrowVm>> GetAllVm()
    {
        return (await unitOfWork.GetRepository<Borrow>()
                    .GetQuery().Include(borrow => borrow.Book).Include(borrow => borrow.User)
                    .ToListAsync()).Select(borrow => ToBorrrowVm(borrow));
    }

    public async Task<PaginatedResult<BorrowVm>> GetAsyncVm
        (
            int pageIndex = 1,
            int pageSize = 10,
            Expression<Func<Borrow, bool>>? filter = null,
            Func<IQueryable<Borrow>, IOrderedQueryable<Borrow>>? orderBy = null
        )
    {
        var query = unitOfWork.GetRepository<Borrow>()
            .Get(filter, orderBy, "User,Book")
            .Select(borrow => ToBorrrowVm(borrow));

        return await PaginatedResult<BorrowVm>.CreateAsync(query, pageIndex, pageSize);
    }

    public async Task<BorrowVm> GetByIdAsyncVm(Guid id)
    {
        return await unitOfWork.GetRepository<Borrow>()
            .Get(borrow => borrow.Id == id, null, "User,Book")
            .Select(borrow => ToBorrrowVm(borrow))
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException("Id not found");
    }

    public async Task AddAsync(BorrowEditVm borrowEditVm)
    {
        await AddAsync(ToBorrow(borrowEditVm));
    }

    public async Task UpdateAsync(Guid id, BorrowEditVm borrowEditVm)
    {
        var borrow = ToBorrow(borrowEditVm);
        borrow.Id = id;
        await UpdateAsync(borrow);
    }

    public async Task ReturnBorrow(Guid id, DateTime returnTime)
    {
        var borrow = await GetByIdAsync(id) ?? throw new BadRequestException("Id not found");
        borrow.ActualReturnTime = returnTime;
        await UpdateAsync(borrow);
    }

    public async Task UndoReturnBorrow(Guid id)
    {
        var borrow = await GetByIdAsync(id) ?? throw new BadRequestException("Id not found");
        borrow.ActualReturnTime = null;
        await UpdateAsync(borrow);
    }

    protected BorrowVm ToBorrrowVm(Borrow borrow)
    {
        return new BorrowVm()
        {
            Id = borrow.Id,
            BookId = borrow.BookId,
            UserId = borrow.UserId,
            UserFullName = borrow.User.FullName,
            BookTitle = borrow.Book.Title,
            Count = borrow.Count,
            StartTime = borrow.StartTime,
            ExpectedReturnTime = borrow.ExpectedReturnTime,
            ActualReturnTime = borrow.ActualReturnTime,
            IssuedPenalties = borrow.IssuedPenalties,
            PaidPenalties = borrow.PaidPenalties
        };
    }

    protected Borrow ToBorrow(BorrowEditVm borrowEditVm)
    {
        return new Borrow()
        {
            User = unitOfWork.GetRepository<User>().GetById(borrowEditVm.UserId)
                ?? throw new BadRequestException("UserId not found"),
            Book = unitOfWork.GetRepository<Book>().GetById(borrowEditVm.BookId)
                ?? throw new BadRequestException("BookId not found"),
            Count = borrowEditVm.Count,
            StartTime = borrowEditVm.StartTime,
            ExpectedReturnTime = borrowEditVm.ExpectedReturnTime,
            ActualReturnTime = unitOfWork.GetRepository<Borrow>()
                .GetById(borrowEditVm.BookId)?.ActualReturnTime,
            IssuedPenalties = borrowEditVm.IssuedPenalties,
            PaidPenalties = borrowEditVm.PaidPenalties
        };
    }
}