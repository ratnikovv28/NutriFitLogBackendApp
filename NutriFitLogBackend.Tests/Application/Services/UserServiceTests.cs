using AutoMapper;
using Moq;
using NutriFitLogBackend.Application.Services.Users;
using NutriFitLogBackend.Domain;

namespace NutriFitLogBackend.Tests.Application.Services;

public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new ();
    private readonly Mock<IMapper> _mapperMock = new ();

    private readonly UserService _sut;

    public UserServiceTests()
    {
        _sut = new UserService(_uowMock.Object, _mapperMock.Object);
    }
}