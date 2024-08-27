using Microsoft.Extensions.Logging;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Data.Infrastructure;

namespace QLThuVien.Business.Services.Implementations;

public class PenaltyService(IUnitOfWork unitOfWork, ILogger<DataService<Penalty>> logger)
    : DataService<Penalty>(unitOfWork, logger), IPenaltyService
{
    
}