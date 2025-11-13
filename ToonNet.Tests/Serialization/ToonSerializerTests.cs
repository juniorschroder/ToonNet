using FluentAssertions;
using ToonNet.Serialization;
using ToonNet.Tests.Entities;
using ToonNet.Tests.Fixtures;

namespace ToonNet.Tests.Serialization;

public class ToonSerializerTests
{
    [Fact]
    public void Should_Serialize_List_Of_Objects_To_TOON_Format()
    {
        // Arrange
        var users = UserFaker.Generate(2);
        var root = "users";

        // Act
        var toon = ToonSerializer.Serialize(users, root);

        // Assert
        toon.Should().StartWith("users[2]{Id,Name,Role}:");
        toon.Should().Contain(users[0].Name);
        toon.Should().Contain(users[1].Role);
    }

    [Fact]
    public void Should_Serialize_Empty_List_As_Empty_TOON()
    {
        // Arrange
        var users = new List<User>();

        // Act
        var toon = ToonSerializer.Serialize(users, "users");

        // Assert
        toon.Should().Be("users[0]{}:");
    }
}