using FluentAssertions;
using ToonNet.Serialization;
using ToonNet.Tests.Entities;

namespace ToonNet.Tests.Serialization;

public class ToonDeserializerTests
{
    [Fact]
    public void Should_Deserialize_TOON_String_To_List_Of_Objects()
    {
        // Arrange
        var toon = @"users[2]{Id,Name,Role}:
              1,Alice,admin
              2,Bob,user";

        // Act
        var result = ToonDeserializer.Deserialize<User>(toon);

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Alice");
        result[1].Role.Should().Be("user");
    }

    [Fact]
    public void Should_Handle_Escaped_Values()
    {
        // Arrange
        var toon = @"users[1]{Id,Name,Role}:
            1,Alice\,Wonderland,admin";

        // Act
        var result = ToonDeserializer.Deserialize<User>(toon);

        // Assert
        result.Should().ContainSingle();
        result[0].Name.Should().Be("Alice,Wonderland");
    }

    [Fact]
    public void Should_Throw_FormatException_For_Invalid_Input()
    {
        // Arrange
        var invalidToon = "invalid text";

        // Act
        var act = () => ToonDeserializer.Deserialize<User>(invalidToon);

        // Assert
        act.Should().Throw<FormatException>();
    }
}