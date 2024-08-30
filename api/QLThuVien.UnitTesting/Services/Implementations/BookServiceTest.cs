using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using QLThuVien.Business.Exceptions;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;

namespace QLThuVien.UnitTesting.Services.Implementations;

[TestFixture]
[TestOf(typeof(BookService))]
public class BookServiceTest
{

     private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ILogger<BookService>> _loggerMock;
    private BookService _bookService;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<BookService>>();
        _bookService = new BookService(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetAll_ShouldReturnListOfBookVm()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = Guid.NewGuid(), Title = "Book1", AuthorName = "Author1", PublisherName = "Publisher1", PublishDate = new DateOnly(2024, 1, 1), Count = 10, Category = new Category { Name = "Category1" }, Ratings = new List<Rating> { new Rating { Value = 4.5f } } }
        }.AsQueryable();

        _unitOfWorkMock.Setup(u => u.GetRepository<Book>().GetQuery())
            .Returns(books);

        // Act
        var result = await _bookService.GetAll();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("Book1", result.First().Title);
    }

    [Test]
    public async Task QueryAsyncVm_ShouldReturnPaginatedResult()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = Guid.NewGuid(), Title = "Book1", AuthorName = "Author1", PublisherName = "Publisher1", PublishDate = new DateOnly(2024, 1, 1), Count = 10, Category = new Category { Name = "Category1" }, Ratings = new List<Rating> { new Rating { Value = 4.5f } } }
        }.AsQueryable();

        _unitOfWorkMock.Setup(u => u.GetRepository<Book>().Get(It.IsAny<Expression<Func<Book, bool>>>(), null, "Category,Ratings"))
            .Returns(books);

        // Act
        var result = await _bookService.QueryAsyncVm(pageIndex: 1, pageSize: 10);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Items.Count);
        Assert.AreEqual("Book1", result.Items.First().Title);
    }

    [Test]
    public async Task GetByIdAsyncVm_ShouldReturnBookVm_WhenBookExists()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var book = new Book { Id = bookId, Title = "Book1", AuthorName = "Author1", PublisherName = "Publisher1", PublishDate = new DateOnly(2024, 1, 1), Count = 10, Category = new Category { Name = "Category1" }, Ratings = new List<Rating> { new Rating { Value = 4.5f } } };

        _unitOfWorkMock.Setup(u => u.GetRepository<Book>().Get(It.IsAny<Expression<Func<Book, bool>>>(), null, "Category,Ratings"))
            .Returns(new List<Book> { book }.AsQueryable());

        // Act
        var result = await _bookService.GetByIdAsyncVm(bookId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Book1", result.Title);
    }

    [Test]
    public void GetByIdAsyncVm_ShouldThrowNotFoundException_WhenBookDoesNotExist()
    {
        // Arrange
        var bookId = Guid.NewGuid();

        _unitOfWorkMock.Setup(u => u.GetRepository<Book>().Get(It.IsAny<Expression<Func<Book, bool>>>(), null, "Category,Ratings"))
            .Returns(Enumerable.Empty<Book>().AsQueryable());

        // Act & Assert
        Assert.ThrowsAsync<NotFoundException>(() => _bookService.GetByIdAsyncVm(bookId));
    }

    [Test]
    public async Task AddAsync_ShouldAddBookSuccessfully()
    {
        // Arrange
        var bookEditVm = new BookEditVm
        {
            AuthorName = "Author1",
            Title = "Book1",
            PublisherName = "Publisher1",
            PublishDate = new DateOnly(2024, 1, 1),
            Count = 10,
            CategoryName = "Category1"
        };

        var category = new Category { Name = "Category1" };

        _unitOfWorkMock.Setup(u => u.GetRepository<Category>()
                .Get(It.IsAny<Expression<Func<Category, bool>>>(), null, null))
            .Returns<Expression<Func<Category, bool>>, Func<IQueryable<Category>, IOrderedQueryable<Category>>, string>((predicate, orderBy, includes) =>
            {
                // Example predefined category for testing
                var predefinedCategory = new Category { Name = "Category1" };

                // Return a category that matches the predicate
                return new List<Category> { predefinedCategory }.AsQueryable()
                    .Where(predicate.Compile())
                    .AsQueryable();
            });

        _unitOfWorkMock.Setup(u => u.GetRepository<Book>().Add(It.IsAny<Book>()));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        await _bookService.AddAsync(bookEditVm);

        // Assert
        _unitOfWorkMock.Verify(u => u.GetRepository<Book>().Add(It.IsAny<Book>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task AddAsync_ShouldThrowBadRequestException_WhenCategoryNotFound()
    {
        // Arrange
        var bookEditVm = new BookEditVm
        {
            AuthorName = "Author1",
            Title = "Book1",
            PublisherName = "Publisher1",
            PublishDate = new DateOnly(2024, 1, 1),
            Count = 10,
            CategoryName = "NonExistentCategory"
        };

        // Mock the repository to return an empty IQueryable for Category
        _unitOfWorkMock.Setup(u => u.GetRepository<Category>()
                .Get(It.IsAny<Expression<Func<Category, bool>>>(), null, null))
            .Returns<Expression<Func<Category, bool>>, Func<IQueryable<Category>, IOrderedQueryable<Category>>, string>((predicate, orderBy, includes) =>
            {
                // Return an empty IQueryable to simulate that no categories match the predicate
                return Enumerable.Empty<Category>().AsQueryable();
            });

        // Act & Assert
        Assert.ThrowsAsync<BadRequestException>(() => _bookService.AddAsync(bookEditVm));
    }
    
    [Test]
    public async Task UpdateAsync_ShouldUpdateBookSuccessfully()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var bookEditVm = new BookEditVm
        {
            AuthorName = "UpdatedAuthor",
            Title = "UpdatedBook",
            PublisherName = "UpdatedPublisher",
            PublishDate = new DateOnly(2024, 1, 1),
            Count = 15,
            CategoryName = "UpdatedCategory"
        };

        var category = new Category { Name = "UpdatedCategory" };

        _unitOfWorkMock.Setup(u => u.GetRepository<Category>()
                .Get(It.IsAny<Expression<Func<Category, bool>>>(), null, null))
            .Returns<Expression<Func<Category, bool>>, Func<IQueryable<Category>, IOrderedQueryable<Category>>, string>((predicate, orderBy, includes) =>
            {
                var categories = new List<Category>
                {
                    category // Return a list containing the category
                }.AsQueryable();

                return categories.Where(predicate);
            });

        _unitOfWorkMock.Setup(u => u.GetRepository<Book>().Update(It.IsAny<Book>()));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        await _bookService.UpdateAsync(bookId, bookEditVm);

        // Assert
        _unitOfWorkMock.Verify(u => u.GetRepository<Book>().Update(It.IsAny<Book>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_ShouldThrowBadRequestException_WhenCategoryNotFound()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var bookEditVm = new BookEditVm
        {
            AuthorName = "UpdatedAuthor",
            Title = "UpdatedBook",
            PublisherName = "UpdatedPublisher",
            PublishDate = new DateOnly(2024, 1, 1),
            Count = 15,
            CategoryName = "NonExistentCategory"
        };

        _unitOfWorkMock.Setup(u => u.GetRepository<Category>()
                .Get(It.IsAny<Expression<Func<Category, bool>>>(), null, null))
            .Returns<Expression<Func<Category, bool>>, Func<IQueryable<Category>, IOrderedQueryable<Category>>, string>((predicate, orderBy, includes) =>
            {
                // Return an empty IQueryable to simulate no categories found
                return Enumerable.Empty<Category>().AsQueryable();
            });

        // Act & Assert
        Assert.ThrowsAsync<BadRequestException>(() => _bookService.UpdateAsync(bookId, bookEditVm));
    }
}