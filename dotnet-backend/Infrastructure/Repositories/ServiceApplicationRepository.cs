using Applications.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ServiceApplicationRepository(AppDbContext context) : GenericRepository<ServiceApplication>(context), IServiceApplicationRepository
{
    // Additional methods specific to Program can be added here
    public async Task<bool> DoesPersonHaveActiveApplicationsAsync(int personId, int serviceOfferId)
    {
        return await _context.ServiceApplications
            .AsNoTracking()
            .AnyAsync(x => x.PersonId == personId && 
                           x.ServiceOfferId == serviceOfferId && 
                           (x.Status == ApplicationStatus.New || x.Status == ApplicationStatus.InProgress));
    }

    public override async Task<IReadOnlyCollection<ServiceApplication>> GetListAsync()
    {
        return await _context.ServiceApplications
            .AsNoTracking()
            .Include(x => x.Person)
            .Include(x => x.ServiceOffer)
            .ToListAsync();
    }

    public override async Task<ServiceApplication?> GetByIdAsync(int id)
    {
        return await _context.ServiceApplications
            .AsNoTracking()
            .Include(x => x.Person)
            .Include(x => x.ServiceOffer)
            .Include(x => x.ProcessedByUser)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ServiceApplication?> GetByPersonIdAsync(int personId)
    {
        return await _context.ServiceApplications
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PersonId == personId);
    }

    public async Task<bool> DoesExistsAsync(int id)
    {
        return await _context.ServiceApplications.AnyAsync(x => x.Id == id);
    }
}