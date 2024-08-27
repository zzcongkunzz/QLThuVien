using Microsoft.Extensions.Logging;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Data.Infrastructure;

namespace QLThuVien.Business.Services.Implementations;

public class BorrowService(IUnitOfWork unitOfWork, ILogger<DataService<Borrow>> logger) 
    : DataService<Borrow>(unitOfWork, logger), IBorrowService
{
    
}