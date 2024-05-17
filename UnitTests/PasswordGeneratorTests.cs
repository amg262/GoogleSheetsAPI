using GoogleSheetsAPI.Middleware;
using System;
using System.Linq;
using Xunit;

namespace UnitTests;

/// <summary>
/// Contains unit tests for the <see cref="PasswordGenerator"/> class, ensuring it handles
/// input parameters correctly and generates passwords that meet specified requirements.
/// </summary>
public class PasswordGeneratorTests
{
    private readonly PasswordGenerator _passwordGenerator = new PasswordGenerator();

    /// <summary>
    /// Tests that the <see cref="PasswordGenerator.Generate(int, int, int, bool, bool)"/> method
    /// throws an <see cref="ArgumentException"/> when the total number of digits and symbols requested
    /// exceeds the specified length of the password.
    /// </summary>
    [Fact]
    public void Generate_ThrowsArgumentException_WhenLengthLessThanSumOfDigitsAndSymbols()
    {
        // Arrange
        const int length = 5;
        const int numberOfDigits = 3;
        const int numberOfSymbols = 3;

        // Act & Assert
        var exception =
            Assert.Throws<ArgumentException>(() =>
                _passwordGenerator.Generate(length, numberOfDigits, numberOfSymbols));
        Assert.Contains("Number of digits and symbols must be less than length", exception.Message);
    }

    /// <summary>
    /// Tests that the <see cref="PasswordGenerator.Generate(int, int, int, bool, bool)"/> method
    /// correctly generates passwords of the specified length, with the exact number of digits and symbols,
    /// respecting the noUpperCase and allowRepeatedCharacters options.
    /// </summary>
    /// <param name="length">The total length of the password to generate.</param>
    /// <param name="numberOfDigits">The number of digits to include in the password.</param>
    /// <param name="numberOfSymbols">The number of symbols to include in the password.</param>
    /// <param name="noUpperCase">Whether to exclude uppercase letters from the password.</param>
    /// <param name="allowRepeatedCharacters">Whether to allow characters to be repeated in the password.</param>
    [Theory]
    [InlineData(10, 2, 2, false, true)] // Normal conditions
    [InlineData(12, 3, 1, true, false)] // No uppercase and no repeated characters
    public void Generate_ReturnsPassword_OfCorrectLengthAndComposition(
        int length, int numberOfDigits, int numberOfSymbols, bool noUpperCase, bool allowRepeatedCharacters)
    {
        // Act
        var password = _passwordGenerator.Generate(length, numberOfDigits, numberOfSymbols, noUpperCase,
            allowRepeatedCharacters);

        // Assert
        Assert.Equal(length, password.Length);
        var digitCount = CountChars(password, char.IsDigit);
        var symbolCount = CountChars(password, c => !char.IsLetterOrDigit(c));
        var upperCaseCount = CountChars(password, char.IsUpper);

        Assert.Equal(numberOfDigits, digitCount);
        Assert.Equal(numberOfSymbols, symbolCount);
        if (noUpperCase)
        {
            Assert.Equal(0, upperCaseCount);
        }
        else
        {
            Assert.True(upperCaseCount > 0); // Assumes there's at least one uppercase if allowed
        }

        // Verify no repeated characters if not allowed
        if (!allowRepeatedCharacters)
        {
            Assert.True(HasAllUniqueCharacters(password));
        }
    }

    /// <summary>
    /// Counts characters in a string that meet a specific condition.
    /// </summary>
    /// <param name="input">The string to scan.</param>
    /// <param name="predicate">The condition to test each character against.</param>
    /// <returns>The count of characters that meet the condition.</returns>
    private static int CountChars(string input, Func<char, bool> predicate)
    {
        return input.Count(predicate);
    }

    /// <summary>
    /// Determines if all characters in the provided string are unique.
    /// </summary>
    /// <param name="input">The string to check for uniqueness.</param>
    /// <returns>True if all characters are unique, otherwise false.</returns>
    private static bool HasAllUniqueCharacters(string input)
    {
        var seen = new HashSet<char>();
        return input.All(c => seen.Add(c));
    }
}