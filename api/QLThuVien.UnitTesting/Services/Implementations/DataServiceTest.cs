using Microsoft.Extensions.Logging;
using Moq;
using QLThuVien.Business.Exceptions;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;

namespace QLThuVien.UnitTesting.Services.Implementations;

[TestFixture]
[TestOf(typeof(DataService<Book>))]
public class DataServiceTest
{

    private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<DataService<Book>>> _loggerMock;
        private DataService<Book> _dataService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DataService<Book>>>();
            _dataService = new DataService<Book>(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task AddAsync_ShouldThrowArgumentNullException_WhenEntityIsNull()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => _dataService.AddAsync(null));
            Assert.AreEqual("entity", ex.ParamName);
        }

        [Test]
        public async Task AddAsync_ShouldLogErrorAndThrowBadRequestException_WhenSaveChangesFails()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                AuthorName = "Author Name",
                Title = "Book Title",
                PublisherName = "Publisher Name",
                PublishDate = new DateOnly(2023, 8, 30),
                Count = 1,
            };
            _unitOfWorkMock.Setup(u => u.GetRepository<Book>().Add(book));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(0);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(() => _dataService.AddAsync(book));
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error, 
                    It.IsAny<EventId>(), 
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Book add failed!")), 
                    null, 
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public async Task AddAsync_ShouldLogInformation_WhenEntityIsAddedSuccessfully()
        {
            // Arrange
            var book = new Book
            {
                Id = Guid.NewGuid(),
                AuthorName = "Author Name",
                Title = "Book Title",
                PublisherName = "Publisher Name",
                PublishDate = new DateOnly(2023, 8, 30),
                Count = 1,
            };
            _unitOfWorkMock.Setup(u => u.GetRepository<Book>().Add(book));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await _dataService.AddAsync(book);

            // Assert
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information, 
                    It.IsAny<EventId>(), 
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Book added successfully!")), 
                    null, 
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
}