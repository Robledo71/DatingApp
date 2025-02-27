using API.Extensions;
using Xunit;
public class DateTimeExtensionsTests
{
    [Theory]
    [InlineData("2024-11-19", 0)] // Fecha futura (mañana)
    [InlineData("2024-11-18", 0)] // Fecha actual (hoy)
    [InlineData("2023-11-18", 1)] // Un año antes de hoy
    [InlineData("2010-12-31", 13)] // Cumpleaños futuro este año
    [InlineData("2010-11-17", 14)] // Cumpleaños ya pasado
    public void CalculateAge_ShouldReturnCorrectAge(string birthDate, int expectedAge)
    {
        // Arrange
        var birthDateOnly = DateOnly.Parse(birthDate);
        // Act
        var age = birthDateOnly.CalculateAge();
        // Assert
        Assert.Equal(expectedAge, age);
    }
}