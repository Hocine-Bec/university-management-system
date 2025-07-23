using Applications.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ServiceOfferRepository(AppDbContext context) : GenericRepository<ServiceOffer>(context), IServiceOfferRepository
{
    public async Task<bool> DoesExistAsync(string name)
    {
        return await _context.ServiceOffers.AnyAsync(x => x.Name == name);
    }
}