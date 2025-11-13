using Bogus;
using ToonNet.Tests.Entities;

namespace ToonNet.Tests.Fixtures;

public static class UserFaker
{
    public static List<User> Generate(int count = 5)
    {
        var faker = new Faker<User>("pt_BR")
            .RuleFor(u => u.Id, f => f.IndexFaker + 1)
            .RuleFor(u => u.Name, f => f.Name.FirstName())
            .RuleFor(u => u.Role, f => f.PickRandom("admin", "user", "guest"));

        return faker.Generate(count);
    }
}