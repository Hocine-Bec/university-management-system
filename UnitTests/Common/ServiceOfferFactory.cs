using Bogus;
using Domain.Entities;

namespace UnitTests.Common;

public static class ServiceOfferFactory
{
    public static List<ServiceOffer> CreateTestServiceOffers(int count, int? seed = null)
    {
        var faker = new Faker<ServiceOffer>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        faker.RuleFor(s => s.Id, f => f.IndexFaker + 1)
            .RuleFor(s => s.Name, f => f.Commerce.ProductName())
            .RuleFor(s => s.Description, f => f.Lorem.Sentence())
            .RuleFor(s => s.Fees, f => f.Finance.Amount(10, 500))
            .RuleFor(s => s.IsActive, f => f.Random.Bool(0.8f));

        return faker.Generate(count);
    }
}