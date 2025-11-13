using Bogus;
using FluentAssertions;
using ToonNet.Models;

namespace ToonNet.Tests.Models;

public class ToonDocumentTests
{
    [Fact]
    public void ToString_Should_Render_Valid_TOON_Format()
    {
        // Arrange
        var doc = new ToonDocument
        {
            RootName = "users",
            Fields = new List<string> { "Id", "Name", "Role" },
            Rows = new List<string[]>
            {
                new[] { "1", "Alice", "admin" },
                new[] { "2", "Bob", "user" }
            }
        };

        // Act
        var result = doc.ToString();

        // Assert
        result.Should().Be(
            @"users[2]{Id,Name,Role}:
  1,Alice,admin
  2,Bob,user");
    }

    [Fact]
    public void ToString_Should_Handle_Empty_Document()
    {
        // Arrange
        var doc = new ToonDocument
        {
            RootName = "empty",
            Fields = new List<string>(),
            Rows = new List<string[]>()
        };

        // Act
        var result = doc.ToString();

        // Assert
        result.Should().Be("empty[0]{}:");
    }

    [Fact]
    public void Count_Should_Return_Number_Of_Rows()
    {
        // Arrange
        var doc = new ToonDocument
        {
            Rows = new List<string[]>
            {
                new[] { "1" },
                new[] { "2" },
                new[] { "3" }
            }
        };

        // Act
        var count = doc.Count;

        // Assert
        count.Should().Be(3);
    }
    
    [Fact]
        public void Parse_Should_Parse_Valid_Toon_Text()
        {
            // Arrange
            var toonText = @"users[2]{Id,Name,Role}:
  1,Alice,admin
  2,Bob,user";

            // Act
            var document = ToonDocument.Parse(toonText);

            // Assert
            document.RootName.Should().Be("users");
            document.Fields.Should().BeEquivalentTo(new[] { "Id", "Name", "Role" });
            document.Rows.Should().HaveCount(2);

            document.Rows[0][0].Should().Be("1");
            document.Rows[0][1].Should().Be("Alice");
            document.Rows[0][2].Should().Be("admin");

            document.Rows[1][0].Should().Be("2");
            document.Rows[1][1].Should().Be("Bob");
            document.Rows[1][2].Should().Be("user");
        }

        [Fact]
        public void Parse_Should_Throw_When_Input_Is_Null_Or_Empty()
        {
            // Arrange
            string? empty = null;

            // Act
            Action act1 = () => ToonDocument.Parse(empty!);
            Action act2 = () => ToonDocument.Parse(string.Empty);

            // Assert
            act1.Should().Throw<ArgumentException>();
            act2.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Parse_Should_Throw_When_Header_Is_Invalid()
        {
            // Arrange
            var invalidHeader = @"users{Id,Name,Role}:
  1,Alice,admin";

            // Act
            Action act = () => ToonDocument.Parse(invalidHeader);

            // Assert
            act.Should().Throw<FormatException>()
                .WithMessage("Invalid TOON header format.");
        }

        [Fact]
        public void Parse_Should_Throw_When_Row_Does_Not_Match_Field_Count()
        {
            // Arrange
            var invalidRow = @"users[1]{Id,Name,Role}:
  1,Alice";

            // Act
            Action act = () => ToonDocument.Parse(invalidRow);

            // Assert
            act.Should().Throw<FormatException>()
                .WithMessage("Row*does not match field count*");
        }

        [Fact]
        public void Parse_Should_Throw_When_Declared_Count_Differs_From_Actual_Rows()
        {
            // Arrange
            var toonText = @"users[3]{Id,Name,Role}:
  1,Alice,admin
  2,Bob,user";

            // Act
            Action act = () => ToonDocument.Parse(toonText);

            // Assert
            act.Should().Throw<FormatException>()
                .WithMessage("Declared count*does not match actual rows*");
        }

        [Fact]
        public void Parse_Should_Handle_Dynamic_Data_From_Faker()
        {
            // Arrange
            var faker = new Faker();
            var id = faker.Random.Int(1, 100);
            var name = faker.Name.FirstName();
            var role = faker.PickRandom("admin", "user", "guest");

            var toonText = $@"people[1]{{Id,Name,Role}}:
  {id},{name},{role}";

            // Act
            var document = ToonDocument.Parse(toonText);

            // Assert
            document.RootName.Should().Be("people");
            document.Rows.Should().ContainSingle();
            document.Rows[0].Should().BeEquivalentTo(new[] { id.ToString(), name, role });
        }
}