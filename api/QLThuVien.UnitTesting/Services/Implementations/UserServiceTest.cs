using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;
using QLThuVien.Data.Repositories;

namespace QLThuVien.UnitTesting.Services.Implementations;

[TestFixture]
[TestOf(typeof(UserService))]
public class UserServiceTest
{

    private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<UserService>> _loggerMock;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<RoleManager<Role>> _roleManagerMock;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userManagerMock = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                null, null, null, null, null, null, null, null
            );
            _roleManagerMock = new Mock<RoleManager<Role>>(
                new Mock<IRoleStore<Role>>().Object,
                null, null, null, null
            );
            _userService = new UserService(_unitOfWorkMock.Object, _loggerMock.Object, _userManagerMock.Object, _roleManagerMock.Object);
        }

        [Test]
        public async Task GetAllAsyncVms_ShouldReturnUserVms()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "user1",
                    Email = "user1@example.com",
                    FullName = "User One",
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    Gender = "M",
                    Roles = new List<Role> { new Role { Name = "Admin" } }
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "user2",
                    Email = "user2@example.com",
                    FullName = "User Two",
                    DateOfBirth = new DateOnly(1992, 2, 2),
                    Gender = "F",
                    Roles = new List<Role> { new Role { Name = "User" } }
                }
            };

            var userRepositoryMock = new Mock<IGenericRepository<User>>();
            userRepositoryMock.Setup(r => r.GetQuery()).Returns(users.AsQueryable());
            _unitOfWorkMock.Setup(u => u.GetRepository<User>()).Returns(userRepositoryMock.Object);

            // Act
            var result = await _userService.GetAllAsyncVms();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<UserVm>>(result);
            var userList = result.ToList();
            Assert.AreEqual(2, userList.Count);
            Assert.AreEqual("user1", userList[0].Email);
            Assert.AreEqual("Admin", userList[0].Role);
            Assert.AreEqual("user2", userList[1].Email);
            Assert.AreEqual("User", userList[1].Role);
        }

        [Test]
        public async Task CreateAsync_ShouldCreateUser()
        {
            // Arrange
            var userCreateVm = new UserCreateVm
            {
                Email = "newuser@example.com",
                Password = "Password123!",
                FullName = "New User",
                DateOfBirth = new DateOnly(1995, 5, 5),
                Gender = "M",
                Role = "User"
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(userCreateVm.Email)).ReturnsAsync((User)null);
            _roleManagerMock.Setup(rm => rm.FindByNameAsync(userCreateVm.Role)).ReturnsAsync(new Role { Name = userCreateVm.Role });

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), userCreateVm.Password))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _userService.CreateAsync(userCreateVm);

            // Assert
            _userManagerMock.Verify(um => um.CreateAsync(It.Is<User>(u =>
                u.Email == userCreateVm.Email &&
                u.UserName == userCreateVm.Email &&
                u.FullName == userCreateVm.FullName &&
                u.DateOfBirth == userCreateVm.DateOfBirth &&
                u.Gender == userCreateVm.Gender &&
                u.Roles.First().Name == userCreateVm.Role
            ), userCreateVm.Password), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Gender = "Nu",
                DateOfBirth = default,
                FullName = null
            };
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            await _userService.DeleteAsync(userId);

            // Assert
            _userManagerMock.Verify(um => um.DeleteAsync(user), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userEditVm = new UserEditVm
            {
                Email = "updateduser@example.com",
                FullName = "Updated User",
                DateOfBirth = new DateOnly(1994, 4, 4),
                Gender = "F"
            };

            var user = new User
            {
                Id = userId,
                Gender = null,
                DateOfBirth = default,
                FullName = null
            };
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            await _userService.UpdateAsync(userId, userEditVm);

            // Assert
            _userManagerMock.Verify(um => um.UpdateAsync(It.Is<User>(u =>
                u.Id == userId &&
                u.Email == userEditVm.Email &&
                u.FullName == userEditVm.FullName &&
                u.DateOfBirth == userEditVm.DateOfBirth &&
                u.Gender == userEditVm.Gender
            )), Times.Once);
        }

        [Test]
        public async Task ChangePasswordAsync_ShouldChangePassword()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "OldPassword123!";
            var newPassword = "NewPassword123!";

            var user = new User
            {
                Id = userId,
                Gender = "Nam",
                DateOfBirth = default,
                FullName = null
            };
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.ChangePasswordAsync(user, currentPassword, newPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _userService.ChangePasswordAsync(userId, currentPassword, newPassword);

            // Assert
            _userManagerMock.Verify(um => um.ChangePasswordAsync(user, currentPassword, newPassword), Times.Once);
        }

        [Test]
        public async Task GetByIdAsyncVm_ShouldReturnUserVm()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Email = "user@example.com",
                FullName = "User",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Gender = "M",
                Roles = new List<Role> { new Role { Name = "Admin" } }
            };

            _userManagerMock.Setup(um => um.Users).Returns(new List<User> { user }.AsQueryable());
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString())).ReturnsAsync(user);

            // Act
            var result = await _userService.GetByIdAsyncVm(userId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual(user.FullName, result.FullName);
            Assert.AreEqual(user.DateOfBirth, result.DateOfBirth);
            Assert.AreEqual(user.Gender, result.Gender);
            Assert.AreEqual(user.Roles.First().Name, result.Role);
        }
}