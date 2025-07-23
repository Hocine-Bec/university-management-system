using Bogus;
using Domain.Entities;
using Domain.Enums;
using Person = Domain.Entities.Person;

namespace UnitTests.Common;

public static class ServiceApplicationFactory
{
    public static List<ServiceApplication> CreateTestServiceApplications(
        int count,
        List<Person> people,
        List<ServiceOffer> serviceOffers,
        List<User> users,
        int? seed = null)
    {
        if (count > people.Count || count > serviceOffers.Count)
            throw new ArgumentException("Cannot create more applications than available people or service offers");

        var faker = new Faker<ServiceApplication>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        var shuffledPeople = people.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var shuffledServices = serviceOffers.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var userPool = users.Where(u => u.IsActive).ToList();
        var index = 0;

        faker.RuleFor(a => a.Id, f => f.IndexFaker + 1)
            .RuleFor(a => a.ApplicationDate, f => f.Date.Recent(30))
            .RuleFor(a => a.Status, f => f.PickRandom<ApplicationStatus>())
            .RuleFor(a => a.PaidFees, (f, a) => 
                a.Status == ApplicationStatus.Completed ? 
                f.Random.Decimal(0, shuffledServices[index].Fees) : 0)
            .RuleFor(a => a.Notes, f => f.Random.Bool(0.3f) ? f.Lorem.Sentence() : null)
            .RuleFor(a => a.CompletedDate, (f, a) => 
                a.Status == ApplicationStatus.Completed ? 
                f.Date.Between(a.ApplicationDate, DateTime.Now) : null)
            .FinishWith((f, a) =>
            {
                a.PersonId = shuffledPeople[index].Id;
                a.Person = shuffledPeople[index];
                a.ServiceOfferId = shuffledServices[index].Id;
                a.ServiceOffer = shuffledServices[index];

                if (a.Status != ApplicationStatus.New && userPool.Any())
                {
                    a.ProcessedByUserId = f.PickRandom(userPool).Id;
                }
                index++;
            });

        return faker.Generate(count);
    }
}