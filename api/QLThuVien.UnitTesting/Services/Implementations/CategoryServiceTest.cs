using Microsoft.Extensions.Logging;
using Moq;
using QLThuVien.Business.Models;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Repositories;

namespace QLThuVien.UnitTesting.Services.Implementations;

[TestFixture]
[TestOf(typeof(CategoryService))]
public class CategoryServiceTest
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ILogger<CategoryService>> _loggerMock;
    private CategoryService _categoryService;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CategoryService>>();
        _categoryService = new CategoryService(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task AddAsync_ShouldAddCategory()
    {
        // Arrange
        var categoryEditVm = new CategoryEditVm
        {
            Name = "Fiction",
            Description = "Books related to fictional content"
        };

        var categoryRepositoryMock = new Mock<IGenericRepository<Category>>();
        _unitOfWorkMock.Setup(u => u.GetRepository<Category>()).Returns(categoryRepositoryMock.Object);

        // Act
        await _categoryService.AddAsync(categoryEditVm);

        // Assert
        categoryRepositoryMock.Verify(r => r.Add(It.Is<Category>(c =>
            c.Name == categoryEditVm.Name &&
            c.Description == categoryEditVm.Description)), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdateCategory()
    {
        // Arrange
        var id = Guid.NewGuid();
        var categoryEditVm = new CategoryEditVm
        {
            Name = "Science Fiction",
            Description = "Books related to science fiction"
        };

        var categoryRepositoryMock = new Mock<IGenericRepository<Category>>();
        _unitOfWorkMock.Setup(u => u.GetRepository<Category>()).Returns(categoryRepositoryMock.Object);

        // Act
        await _categoryService.UpdateAsync(id, categoryEditVm);

        // Assert
        categoryRepositoryMock.Verify(r => r.Update(It.Is<Category>(c =>
            c.Id == id &&
            c.Name == categoryEditVm.Name &&
            c.Description == categoryEditVm.Description)), Times.Once);
    }
}