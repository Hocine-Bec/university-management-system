using Applications.Interfaces.Base;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Interfaces.Repositories;

public interface IServiceOfferRepository : IGenericRepository<ServiceOffer>
{
    Task<bool> DoesExistAsync(string name);
}