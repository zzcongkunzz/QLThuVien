using Microsoft.Extensions.Logging;
using Moq;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using QLThuVien.Data.Repositories;

namespace QLThuVien.UnitTesting.Services.Implementations;

[TestFixture]
[TestOf(typeof(RoleService))]
public class RoleServiceTest
{

    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ILogger<RoleService>> _loggerMock;
    private RoleService _roleService;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<RoleService>>();
        _roleService = new RoleService(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetAllAsyncVm_ShouldReturnRoleVms()
    {
        // Arrange
        var roles = new List<Role>
        {
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                Description = "Administrator role"
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "User",
                Description = "Standard user role"
            }
        };

        var roleRepositoryMock = new Mock<IGenericRepository<Role>>();
        roleRepositoryMock.Setup(r => r.GetQuery()).Returns(roles.AsQueryable());

        _unitOfWorkMock.Setup(u => u.GetRepository<Role>()).Returns(roleRepositoryMock.Object);

        // Act
        var result = await _roleService.GetAllAsyncVm();

        // Assert
        Assert.NotNull(result);
        Assert.IsInstanceOf<IEnumerable<RoleVm>>(result);
        var roleList = result.ToList();
        Assert.AreEqual(2, roleList.Count);
        Assert.AreEqual("Admin", roleList[0].Name);
        Assert.AreEqual("Administrator role", roleList[0].Description);
        Assert.AreEqual("User", roleList[1].Name);
        Assert.AreEqual("Standard user role", roleList[1].Description);
    }
}