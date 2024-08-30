using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using QLThuVien.Business.Services.Implementations;
using QLThuVien.Business.Services.Interfaces;
using QLThuVien.Business.ViewModels;
using QLThuVien.Data.Infrastructure;
using QLThuVien.Data.Models;

namespace QLThuVien.UnitTesting.Services.Implementations;

[TestFixture]
[TestOf(typeof(AuthenticationService))]
public class AuthenticationServiceTest
{
    private Mock<UserManager<User>> _userManagerMock;
    private Mock<RoleManager<Role>> _roleManagerMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IConfiguration> _configurationMock;
    private Mock<IUserService> _userServiceMock;
    private AuthenticationService _authService;

    [SetUp]
    public void Setup()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        var roleStoreMock = new Mock<IRoleStore<Role>>();
        _roleManagerMock = new Mock<RoleManager<Role>>(roleStoreMock.Object, null, null, null, null);
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _configurationMock = new Mock<IConfiguration>();
        _userServiceMock = new Mock<IUserService>();

        _authService = new AuthenticationService(
            _userManagerMock.Object, 
            _roleManagerMock.Object, 
            _unitOfWorkMock.Object, 
            _configurationMock.Object, 
            _userServiceMock.Object);
    }
    
    [Test]
    public async Task Login_ShouldReturnAuthResult_WhenUserExistsAndPasswordIsCorrect()
    {
        // Arrange
        var payload = new LoginVM
        {
            Email = "admin@gmail.com",
            Password = "Admin_123"
        };

        var user = new User
        {
            UserName = "testuser",
            Email = payload.Email,
            Id = Guid.NewGuid(),
            FullName = "John Doe",
            DateOfBirth = new DateOnly(1990, 1, 1),
            Gender = "Male",
        };

        _userManagerMock.Setup(u => u.FindByEmailAsync(payload.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.CheckPasswordAsync(user, payload.Password)).ReturnsAsync(true);
        _configurationMock.Setup(c => c["JWT:Secret"]).Returns("YourJwtSecretKey"); // Replace with your secret
        _configurationMock.Setup(c => c["JWT:Issuer"]).Returns("YourJwtIssuer");
        _configurationMock.Setup(c => c["JWT:Audience"]).Returns("YourJwtAudience");

        var mockJwtHandler = new Mock<JwtSecurityTokenHandler>();
        mockJwtHandler.Setup(j => j.WriteToken(It.IsAny<JwtSecurityToken>())).Returns("mocked_jwt_token");
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
        _userServiceMock
            .Setup(u => u.GetByIdAsyncVm(It.IsAny<Guid>()))
            .ReturnsAsync(new UserVm
            {
                Id = user.Id,
                Email = user.Email,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                FullName = user.FullName,
                Role = ""
            });

        // Act
        var result = await _authService.Login(payload);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("mocked_jwt_token", result.Token);
        _userManagerMock.Verify(u => u.FindByEmailAsync(payload.Email), Times.Once);
        _userManagerMock.Verify(u => u.CheckPasswordAsync(user, payload.Password), Times.Once);
    }

    [Test]
    public async Task Login_ShouldReturnNullToken_WhenUserDoesNotExistOrPasswordIsIncorrect()
    {
        // Arrange
        var payload = new LoginVM
        {
            Email = "nonexistent@example.com",
            Password = "InvalidPassword"
        };

        _userManagerMock.Setup(u => u.FindByEmailAsync(payload.Email)).ReturnsAsync((User)null);

        // Act
        var result = await _authService.Login(payload);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNull(result.Token);
        Assert.IsNull(result.RefreshToken);
        _userManagerMock.Verify(u => u.FindByEmailAsync(payload.Email), Times.Once);
        _userManagerMock.Verify(u => u.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
    }
}