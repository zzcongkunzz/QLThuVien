using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using QLThuVien.Data.Repositories;

namespace QLThuVien.UnitTesting.Services.Implementations;

[TestFixture]
[TestOf(typeof(BorrowService))]
public class BorrowServiceTest
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ILogger<DataService<Borrow>>> _loggerMock;
    private BorrowService _borrowService;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<DataService<Borrow>>>();
        _borrowService = new BorrowService(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetAllVm_ShouldReturnAllBorrowVms()
    {
        // Arrange
        var borrowList = new List<Borrow>
        {
            new Borrow
            {
                Id = Guid.NewGuid(),
                User = new User
                {
                    FullName = "John Doe",
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    Gender = "Male",
                },
                Book = new Book
                {
                    Title = "Book Title",
                    AuthorName = "cong",
                    PublisherName = "cong",
                    PublishDate = default,
                    Count = 0
                },
                Count = 1,
                StartTime = DateTime.Now,
                ExpectedReturnTime = DateTime.Now.AddDays(7),
                IssuedPenalties = 0f,
                PaidPenalties = 0f
            }
        }.AsQueryable();

        var repositoryMock = new Mock<IGenericRepository<Borrow>>();
        repositoryMock.Setup(r => r.GetQuery()).Returns(borrowList);

        _unitOfWorkMock.Setup(u => u.GetRepository<Borrow>()).Returns(repositoryMock.Object);

        // Act
        var result = await _borrowService.GetAllVm();

        // Assert
        Assert.NotNull(result);
        Assert.IsInstanceOf<IEnumerable<BorrowVm>>(result);
    }

    [Test]
    public async Task GetAsyncVm_ShouldReturnPaginatedBorrowVms()
    {
        // Arrange
        var borrowList = new List<Borrow>
        {
            new Borrow
            {
                Id = Guid.NewGuid(),
                User = new User
                {
                    FullName = "John Doe",
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    Gender = "Male",
                },
                Book = new Book
                {
                    Title = "Book Title",
                    AuthorName = "cong",
                    PublisherName = "cong",
                    PublishDate = default,
                    Count = 0
                },
                Count = 1,
                StartTime = DateTime.Now,
                ExpectedReturnTime = DateTime.Now.AddDays(7),
                IssuedPenalties = 0f,
                PaidPenalties = 0f
            }
        }.AsQueryable();

        var repositoryMock = new Mock<IGenericRepository<Borrow>>();
        repositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Borrow, bool>>>(), null, "User,Book"))
            .Returns(borrowList);

        _unitOfWorkMock.Setup(u => u.GetRepository<Borrow>()).Returns(repositoryMock.Object);

        // Act
        var result = await _borrowService.GetAsyncVm(1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.IsInstanceOf<PaginatedResult<BorrowVm>>(result);
    }

    [Test]
    public async Task GetByIdAsyncVm_ShouldReturnBorrowVm()
    {
        // Arrange
        var borrow = new Borrow
        {
            Id = Guid.NewGuid(),
            User = new User
            {
                FullName = "John Doe",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Gender = "Male",
            },
            Book = new Book
            {
                Title = "Book Title",
                AuthorName = "cong",
                PublisherName = "cong",
                PublishDate = default,
                Count = 0
            },
            Count = 1,
            StartTime = DateTime.Now,
            ExpectedReturnTime = DateTime.Now.AddDays(7),
            IssuedPenalties = 0f,
            PaidPenalties = 0f
        };

        var repositoryMock = new Mock<IGenericRepository<Borrow>>();
        repositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Borrow, bool>>>(), null, "User,Book"))
            .Returns(new List<Borrow> { borrow }.AsQueryable());

        _unitOfWorkMock.Setup(u => u.GetRepository<Borrow>()).Returns(repositoryMock.Object);

        // Act
        var result = await _borrowService.GetByIdAsyncVm(borrow.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsInstanceOf<BorrowVm>(result);
    }

    [Test]
    public async Task AddAsync_ShouldAddBorrow()
    {
        // Arrange
        var borrowEditVm = new BorrowEditVm
        {
            UserId = Guid.NewGuid(),
            BookId = Guid.NewGuid(),
            Count = 1,
            StartTime = DateTime.Now,
            ExpectedReturnTime = DateTime.Now.AddDays(7),
            IssuedPenalties = 0f,
            PaidPenalties = 0f
        };

        var borrowRepositoryMock = new Mock<IGenericRepository<Borrow>>();
        _unitOfWorkMock.Setup(u => u.GetRepository<Borrow>()).Returns(borrowRepositoryMock.Object);

        // Act
        await _borrowService.AddAsync(borrowEditVm);

        // Assert
        borrowRepositoryMock.Verify(r => r.Add(It.IsAny<Borrow>()), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdateBorrow()
    {
        // Arrange
        var borrowEditVm = new BorrowEditVm
        {
            UserId = Guid.NewGuid(),
            BookId = Guid.NewGuid(),
            Count = 1,
            StartTime = DateTime.Now,
            ExpectedReturnTime = DateTime.Now.AddDays(7),
            IssuedPenalties = 0f,
            PaidPenalties = 0f
        };

        var borrowRepositoryMock = new Mock<IGenericRepository<Borrow>>();
        _unitOfWorkMock.Setup(u => u.GetRepository<Borrow>()).Returns(borrowRepositoryMock.Object);

        // Act
        await _borrowService.UpdateAsync(Guid.NewGuid(), borrowEditVm);

        // Assert
        borrowRepositoryMock.Verify(r => r.Update(It.IsAny<Borrow>()), Times.Once);
    }

    [Test]
    public async Task ReturnBorrow_ShouldUpdateActualReturnTime()
    {
        // Arrange
        var borrow = new Borrow
        {
            Id = Guid.NewGuid(),
            ActualReturnTime = null,
            StartTime = default,
            ExpectedReturnTime = default,
            Count = 0,
            IssuedPenalties = 0,
            PaidPenalties = 0
        };

        var borrowRepositoryMock = new Mock<IGenericRepository<Borrow>>();
        borrowRepositoryMock.Setup(r => r.GetById(borrow.Id)).Returns(borrow);
        _unitOfWorkMock.Setup(u => u.GetRepository<Borrow>()).Returns(borrowRepositoryMock.Object);

        // Act
        // await _borrowService.ReturnBorrow(borrow.Id, DateTime.Now);

        // Assert
        Assert.NotNull(borrow.ActualReturnTime);
        borrowRepositoryMock.Verify(r => r.Update(It.IsAny<Borrow>()), Times.Once);
    }

    [Test]
    public async Task UndoReturnBorrow_ShouldResetActualReturnTime()
    {
        // Arrange
        var borrow = new Borrow
        {
            Id = Guid.NewGuid(),
            ActualReturnTime = DateTime.Now,
            StartTime = default,
            ExpectedReturnTime = default,
            Count = 0,
            IssuedPenalties = 0,
            PaidPenalties = 0
        };

        var borrowRepositoryMock = new Mock<IGenericRepository<Borrow>>();
        borrowRepositoryMock.Setup(r => r.GetById(borrow.Id)).Returns(borrow);
        _unitOfWorkMock.Setup(u => u.GetRepository<Borrow>()).Returns(borrowRepositoryMock.Object);

        // Act
        await _borrowService.UndoReturnBorrow(borrow.Id);

        // Assert
        Assert.Null(borrow.ActualReturnTime);
        borrowRepositoryMock.Verify(r => r.Update(It.IsAny<Borrow>()), Times.Once);
    }
}
