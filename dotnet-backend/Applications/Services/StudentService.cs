using Applications.DTOs.Student;
using Applications.Helpers;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMyLogger _logger;

        public StudentService(IStudentRepository repository, IMapper mapper, IMyLogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyCollection<StudentResponse>>> GetListAsync()
        {
            try
            {
                //Refactor later to use AutoMapper Projection when the database grows
                var students = await _repository.GetListAsync();
                if (!students.Any())
                {
                    return Result<IReadOnlyCollection<StudentResponse>>.Failure(
                        "No students found in the system", ErrorType.NotFound);
                }

                var response = _mapper.Map<IReadOnlyCollection<StudentResponse>>(students);
                return Result<IReadOnlyCollection<StudentResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving all student", ex);

                return Result<IReadOnlyCollection<StudentResponse>>
                    .Failure("Failed to retrieve students due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result<StudentResponse>> AddAsync(StudentRequest request)
        {
            if (request == default)
                return Result<StudentResponse>.Failure("Student information is required", ErrorType.BadRequest);
            
            try
            {
                var isExist = await _repository.DoesExistAsync(request.PersonId);
                if (!isExist)
                    return Result<StudentResponse>.Failure("Student already exists", ErrorType.Conflict);

                var student = _mapper.Map<Student>(request);
                student.StudentNumber = student.GenerateUniqueNumber();
                
                int id = await _repository.AddAsync(student);
                if (id <= 0)
                    return Result<StudentResponse>.Failure("Failed to create new student record", ErrorType.BadRequest);

                var response = _mapper.Map<StudentResponse>(student);
                return Result<StudentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error adding new student", ex, new { request });
                return Result<StudentResponse>.Failure("Failed to create student due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result> UpdateAsync(int id, StudentRequest request)
        {
            if (request == default)
                return Result.Failure("Student information is required for update", ErrorType.BadRequest);

            try
            {
                var student = await _repository.GetByIdAsync(id);
                if (student == null)
                    return Result.Failure("Student Not Found", ErrorType.NotFound);

                _mapper.Map(request, student);
                student.Id = id;
                
                bool isUpdated = await _repository.UpdateAsync(student);
                return !isUpdated ? Result.Failure($"Failed to update student", ErrorType.BadRequest) : Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error updating student", ex, new { request });
                return Result.Failure("Failed to update student due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            if (id <= 0)
                return Result.Failure("Invalid student ID provided", ErrorType.BadRequest);

            try
            {
                bool isDeleted = await _repository.DeleteAsync(id);
                return !isDeleted ? Result.Failure("Student not found", ErrorType.NotFound) : Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error deleting student", ex, new { id });
                return Result.Failure("Failed to delete student due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result> DeleteAsync(string studentNumber)
        {
            if (string.IsNullOrEmpty(studentNumber))
                return Result.Failure("Student number is required", ErrorType.BadRequest);

            try
            {
                bool isDeleted = await _repository.DeleteAsync(studentNumber);
                return !isDeleted ? Result.Failure("Student not found", ErrorType.NotFound) : Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error deleting student", ex, new { studentNumber });
                return Result.Failure("Failed to delete student due to a system error", ErrorType.InternalServerError);
            }
        }
        
        public async Task<Result<StudentResponse>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return Result<StudentResponse>.Failure("Invalid student ID provided", ErrorType.BadRequest);

            try
            {
                var student = await _repository.GetByIdAsync(id);

                if (student == null)
                    return Result<StudentResponse>.Failure("Student not found with the specified ID", ErrorType.NotFound);

                var response = _mapper.Map<StudentResponse>(student);
                return Result<StudentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving student", ex, new { id });
                return Result<StudentResponse>.Failure("Failed to retrieve student due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result<StudentResponse>> GetByStudentNumberAsync(string studentNumber)
        {
            if (string.IsNullOrEmpty(studentNumber))
                return Result<StudentResponse>.Failure("Student number is required", ErrorType.BadRequest);

            try
            {
                var student = await _repository.GetByStudentNumberAsync(studentNumber);

                if (student == null)
                {
                    return Result<StudentResponse>.Failure(
                        "Student not found with the specified last name", ErrorType.NotFound);
                }

                var response = _mapper.Map<StudentResponse>(student);
                return Result<StudentResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving student", ex, new { studentNumber });
                return Result<StudentResponse>.Failure("Failed to retrieve student due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result> UpdateStudentStatusAsync(int id, UpdateStudentStatusRequest status)
        {
            if (id <= 0)
                return Result.Failure("Invalid student ID provided", ErrorType.BadRequest);

            if(!status.StudentStatus.HasValue)
                return Result.Failure("No Status Value Provided", ErrorType.BadRequest);

            try
            {
                var student = await _repository.GetByIdAsync(id);

                if (student == null)
                    return Result.Failure($"Student not found", ErrorType.NotFound);

                student.StudentStatus = status.StudentStatus.Value;
                student.Notes = string.IsNullOrEmpty(status.Notes) ? student.Notes : status.Notes;

                var isUpdated = await _repository.UpdateAsync(student);
                return !isUpdated ? Result.Failure($"Failed to update student status", ErrorType.BadRequest) : Result.Success;
            }
            catch (Exception ex)
            {
                var statusName = status.ToString();
                _logger.LogError("Database error updating student", ex, new { statusName });
                return Result.Failure("Failed to update student due to a system error", ErrorType.InternalServerError);
            }
        }
    }
}

