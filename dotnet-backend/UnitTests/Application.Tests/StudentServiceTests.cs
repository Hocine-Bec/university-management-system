using Applications.DTOs;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Applications.Tests.Services
{
    //public class StudentServiceTests
    //{
    //    private const int ValidStudentId = 1;
    //    private const string ValidLastName = "Smith";
    //    private readonly Mock<IStudentRepository> _mockRepository;
    //    private readonly Mock<IMapper> _mockMapper;
    //    private readonly Mock<IMyLogger> _mockLogger;
    //    private readonly StudentService _service;

    //    public StudentServiceTests()
    //    {
    //        _mockRepository = new Mock<IStudentRepository>();
    //        _mockMapper = new Mock<IMapper>();
    //        _mockLogger = new Mock<IMyLogger>();
    //        _service = new StudentService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    //    }

    //    #region GetAllAsync
    //    [Fact]
    //    public async Task GetAllAsync_WhenStudentsExist_ReturnsSuccessWithStudents()
    //    {
    //        // Arrange
    //        var students = Enumerable.Range(1, 500).Select(i => new Student()
    //        {
    //            Id = i,
    //            FName = $"First Name {i}",
    //            LName = $"Last Name {i}"
    //        }).ToList();

    //        var studentsDto = Enumerable.Range(1, 500).Select(i => new StudentDto()
    //        {
    //            Id = i,
    //            FName = $"First Name {i}",
    //            LName = $"Last Name {i}"
    //        }).ToList();
            

    //        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(students);
    //        _mockMapper.Setup(m => m.Map<IReadOnlyCollection<StudentDto>>(students))
    //                  .Returns(studentsDto);

    //        // Act
    //        var result = await _service.GetAllAsync();

    //        // Assert
    //        result.IsSuccess.Should().BeTrue();
    //        result.Value.Should().BeEquivalentTo(studentsDto);
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task GetAllAsync_WhenNoStudentsExist_ReturnsFailure()
    //    {
    //        // Arrange
    //        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Student>());

    //        // Act
    //        var result = await _service.GetAllAsync();

    //        // Assert
    //        result.Value.Should().BeNull();
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("No students found in the system");
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task GetAllAsync_WhenRepositoryThrowsException_ReturnsFailureAndLogsError()
    //    {
    //        // Arrange
    //        var exception = new Exception("Database error");
    //        _mockRepository.Setup(r => r.GetAllAsync()).ThrowsAsync(exception);
    //        _mockLogger.Setup(x => x.LogError(It.IsAny<string>(), new Exception(), null));

    //        // Act
    //        var result = await _service.GetAllAsync();

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Failed to retrieve students due to a system error");
    //        _mockLogger.Verify(x => x.LogError(
    //            "Database error retrieving all student", exception, null), Times.Once);
    //    }
    //    #endregion

    //    #region AddAsync
    //    [Fact]
    //    public async Task AddAsync_WhenStudentDtoIsValid_ReturnsSuccessWithId()
    //    {
    //        // Arrange
    //        var studentDto = CreateTestStudentDto();
    //        var student = CreateTestStudent();

    //        _mockMapper.Setup(m => m.Map<Student>(studentDto)).Returns(student);
    //        _mockRepository.Setup(r => r.AddAsync(student)).ReturnsAsync(ValidStudentId);

    //        // Act
    //        var result = await _service.AddAsync(studentDto);

    //        // Assert
    //        result.IsSuccess.Should().BeTrue();
    //        result.Value.Should().Be(ValidStudentId);
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task AddAsync_WhenStudentDtoIsNull_ReturnsFailure()
    //    {
    //        // Act
    //        var result = await _service.AddAsync(null!);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Student information is required");
    //        _mockRepository.VerifyNoOtherCalls();
    //        _mockMapper.VerifyNoOtherCalls();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task AddAsync_WhenRepositoryReturnsZero_ReturnsFailure()
    //    {
    //        // Arrange
    //        var studentDto = CreateTestStudentDto();
    //        var student = CreateTestStudent();

    //        _mockMapper.Setup(m => m.Map<Student>(studentDto)).Returns(student);
    //        _mockRepository.Setup(r => r.AddAsync(student)).ReturnsAsync(0);

    //        // Act
    //        var result = await _service.AddAsync(studentDto);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Failed to create new student record");
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task AddAsync_WhenRepositoryThrowsException_ReturnsFailureAndLogsError()
    //    {
    //        // Arrange
    //        var studentDto = CreateTestStudentDto();
    //        var student = CreateTestStudent();
    //        var exception = new Exception("Database error");

    //        _mockMapper.Setup(m => m.Map<Student>(studentDto)).Returns(student);
    //        _mockRepository.Setup(r => r.AddAsync(student)).ThrowsAsync(exception);
    //        _mockLogger.Setup(l => l.LogError(It.IsAny<string>(), new Exception(), new StudentDto()));

    //        // Act
    //        var result = await _service.AddAsync(studentDto);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Failed to create student due to a system error");
    //        _mockLogger.Verify(l => l.LogError(
    //            "Database error adding new student", exception, It.IsAny<object>()), Times.Once);
    //    }
    //    #endregion

    //    #region UpdateAsync
    //    [Fact]
    //    public async Task UpdateAsync_WhenStudentDtoIsValidAndUpdateSucceeds_ReturnsSuccess()
    //    {
    //        // Arrange
    //        var studentDto = CreateTestStudentDto();
    //        var student = CreateTestStudent();

    //        _mockMapper.Setup(m => m.Map<Student>(studentDto)).Returns(student);
    //        _mockRepository.Setup(r => r.UpdateAsync(student)).ReturnsAsync(true);

    //        // Act
    //        var result = await _service.UpdateAsync(studentDto);

    //        // Assert
    //        result.IsSuccess.Should().BeTrue();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task UpdateAsync_WhenStudentDtoIsNull_ReturnsFailure()
    //    {
    //        // Act
    //        var result = await _service.UpdateAsync(null!);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Student information is required for update");
    //        _mockRepository.VerifyNoOtherCalls();
    //        _mockMapper.VerifyNoOtherCalls();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task UpdateAsync_WhenUpdateFails_ReturnsFailure()
    //    {
    //        // Arrange
    //        var studentDto = CreateTestStudentDto();
    //        var student = CreateTestStudent();

    //        _mockMapper.Setup(m => m.Map<Student>(studentDto)).Returns(student);
    //        _mockRepository.Setup(r => r.UpdateAsync(student)).ReturnsAsync(false);

    //        // Act
    //        var result = await _service.UpdateAsync(studentDto);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Failed to update student");
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task UpdateAsync_WhenRepositoryThrowsException_ReturnsFailureAndLogsError()
    //    {
    //        // Arrange
    //        var studentDto = CreateTestStudentDto();
    //        var student = CreateTestStudent();
    //        var exception = new Exception("Database error");

    //        _mockMapper.Setup(m => m.Map<Student>(studentDto)).Returns(student);
    //        _mockRepository.Setup(r => r.UpdateAsync(student)).ThrowsAsync(exception);
    //        _mockLogger.Setup(x => x.LogError(It.IsAny<string>(), new Exception(), new StudentDto()));

    //        // Act
    //        var result = await _service.UpdateAsync(studentDto);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Failed to update student due to a system error");
    //        _mockLogger.Verify(l => l.LogError(
    //            "Database error updating student", exception, It.IsAny<object>()), Times.Once);
    //    }
    //    #endregion

    //    #region DeleteAsync (by ID)
    //    [Fact]
    //    public async Task DeleteAsync_ById_WhenIdIsValidAndDeleteSucceeds_ReturnsSuccess()
    //    {
    //        // Arrange
    //        _mockRepository.Setup(r => r.DeleteAsync(ValidStudentId)).ReturnsAsync(true);

    //        // Act
    //        var result = await _service.DeleteAsync(ValidStudentId);

    //        // Assert
    //        result.IsSuccess.Should().BeTrue();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task DeleteAsync_ById_WhenIdIsZero_ReturnsFailure()
    //    {
    //        // Act
    //        var result = await _service.DeleteAsync(0);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Invalid student ID provided");
    //        _mockRepository.VerifyNoOtherCalls();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task DeleteAsync_ById_WhenStudentNotFound_ReturnsFailure()
    //    {
    //        // Arrange
    //        _mockRepository.Setup(r => r.DeleteAsync(ValidStudentId)).ReturnsAsync(false);

    //        // Act
    //        var result = await _service.DeleteAsync(ValidStudentId);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Student not found");
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task DeleteAsync_ById_WhenRepositoryThrowsException_ReturnsFailureAndLogsError()
    //    {
    //        // Arrange
    //        var exception = new Exception("Database error");
    //        _mockRepository.Setup(r => r.DeleteAsync(ValidStudentId)).ThrowsAsync(exception);
    //        _mockLogger.Setup(l => l.LogError(It.IsAny<string>(), new Exception(), It.IsAny<object>()));

    //        // Act
    //        var result = await _service.DeleteAsync(ValidStudentId);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Failed to delete student due to a system error");
    //        _mockLogger.Verify(l => l.LogError(
    //            "Database error deleting student", exception, It.IsAny<object>()), Times.Once);
    //    }
    //    #endregion

    //    #region DeleteAsync (by LastName)
    //    [Fact]
    //    public async Task DeleteAsync_ByLastName_WhenLastNameIsValidAndDeleteSucceeds_ReturnsSuccess()
    //    {
    //        // Arrange
    //        _mockRepository.Setup(r => r.DeleteAsync(ValidLastName)).ReturnsAsync(true);

    //        // Act
    //        var result = await _service.DeleteAsync(ValidLastName);

    //        // Assert
    //        result.IsSuccess.Should().BeTrue();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task DeleteAsync_ByLastName_WhenLastNameIsNullOrEmpty_ReturnsFailure()
    //    {
    //        // Act
    //        var result = await _service.DeleteAsync("");

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Last name is required");
    //        _mockRepository.VerifyNoOtherCalls();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task DeleteAsync_ByLastName_WhenStudentNotFound_ReturnsFailure()
    //    {
    //        // Arrange
    //        _mockRepository.Setup(r => r.DeleteAsync(ValidLastName)).ReturnsAsync(false);

    //        // Act
    //        var result = await _service.DeleteAsync(ValidLastName);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Student not found");
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task DeleteAsync_ByLastName_WhenRepositoryThrowsException_ReturnsFailureAndLogsError()
    //    {
    //        // Arrange
    //        var exception = new Exception("Database error");
    //        _mockRepository.Setup(r => r.DeleteAsync(ValidLastName)).ThrowsAsync(exception);
    //        _mockLogger.Setup(l => l.LogError(It.IsAny<string>(), new Exception(), It.IsAny<object>()));

    //        // Act
    //        var result = await _service.DeleteAsync(ValidLastName);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Failed to delete student due to a system error");
    //        _mockLogger.Verify(l => l.LogError(
    //            "Database error deleting student", exception, It.IsAny<object>()), Times.Once);
    //    }
    //    #endregion

    //    #region DoesExistAsync (by ID)
    //    [Fact]
    //    public async Task DoesExistAsync_ById_WhenStudentExists_ReturnsSuccess()
    //    {
    //        // Arrange
    //        _mockRepository.Setup(r => r.DoesExistAsync(ValidStudentId)).ReturnsAsync(true);

    //        // Act
    //        var result = await _service.DoesExistAsync(ValidStudentId);

    //        // Assert
    //        result.IsSuccess.Should().BeTrue();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task DoesExistAsync_ById_WhenIdIsZero_ReturnsFailure()
    //    {
    //        // Act
    //        var result = await _service.DoesExistAsync(0);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Invalid student ID provided");
    //        _mockRepository.VerifyNoOtherCalls();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task DoesExistAsync_ById_WhenStudentDoesNotExist_ReturnsFailure()
    //    {
    //        // Arrange
    //        _mockRepository.Setup(r => r.DoesExistAsync(ValidStudentId)).ReturnsAsync(false);

    //        // Act
    //        var result = await _service.DoesExistAsync(ValidStudentId);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Student not found with the specified ID");
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task DoesExistAsync_ById_WhenRepositoryThrowsException_ReturnsFailureAndLogsError()
    //    {
    //        // Arrange
    //        var exception = new Exception("Database error");
    //        _mockRepository.Setup(r => r.DoesExistAsync(ValidStudentId)).ThrowsAsync(exception);
    //        _mockLogger.Setup(l => l.LogError(It.IsAny<string>(), new Exception(), It.IsAny<object>()));

    //        // Act
    //        var result = await _service.DoesExistAsync(ValidStudentId);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Failed to verify student existence due to a system error");
    //        _mockLogger.Verify(l => l.LogError(
    //            "Database error checking student existence", exception, It.IsAny<object>()), Times.Once);
    //    }
    //    #endregion

    //    #region DoesExistAsync (by LastName)
    //    [Fact]
    //    public async Task DoesExistAsync_ByLastName_WhenStudentExists_ReturnsSuccess()
    //    {
    //        // Arrange
    //        _mockRepository.Setup(r => r.DoesExistAsync(ValidLastName)).ReturnsAsync(true);

    //        // Act
    //        var result = await _service.DoesExistAsync(ValidLastName);

    //        // Assert
    //        result.IsSuccess.Should().BeTrue();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task DoesExistAsync_ByLastName_WhenLastNameIsNullOrEmpty_ReturnsFailure()
    //    {
    //        // Act
    //        var result = await _service.DoesExistAsync("");

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Last name is required");
    //        _mockRepository.VerifyNoOtherCalls();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task DoesExistAsync_ByLastName_WhenStudentDoesNotExist_ReturnsFailure()
    //    {
    //        // Arrange
    //        _mockRepository.Setup(r => r.DoesExistAsync(ValidLastName)).ReturnsAsync(false);

    //        // Act
    //        var result = await _service.DoesExistAsync(ValidLastName);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Student not found with the specified last name");
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task DoesExistAsync_ByLastName_WhenRepositoryThrowsException_ReturnsFailureAndLogsError()
    //    {
    //        // Arrange
    //        var exception = new Exception("Database error");
    //        _mockRepository.Setup(r => r.DoesExistAsync(ValidLastName)).ThrowsAsync(exception);
    //        _mockLogger.Setup(l => l.LogError(It.IsAny<string>(), new Exception(), It.IsAny<object>()));

    //        // Act
    //        var result = await _service.DoesExistAsync(ValidLastName);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Failed to verify student existence due to a system error");
    //        _mockLogger.Verify(l => l.LogError(
    //            "Database error checking student existence", exception, It.IsAny<object>()), Times.Once);
    //    }
    //    #endregion

    //    #region GetByIdAsync
    //    [Fact]
    //    public async Task GetByIdAsync_WhenStudentExists_ReturnsSuccessWithStudentDto()
    //    {
    //        // Arrange
    //        var student = CreateTestStudent();
    //        var studentDto = CreateTestStudentDto();

    //        _mockRepository.Setup(r => r.GetByIdAsync(ValidStudentId)).ReturnsAsync(student);
    //        _mockMapper.Setup(m => m.Map<StudentDto>(student)).Returns(studentDto);

    //        // Act
    //        var result = await _service.GetByIdAsync(ValidStudentId);

    //        // Assert
    //        result.IsSuccess.Should().BeTrue();
    //        result.Value.Should().BeEquivalentTo(studentDto);
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task GetByIdAsync_WhenIdIsZero_ReturnsFailure()
    //    {
    //        // Act
    //        var result = await _service.GetByIdAsync(0);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Invalid student ID provided");
    //        _mockRepository.VerifyNoOtherCalls();
    //        _mockMapper.VerifyNoOtherCalls();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task GetByIdAsync_WhenStudentDoesNotExist_ReturnsFailure()
    //    {
    //        // Arrange
    //        _mockRepository.Setup(r => r.GetByIdAsync(ValidStudentId)).ReturnsAsync((Student)null!);

    //        // Act
    //        var result = await _service.GetByIdAsync(ValidStudentId);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Student not found with the specified ID");
    //        _mockMapper.VerifyNoOtherCalls();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task GetByIdAsync_WhenRepositoryThrowsException_ReturnsFailureAndLogsError()
    //    {
    //        // Arrange
    //        var exception = new Exception("Database error");
    //        _mockRepository.Setup(r => r.GetByIdAsync(ValidStudentId)).ThrowsAsync(exception);
    //        _mockLogger.Setup(l => l.LogError(It.IsAny<string>(), new Exception(), It.IsAny<object>()));

    //        // Act
    //        var result = await _service.GetByIdAsync(ValidStudentId);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Failed to retrieve student due to a system error");
    //        _mockLogger.Verify(l => l.LogError(
    //            "Database error retrieving student", exception, It.IsAny<object>()), Times.Once);
    //    }
    //    #endregion

    //    #region GetByNameAsync
    //    [Fact]
    //    public async Task GetByNameAsync_WhenStudentExists_ReturnsSuccessWithStudentDto()
    //    {
    //        // Arrange
    //        var student = CreateTestStudent();
    //        var studentDto = CreateTestStudentDto();

    //        _mockRepository.Setup(r => r.GetByNameAsync(ValidLastName)).ReturnsAsync(student);
    //        _mockMapper.Setup(m => m.Map<StudentDto>(student)).Returns(studentDto);

    //        // Act
    //        var result = await _service.GetByNameAsync(ValidLastName);

    //        // Assert
    //        result.IsSuccess.Should().BeTrue();
    //        result.Value.Should().BeEquivalentTo(studentDto);
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task GetByNameAsync_WhenLastNameIsNullOrEmpty_ReturnsFailure()
    //    {
    //        // Act
    //        var result = await _service.GetByNameAsync("");

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Last name is required");
    //        _mockRepository.VerifyNoOtherCalls();
    //        _mockMapper.VerifyNoOtherCalls();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task GetByNameAsync_WhenStudentDoesNotExist_ReturnsFailure()
    //    {
    //        // Arrange
    //        _mockRepository.Setup(r => r.GetByNameAsync(ValidLastName)).ReturnsAsync((Student)null!);

    //        // Act
    //        var result = await _service.GetByNameAsync(ValidLastName);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Student not found with the specified last name");
    //        _mockMapper.VerifyNoOtherCalls();
    //        _mockLogger.VerifyNoOtherCalls();
    //    }

    //    [Fact]
    //    public async Task GetByNameAsync_WhenRepositoryThrowsException_ReturnsFailureAndLogsError()
    //    {
    //        // Arrange
    //        var exception = new Exception("Database error");
    //        _mockRepository.Setup(r => r.GetByNameAsync(ValidLastName)).ThrowsAsync(exception);
    //        _mockLogger.Setup(l => l.LogError(It.IsAny<string>(), new Exception(), It.IsAny<object>()));

    //        // Act
    //        var result = await _service.GetByNameAsync(ValidLastName);

    //        // Assert
    //        result.IsSuccess.Should().BeFalse();
    //        result.Error.Should().Be("Failed to retrieve student due to a system error");
    //        _mockLogger.Verify(l => l.LogError(
    //            "Database error retrieving student", exception, It.IsAny<object>()), Times.Once);
    //    }
    //    #endregion

    //    #region Private Helpers
    //    private Student CreateTestStudent() => new Student
    //    {
    //        Id = ValidStudentId,
    //        FName = "John",
    //        LName = ValidLastName
    //    };

    //    private StudentDto CreateTestStudentDto() => new StudentDto
    //    {
    //        Id = ValidStudentId,
    //        FName = "John",
    //        LName = ValidLastName,
    //    };
    //    #endregion
    //}
}