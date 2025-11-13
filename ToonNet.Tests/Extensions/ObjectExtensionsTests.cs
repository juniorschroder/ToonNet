using FluentAssertions;
using ToonNet.Extensions;

namespace ToonNet.Tests.Extensions;

public class ObjectExtensionsTests
{
    private class SampleObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Ignored => $"{Name}-{Id}";
    }

    [Fact]
    public void GetReadableProperties_Should_Return_All_Readable_Public_Properties()
    {
        // Arrange
        var sample = new SampleObject
        {
            Id = 1,
            Name = "Alice",
            CreatedAt = new DateTime(2025, 1, 1)
        };

        // Act
        var props = sample.GetReadableProperties();

        // Assert
        props.Should().ContainKeys("Id", "Name", "CreatedAt");
        props["Id"].Should().Be(1);
        props["Name"].Should().Be("Alice");
        props["CreatedAt"].Should().Be(new DateTime(2025, 1, 1));
    }

    [Fact]
    public void GetReadableProperties_Should_Throw_When_Object_Is_Null()
    {
        // Arrange
        object? obj = null;

        // Act
        var act = () => obj.GetReadableProperties();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("hello,world", "hello\\,world")]
    [InlineData("simple text", "simple text")]
    [InlineData(null, "")]
    public void EscapeToonValue_Should_Escape_Commas_And_Handle_Nulls(string input, string expected)
    {
        // Act
        var result = input.EscapeToonValue();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("123", typeof(int), 123)]
    [InlineData("true", typeof(bool), true)]
    [InlineData("42.5", typeof(double), 42.5)]
    [InlineData("", typeof(int), 0)]
    [InlineData(null, typeof(string), null)]
    public void ConvertTo_Should_Convert_Strings_To_Target_Types(string input, Type targetType, object expected)
    {
        // Act
        var result = input.ConvertTo(targetType);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ConvertTo_Should_Return_Default_For_ValueType_When_Null_Or_Empty()
    {
        // Act
        var result1 = "".ConvertTo(typeof(int));
        var result2 = ((string)null).ConvertTo(typeof(DateTime));

        // Assert
        result1.Should().Be(0);
        result2.Should().Be(default(DateTime));
    }

    [Fact]
    public void ConvertTo_Should_Handle_ReferenceType_Null()
    {
        // Act
        var result = ((string)null).ConvertTo(typeof(string));

        // Assert
        result.Should().BeNull();
    }
}