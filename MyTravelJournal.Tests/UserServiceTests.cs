using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Exceptions;
using MyTravelJournal.Api.Models;
using MyTravelJournal.Api.Repositories.UserRepository;
using MyTravelJournal.Api.Services.TripService;
using MyTravelJournal.Api.Services.UserService;
using Xunit;

namespace MyTravelJournal.Tests;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<ITripService> _tripServiceMock = new();

    public UserServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<User, UserDetailsResponse>()
        );

        var mapper = config.CreateMapper();

        _sut = new UserService(
            mapper: mapper,
            tripService: _tripServiceMock.Object,
            userRepository: _userRepoMock.Object
        );
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var userFirstName = "Tom";
        var userLastName = "Smith";
        var userEmail = "tom.smith@example.com";
        var userRole = "User";
        var userUsername = "tommy.smith";

        var expected = new User
        {
            UserId = userId,
            FirstName = userFirstName,
            LastName = userLastName,
            Email = userEmail,
            Role = userRole,
            Username = userUsername
        };

        _userRepoMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(expected);

        // Act
        var actual = await _sut.GetByIdAsync(userId);

        // Assert
        Assert.Equal(expected.UserId, actual.UserId);
        Assert.Equal(expected.FirstName, actual.FirstName);
        Assert.Equal(expected.LastName, actual.LastName);
        Assert.Equal(expected.Email, actual.Email);
        Assert.Equal(expected.Role, actual.Role);
        Assert.Equal(expected.Username, actual.Username);
    }


    [Fact]
    public async Task GetByIdAsync_ShouldThrow_WhenUserDoesntExist()
    {
        // Arrange
        var userId = 0;

        _userRepoMock
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(null as Func<User?>);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetByIdAsync(userId));
    }
}