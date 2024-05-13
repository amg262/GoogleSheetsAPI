using System.Security.Cryptography;

namespace GoogleSheetsAPI.Middleware;

/// <summary>
/// Provides functionality to generate a secure API key using cryptographic random number generation.
/// </summary>
public class ApiKeyGenerator
{
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