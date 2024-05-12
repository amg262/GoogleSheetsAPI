using System.Security.Cryptography;

namespace GoogleSheetsAPI.Middleware;

public class ApiKeyGenerator
{
    /// <summary>
    /// Generates a secure API key.
    /// </summary>
    /// <param name="keySize">Size of the API key in bytes. Recommended size is at least 32 bytes (256 bits).</param>
    /// <returns>A base64 encoded string that represents the generated secure API key.</returns>
    public static string GenerateSecureApiKey(int keySize = 64)
    {
        // using var rng = new RNGCryptoServiceProvider();
        byte[] keyBytes = new byte[keySize];
        RandomNumberGenerator.Fill(keyBytes);
        // rng.GetBytes(keyBytes);
        return Convert.ToBase64String(keyBytes);
    }
}