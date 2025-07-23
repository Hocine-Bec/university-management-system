using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories;

public class EntranceExamRepository(AppDbContext context) : GenericRepository<EntranceExam>(context), IEntranceExamRepository
{
    // Additional methods specific to Program can be added here
}