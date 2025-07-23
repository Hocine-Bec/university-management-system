using Applications.DTOs.People;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMyLogger _logger;

        public PersonService(IPersonRepository repository, IMapper mapper, IMyLogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task<Result<PersonResponse>> AddAsync(PersonRequest request)
        {
            if (request == default)
                return Result<PersonResponse>.Failure("Student information is required", ErrorType.BadRequest);
            
            try
            {
                bool isExists = await _repository.DoesExistAsync(request.LastName ?? string.Empty);
                if(isExists)
                    return Result<PersonResponse>.Failure("Person already exists", ErrorType.Conflict);
            
                var person = _mapper.Map<Person>(request);
                await _repository.AddAsync(person);
            
                if (person.Id <= 0)
                    return Result<PersonResponse>.Failure("Failed to create new person record", ErrorType.BadRequest);

                var response = _mapper.Map<PersonResponse>(person);
                return Result<PersonResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error adding new person", ex, new { request });
                return Result<PersonResponse>.Failure("Failed to create person due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result<IReadOnlyCollection<PersonResponse>>> GetListAsync()
        {
            try
            {
                //Refactor later to use AutoMapper Projection when the database grows
                var people = await _repository.GetListAsync();
                if (!people.Any())
                {
                    return Result<IReadOnlyCollection<PersonResponse>>.Failure(
                        "No people records found in the system", ErrorType.NotFound);
                }

                var response = _mapper.Map<IReadOnlyCollection<PersonResponse>>(people);
                return Result<IReadOnlyCollection<PersonResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving all people", ex);

                return Result<IReadOnlyCollection<PersonResponse>>
                    .Failure("Failed to retrieve people due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result<PersonResponse>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return Result<PersonResponse>.Failure("Invalid person ID provided", ErrorType.BadRequest);

            try
            {
                var person = await _repository.GetByIdAsync(id);
                if (person == null)
                    return Result<PersonResponse>.Failure("Person not found with the specified ID", ErrorType.NotFound);

                var response = _mapper.Map<PersonResponse>(person);
                return Result<PersonResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving student", ex, new { id });
                return Result<PersonResponse>.Failure("Failed to retrieve student due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result<PersonResponse>> GetByNameAsync(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
                return Result<PersonResponse>.Failure("Last name is required", ErrorType.BadRequest);

            try
            {
                var person = await _repository.GetByNameAsync(lastName);

                if (person == null)
                {
                    return Result<PersonResponse>.Failure(
                        "Person not found with the specified last name", ErrorType.NotFound);
                }

                var response = _mapper.Map<PersonResponse>(person);

                return Result<PersonResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving person", ex, new { lastName });
                return Result<PersonResponse>.Failure("Failed to retrieve person due to a system error",
                    ErrorType.InternalServerError);
            }
        }

        public async Task<Result> UpdateAsync(int id, PersonRequest request)
        {
            if (id <= 0)
                return Result.Failure("Invalid person ID provided", ErrorType.BadRequest);
            
            if (request == default)
                return Result.Failure("Person information is required for update", ErrorType.BadRequest);
            
            try
            {
                var person = await _repository.GetByIdAsync(id);
                if (person == null)
                    return Result.Failure("Person Already Exists", ErrorType.Conflict);

                _mapper.Map(request, person);
                person.Id = id;
                
                bool isUpdated = await _repository.UpdateAsync(person);
                return !isUpdated ? Result.Failure($"Failed to update person", ErrorType.BadRequest) : Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error updating person", ex, new { request });
                return Result.Failure("Failed to update person due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            if (id <= 0)
                return Result.Failure("Invalid person ID provided", ErrorType.BadRequest);

            try
            {
                bool isDeleted = await _repository.DeleteAsync(id);
                return !isDeleted ? Result.Failure("Person not found", ErrorType.NotFound) : Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error deleting person", ex, new { id });
                return Result.Failure("Failed to delete person due to a system error", ErrorType.InternalServerError);
            }

        }

        public async Task<Result> DeleteAsync(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
                return Result.Failure("Last name is required", ErrorType.BadRequest);

            try
            {
                bool isDeleted = await _repository.DeleteAsync(lastName);
                return !isDeleted ? Result.Failure("Person not found", ErrorType.NotFound) : Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error deleting person", ex, new { lastName });
                return Result.Failure("Failed to delete person due to a system error", ErrorType.InternalServerError);
            }
        }
    }
}
