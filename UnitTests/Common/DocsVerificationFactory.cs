using Bogus;
using Domain.Entities;
using Domain.Enums;
using Person = Domain.Entities.Person;

namespace UnitTests.Common;

public static class DocsVerificationFactory
{
    public static List<DocsVerification> CreateTestDocsVerifications(
        int count,
        List<Person> people,
        List<User> users,
        int? seed = null)
    {
        var faker = new Faker<DocsVerification>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        var shuffledPeople = people.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var shuffledUsers = users.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var index = 0;

        faker.RuleFor(d => d.Id, f => f.IndexFaker + 1)
            .RuleFor(d => d.SubmissionDate, f => f.Date.Past(1))
            .RuleFor(d => d.VerificationDate, (f, d) => 
                f.Random.Bool(0.7f) ? f.Date.Between(d.SubmissionDate, DateTime.Now) : null)
            .RuleFor(d => d.Status, f => f.PickRandom<VerificationStatus>())
            .RuleFor(d => d.IsApproved, (f, d) => 
                d.Status == VerificationStatus.Approved ? true :
                d.Status == VerificationStatus.Rejected ? false : null)
            .RuleFor(d => d.RejectedReason, (f, d) => 
                d.Status == VerificationStatus.Rejected ? f.Lorem.Sentence() : null)
            .RuleFor(d => d.PaidFees, f => f.Finance.Amount(0, 500))
            .RuleFor(d => d.Notes, f => f.Random.Bool(0.5f) ? f.Lorem.Sentence() : null)
            .FinishWith((f, d) =>
            {
                d.PersonId = shuffledPeople[index].Id;
                d.Person = shuffledPeople[index];
                
                if (f.Random.Bool(0.6f) && d.Status != VerificationStatus.Pending)
                {
                    d.VerifiedByUserId = shuffledUsers[index].Id;
                    d.VerifiedByUser = shuffledUsers[index];
                }
                index++;
            });

        return faker.Generate(count);
    }
}