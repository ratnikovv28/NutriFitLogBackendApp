using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Moq;
using AutoFixture.Xunit2;
using NutriFitLogBackend.Application.Services.Users;
using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Domain.DTOs.Users;
using NutriFitLogBackend.Domain.DTOs.Users.RequestDTOs;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Domain.Exceptions;
using NutriFitLogBackend.Infrastructure.Mapper;
using Xunit;

public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly IMapper _mapper;
    private readonly UserService _sut;
    private readonly IFixture _fixture;

    public UserServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = new Mapper(config);
        _sut = new UserService(_uowMock.Object, _mapper);
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }

    [Theory, AutoData]
    public async Task CreateUser_WhenUserDoesNotExist_CreatesUser(CreateUserDto createUserDto, long telegramId)
    {
        // Arrange
        var user = new User(telegramId);
        _uowMock.Setup(u => u.UserRepository.ExistAsync(createUserDto.TelegramId)).ReturnsAsync(false);
        _uowMock.Setup(u => u.UserRepository.AddAsync(It.IsAny<User>())).ReturnsAsync(user);
        _uowMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.CreateUser(createUserDto);

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<UserDto>(user));
    }

    [Theory, AutoData]
    public async Task CreateUser_WhenUserExists_ThrowsConflictException(CreateUserDto createUserDto)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.ExistAsync(createUserDto.TelegramId)).ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _sut.CreateUser(createUserDto);

        // Assert
        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage($"The user with the ID = {createUserDto.TelegramId} already exists.");
    }

    [Theory, AutoData]
    public async Task GetUserByTelegramId_WhenUserExists_ReturnsUserDto(long telegramId)
    {
        // Arrange
        var user = new User(telegramId);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(user.TelegramId)).ReturnsAsync(user);

        // Act
        var result = await _sut.GetUserByTelegramId(user.TelegramId);

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<UserDto>(user));
    }

    [Theory, AutoData]
    public async Task GetUserByTelegramId_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.GetUserByTelegramId(telegramId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>()
            .WithMessage($"The user with the ID = {telegramId} was not found.");
    }

    [Theory, AutoData]
    public async Task GetStudents_WhenUserExists_ReturnsActiveStudents(long telegramId, List<long> ids)
    {
        // Arrange
        var fixture = new Fixture();
        var user = new User(telegramId);
        var students = new List<User>();
        foreach (var id in ids)
        {
            var tgId = fixture.Create<long>();
            students.Add(new User(tgId)
            {
                Id = id,
                TelegramId = tgId
            });
        }
        user.Students.AddRange(students.Select(s => new StudentTrainer { StudentId = s.Id, TrainerId = telegramId, IsWorking = true }));
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(user.TelegramId)).ReturnsAsync(user);
        foreach (var student in students)
        {
            _uowMock.Setup(u => u.UserRepository.GetByIdAsync(student.Id)).ReturnsAsync(student);
        }

        // Act
        var result = await _sut.GetStudents(user.TelegramId, true);

        // Assert
        result.OrderBy(x => x.TelegramId).Should().BeEquivalentTo(_mapper.Map<IReadOnlyCollection<UserDto>>(students.OrderBy(x => x.TelegramId)));
    }

    [Theory, AutoData]
    public async Task GetStudents_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.GetStudents(telegramId, true);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>()
            .WithMessage($"The user with the ID = {telegramId} was not found.");
    }

    [Theory, AutoData]
    public async Task GetTrainers_WhenUserExists_ReturnsTrainers(long telegramId, List<long> ids)
    {
        // Arrange
        var fixture = new Fixture();
        var user = new User(telegramId);
        var trainers = new List<User>();
        foreach (var id in ids)
        {
            var tgId = fixture.Create<long>();
            trainers.Add(new User(tgId)
            {
                Id = id,
                TelegramId = tgId
            });
        }
        user.Trainers.AddRange(trainers.Select(t => new StudentTrainer { TrainerId = t.Id, IsWorking = true }));
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(user.TelegramId)).ReturnsAsync(user);
        foreach (var trainer in trainers)
        {
            _uowMock.Setup(u => u.UserRepository.GetByIdAsync(trainer.Id)).ReturnsAsync(trainer);
        }

        // Act
        var result = await _sut.GetTrainers(user.TelegramId);

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<IReadOnlyCollection<UserDto>>(trainers));
    }

    [Theory, AutoData]
    public async Task GetTrainers_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.GetTrainers(telegramId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>()
            .WithMessage($"The user with the ID = {telegramId} was not found.");
    }

    [Theory, AutoData]
    public async Task UpdateTrainerStatus_WhenUserExists_UpdatesStatus(long telegramId)
    {
        // Arrange
        var user = new User(telegramId);
        var userStatus = user.IsActiveTrainer;
        _uowMock.Setup(u => u.UserRepository.GetJustByTelegramIdAsync(user.TelegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.UpdateTrainerStatus(user.TelegramId);

        // Assert
        user.IsActiveTrainer.Should().Be(!userStatus);
        result.Should().BeEquivalentTo(_mapper.Map<UserDto>(user));
    }

    [Theory, AutoData]
    public async Task UpdateTrainerStatus_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetJustByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.UpdateTrainerStatus(telegramId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>()
            .WithMessage($"The user with the ID = {telegramId} was not found.");
    }

    [Theory, AutoData]
    public async Task DeleteStudentTrainerRelationShip_WhenCalled_DeletesRelation(long trainerId, long studentId)
    {
        // Arrange
        var student = new User(studentId);
        var trainer = new User(studentId);
        var studentTrainer = new StudentTrainer
        {
            IsWorking = true,
            StudentId = studentId,
            TrainerId = trainerId
        };
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(student.TelegramId)).ReturnsAsync(student);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainer.TelegramId)).ReturnsAsync(trainer);
        _uowMock.Setup(u => u.StudentTrainerRepository.GetRelationShip(student.Id, trainer.Id)).ReturnsAsync(studentTrainer);
        _uowMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        await _sut.DeleteStudentTrainerRelationShip(student.TelegramId, trainer.TelegramId);

        // Assert
        _uowMock.Verify(u => u.StudentTrainerRepository.DeleteRelationShip(studentTrainer), Times.Once);
        _uowMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Theory, AutoData]
    public async Task AddStudentToTrainer_WhenTrainerDoesNotExist_ThrowsUserNotFoundException(long studentId, long trainerId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainerId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.AddStudentToTrainer(studentId, trainerId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>()
            .WithMessage($"The user with the ID = {trainerId} was not found.");
    }

    [Theory, AutoData]
    public async Task AddStudentToTrainer_WhenTrainerIsNotActive_ThrowsException(long telegramId)
    {
        // Arrange
        var trainer = new User(telegramId);
        trainer.IsActiveTrainer = false;
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainer.TelegramId)).ReturnsAsync(trainer);

        // Act
        Func<Task> act = async () => await _sut.AddStudentToTrainer(_fixture.Create<long>(), trainer.TelegramId);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"Trainer with ID = '{trainer.TelegramId}' doesnt work right now");
    }

    [Theory, AutoData]
    public async Task AddStudentToTrainer_WhenStudentDoesNotExist_ThrowsUserNotFoundException(long trainerId, long studentId)
    {
        // Arrange
        var trainer = new User(trainerId);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainer.TelegramId)).ReturnsAsync(trainer);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(studentId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.AddStudentToTrainer(studentId, trainer.TelegramId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>()
            .WithMessage($"The user with the ID = {studentId} was not found.");
    }

    [Theory, AutoData]
    public async Task AddStudentToTrainer_WhenRelationExists_ThrowsException(long studentId, long trainerId)
    {
        // Arrange
        var student = new User(studentId);
        var trainer = new User(studentId);
        var studentTrainer = new StudentTrainer
        {
            IsWorking = true,
            StudentId = studentId,
            TrainerId = trainerId
        };
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainer.TelegramId)).ReturnsAsync(trainer);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(student.TelegramId)).ReturnsAsync(student);
        _uowMock.Setup(u => u.StudentTrainerRepository.GetRelationShip(student.Id, trainer.Id)).ReturnsAsync(studentTrainer);

        // Act
        Func<Task> act = async () => await _sut.AddStudentToTrainer(student.TelegramId, trainer.TelegramId);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"Relationship between Student with ID = '{student.TelegramId}' and Trainer with ID = '{trainer.TelegramId}' already exists");
    }

    [Theory, AutoData]
    public async Task AddStudentToTrainer_WhenCalled_AddsRelation(long studentId, long trainerId)
    {
        // Arrange
        var student = new User(studentId);
        var trainer = new User(studentId);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainer.TelegramId)).ReturnsAsync(trainer);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(student.TelegramId)).ReturnsAsync(student);
        _uowMock.Setup(u => u.StudentTrainerRepository.GetRelationShip(student.Id, trainer.Id)).ReturnsAsync((StudentTrainer)null);
        _uowMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        await _sut.AddStudentToTrainer(student.TelegramId, trainer.TelegramId);

        // Assert
        _uowMock.Verify(u => u.StudentTrainerRepository.AddRelationShip(It.IsAny<StudentTrainer>()), Times.Once);
        _uowMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Theory, AutoData]
    public async Task CreateStudentTrainer_WhenStudentDoesNotExist_ThrowsUserNotFoundException(long studentId, long trainerId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(studentId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.CreateStudentTrainer(studentId, trainerId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>()
            .WithMessage($"The user with the ID = {studentId} was not found.");
    }

    [Theory, AutoData]
    public async Task CreateStudentTrainer_WhenTrainerDoesNotExist_ThrowsUserNotFoundException(long studentId, long trainerId)
    {
        // Arrange
        var student = new User(studentId);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(studentId)).ReturnsAsync(student);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainerId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.CreateStudentTrainer(studentId, trainerId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>()
            .WithMessage($"The user with the ID = {trainerId} was not found.");
    }

    [Theory, AutoData]
    public async Task CreateStudentTrainer_WhenRelationDoesNotExist_ThrowsException(long studentId, long trainerId)
    {
        // Arrange
        var student = new User(studentId);
        var trainer = new User(studentId);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(studentId)).ReturnsAsync(student);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainerId)).ReturnsAsync(trainer);
        _uowMock.Setup(u => u.StudentTrainerRepository.GetRelationShip(student.Id, trainer.Id)).ReturnsAsync((StudentTrainer)null);

        // Act
        Func<Task> act = async () => await _sut.CreateStudentTrainer(studentId, trainerId);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"Relationship between Student with ID = '{studentId}' and Trainer with ID = '{trainerId}' not exists");
    }

    [Theory, AutoData]
    public async Task CreateStudentTrainer_WhenCalled_UpdatesRelation(long studentId, long trainerId)
    {
        // Arrange
        var student = new User(studentId);
        var trainer = new User(studentId);
        var studentTrainer = new StudentTrainer
        {
            IsWorking = true,
            StudentId = studentId,
            TrainerId = trainerId
        };
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(studentId)).ReturnsAsync(student);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainerId)).ReturnsAsync(trainer);
        _uowMock.Setup(u => u.StudentTrainerRepository.GetRelationShip(student.Id, trainer.Id)).ReturnsAsync(studentTrainer);
        _uowMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        await _sut.CreateStudentTrainer(studentId, trainerId);

        // Assert
        studentTrainer.IsWorking.Should().BeTrue();
        _uowMock.Verify(u => u.StudentTrainerRepository.UpdateRelationShip(studentTrainer), Times.Once);
        _uowMock.Verify(u => u.SaveAsync(), Times.Once);
    }
}