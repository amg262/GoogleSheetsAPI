using System.Security.Cryptography;
using System.Text.Json;

namespace GoogleSheetsAPI.Helpers;

/// <summary>
/// Provides helper methods for working with API data and JSON values.
/// </summary>
public static class ApiHelper
{
    /// <summary>
    /// Converts a JsonElement object to an appropriate .NET object type.
    /// </summary>
    /// <param name="obj">The object to convert, typically a JsonElement.</param>
    /// <returns>
    /// The converted object as a .NET type. If the conversion fails, returns the exception message.
    /// Possible return types are string, float, bool, or null.
    /// </returns>
    /// <remarks>
    /// This method attempts to determine the type of the JSON element and convert it to a corresponding .NET type.
    /// It handles various JSON value kinds such as Number, String, True, False, Null, Undefined, Object, and Array.
    /// If the conversion fails, it catches the exception and returns the exception message.
    /// </remarks>
    public static object? GetObjectValue(object obj)
    {
        try
        {
            if (obj == null) return "NULL";

            var jsonElement = (JsonElement)obj;
            var rawText = jsonElement.GetRawText();
            var typeOfObject = ((JsonElement)obj).ValueKind;

            return typeOfObject switch
            {
                JsonValueKind.Number => float.Parse(rawText), // return long.Parse(obj.ToString());
                JsonValueKind.String => rawText.ToString(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                JsonValueKind.Undefined => null,
                JsonValueKind.Object => rawText.ToString(),
                JsonValueKind.Array => rawText.ToString(),
                _ => rawText.ToString()
            };
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    /// Generates a secure API key as a base64 encoded string.
    /// </summary>
    /// <param name="keySize">Size of the API key in bytes. A size of 64 bytes (512 bits) is used by default.
    /// It is recommended to use a size of at least 32 bytes (256 bits) for adequate security.</param>
    /// <returns>A base64 encoded string that represents the generated secure API key.</returns>
    /// <remarks>
    /// This method uses the <see cref="RandomNumberGenerator"/> class to fill an array of bytes
    /// with a cryptographically strong random sequence of values, which is then encoded to a base64 string.
    /// This method is static and thread-safe under all platforms.
    /// </remarks>
    public static string GenerateSecureApiKey(int keySize = 64)
    {
        // using var rng = new RNGCryptoServiceProvider();
        byte[] keyBytes = new byte[keySize];
        RandomNumberGenerator.Fill(keyBytes);
        // rng.GetBytes(keyBytes);
        return Convert.ToBase64String(keyBytes);
    }
}